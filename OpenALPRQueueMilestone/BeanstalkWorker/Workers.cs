using OpenALPRQueueConsumer.Milestone;
using OpenALPRQueueConsumer.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Turbocharged.Beanstalk;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;

namespace OpenALPRQueueConsumer.BeanstalkWorker
{
    internal class Worker
    {
        public static int EventExpireAfterDays = 7;
        public static int EpochStartSecondsBefore = 3;
        public static int EpochEndSecondsAfter = 3;
        private IDictionary<string, string> dicBlack;
        private DateTime lastUpdateTime;
        private IDisposable worker = null;
        private IList<OpenALPRmilestoneCameraName> cameraList;
        private List<KeyValuePair<string,string>> openALPRList;

        public Worker()
        {
            cameraList = new List<OpenALPRmilestoneCameraName>();
            CameraMapper.FillCameraList(cameraList);

            openALPRList = new List<KeyValuePair<string, string>>();
            OpenALPRLNameHelper.FillCameraNameList(openALPRList);

            dicBlack = new Dictionary<string, string>();
            AlertListHelper.LoadAlertList(dicBlack);
            lastUpdateTime = AlertListHelper.GetLastWriteTime();
        }

        public async Task DoWork()
        {
            var options = new WorkerOptions
            {
                Tubes = new List<string>(1) { "alprd" },
                //TaskScheduler = TaskScheduler.Current,
            };

            var beanstalkAddress = Helper.ReadConfigKey("BeanstalkAddress");
            if (string.IsNullOrEmpty(beanstalkAddress))
                beanstalkAddress = "localhost:11300";

            Program.Logger.Log.Info($"Beanstalk address used: {beanstalkAddress}");

            worker = await BeanstalkConnection.ConnectWorkerAsync(beanstalkAddress, options, async (iworker, job) =>
            {
                bool done = true;

                if (job != null)
                {
                    var json = Encoding.UTF8.GetString(job.Data, 0, job.Data.Length);
                    done = ProcessJob(json);
                }
                else
                    Program.Logger.Log.Warn("Job received was null.");

                if (done)
                    await iworker.DeleteAsync();
                else
                    await iworker.BuryAsync(2);
            });
        }

        public bool Test(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(json))
                    return ProcessJob(json);
                else
                    Program.Logger.Log.Info("Empty json data.");
            }

            return false;
        }

        private bool ProcessJob(string json)
        {
            bool done = true;
            var palteInfo = JsonHelper.ToClass<OpenALPRData>(json);

            if (palteInfo != null)
            {
                switch (palteInfo.Data_type)
                {
                    case "alpr_group":
                    case "alpr_results":
                        try
                        {
                            if (!palteInfo.Is_parked)
                                done = ProcessAlprGroupOrResults(palteInfo);
                            else
                                Program.Logger.Log.Info($"{palteInfo.Best_plate_number} is parked, no Bookmark or Alert will be created.");
                        }
                        catch (Exception ex)
                        {
                            done = false;
                            Program.Logger.Log.Error(null, ex);
                        }
                        break;

                    case "heartbeat":
                        var heartbeats = JsonHelper.ToClass<Heartbeats>(json);
                        try
                        {
                            done = ProcessAlprHeartbeat(heartbeats);
                        }
                        catch (Exception ex)
                        {
                            done = false;
                            Program.Logger.Log.Error(null, ex);
                        }
                        break;

                    default:
                        done = true;
                        break;
                }
            }

            return done;
        }


        //http://www.jsonutils.com/
        private bool ProcessAlprGroupOrResults(OpenALPRData palteInfo)
        {
            if (palteInfo != null)
            {
                var candidates = palteInfo.Candidates.GroupBy(candidate => candidate.Plate)
                   .Select(grp => grp.First())
                   .Select(p => p.Plate)
                   .ToList();

                var bestPlateInfo = new PlateInfo
                {
                    DataType = palteInfo.Data_type,
                    CameraId = palteInfo.Camera_id,
                    BestPlateNumber = palteInfo.Best_plate_number,
                    BestRegion = palteInfo.Best_region,
                    CandidatesPlate = string.Join(",", candidates)
                };

                Console.WriteLine($"data_type: {palteInfo.Data_type}");
                Console.WriteLine($"camera id: {palteInfo.Camera_id}");
                Console.WriteLine($"plate Number: {palteInfo.Best_plate_number}");
                Console.WriteLine($"best region: {palteInfo.Best_region}");
                Console.WriteLine($"country: {palteInfo.Country}");
                Console.WriteLine($"candidates: {bestPlateInfo.CandidatesPlate}");

                bestPlateInfo.EpochStart = Epoch2LocalDateTime(palteInfo.Epoch_start);
                Console.WriteLine($"epoch_start: {bestPlateInfo.EpochStart.ToString()}");

                bestPlateInfo.EpochEnd = Epoch2LocalDateTime(palteInfo.Epoch_end);
                Console.WriteLine($"epoch_end: {bestPlateInfo.EpochEnd}");

                if (palteInfo.Vehicle != null)
                {
                    if (palteInfo.Vehicle.Make != null)
                    {
                        var make = palteInfo.Vehicle.Make.OrderByDescending(m => m.Confidence).FirstOrDefault();
                        if (make != null)
                        {
                            bestPlateInfo.Make = make.Name;
                            Console.WriteLine($"make name: {make.Name}, confidence: {make.Confidence}");
                        }
                    }

                    if (palteInfo.Vehicle.Body_type != null)
                    {
                        var body_type = palteInfo.Vehicle.Body_type.OrderByDescending(m => m.Confidence).FirstOrDefault();
                        if (body_type != null)
                        {
                            bestPlateInfo.BodyType = body_type.Name;
                            Console.WriteLine($"body type name: {body_type.Name}, confidence: {body_type.Confidence}");
                        }
                    }

                    if (palteInfo.Vehicle.Make_model != null)
                    {
                        var makeModel = palteInfo.Vehicle.Make_model.OrderByDescending(m => m.Confidence).FirstOrDefault();
                        if (makeModel != null)
                        {
                            bestPlateInfo.MakeModel = makeModel.Name;
                            Console.WriteLine($"model name: {makeModel.Name}  , confidence: {makeModel.Confidence}");
                        }
                    }

                    if (palteInfo.Vehicle.Color != null)
                    {
                        var color = palteInfo.Vehicle.Color.OrderBy(c => c.Confidence).FirstOrDefault();
                        if (color != null)
                        {
                            bestPlateInfo.Color = color.Name;
                            Console.WriteLine($"color name: {color.Name}, confidence: {color.Confidence}");
                        }
                    }
                }
                else
                    Program.Logger.Log.Warn("Vehicle json data returned null object.");
                

                if (!string.IsNullOrEmpty(palteInfo.Best_plate_number) && palteInfo.Camera_id != 0)
                    return AddNewBookmark(bestPlateInfo);
                else
                    Program.Logger.Log.Warn("Best_plate_number is empty or Camera_id == 0");
            }

            return true;
        }

        // Auto mapping happened when there is an ip address in videoStream.Url : rtsp://mhill:cosmos@192.168.0.152/onvif-media / media.amp ? profile = balanced_h264 & sessiontimeout = 60 & streamtype = unicast
        private bool ProcessAlprHeartbeat(Heartbeats heartbeats)
        {
            if (heartbeats != null)
            {
                for (int i = 0; i < heartbeats.Video_streams.Count; i++)
                {
                    var videoStream = heartbeats.Video_streams[i];
                    if (videoStream != null)
                    {
                        var kv = new KeyValuePair<string, string>(videoStream.Camera_id.ToString(), videoStream.Camera_name);
                        if (!openALPRList.Contains(kv))
                        {
                            openALPRList.Add(kv);
                            OpenALPRLNameHelper.SaveCameraNameList(openALPRList);
                        }

                        var cameraId = videoStream.Camera_id;
                        if (!string.IsNullOrEmpty(videoStream.Url)) ////rtsp://mhill:cosmos@192.168.0.152/onvif-media / media.amp ? profile = balanced_h264 & sessiontimeout = 60 & streamtype = unicast
                        {
                            var match = Regex.Match(videoStream.Url, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b");
                            if (match.Success)
                            {
                                // 1st case (No Camera_id found): AXIS M1054 Network Camera (192.168.0.36) - Camera 1|TestCamera|
                                var exists = cameraList.Any(m => m.OpenALPRId == videoStream.Camera_id.ToString()); 

                                if (!exists)
                                {
                                    exists = cameraList.Any(m => m.OpenALPRname == videoStream.Camera_name); 
                                    if (exists)
                                    {
                                        var cameras = cameraList.Where(m => m.OpenALPRname == videoStream.Camera_name);
                                        foreach (var camera in cameras)
                                        {
                                            camera.OpenALPRId = videoStream.Camera_id.ToString();
                                        }

                                        CameraMapper.SaveCameraList(cameraList);
                                    }
                                }

                                // 2nd case (No Camera_id No Camera_Name found): AXIS M1054 Network Camera (192.168.0.36) - Camera 1
                                if (!exists)
                                {
                                    var milestoneCamera = MilestoneServer.GetCameraItem(match.Captures[0].Value); 
                                    if (milestoneCamera != null)
                                    {
                                        exists = cameraList.Any(m => m.MilestoneName == milestoneCamera.Name);
                                        if (exists)
                                        {
                                            var cameras = cameraList.Where(m => m.MilestoneName == milestoneCamera.Name);
                                            foreach (var camera in cameras)
                                            {
                                                camera.OpenALPRId = videoStream.Camera_id.ToString();
                                                camera.OpenALPRname = videoStream.Camera_name;
                                            }
                                            CameraMapper.SaveCameraList(cameraList);
                                        }
                                        else
                                        {
                                            var camera = new OpenALPRmilestoneCameraName
                                            {
                                                 MilestoneName = milestoneCamera.Name,
                                                OpenALPRId = videoStream.Camera_id.ToString(),
                                                OpenALPRname = videoStream.Camera_name
                                            };
                                            cameraList.Add(camera);
                                            CameraMapper.SaveCameraList(cameraList);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static DateTime Epoch2LocalDateTime(long epoch)
        {
            try
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(epoch).ToLocalTime();
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
            }

            return DateTime.Now;
        }

        private bool AddNewBookmark(PlateInfo plateInfo)
        {
            FQID fqid = null;
            Bookmark bookmark = null;
            var bookmarks = new List<Bookmark>();

            try
            {
                var cameras = cameraList.Where(c => c.OpenALPRId == plateInfo.CameraId.ToString()).ToList();
                if (cameras.Count == 0)
                    Program.Logger.Log.Warn($"{plateInfo.CameraId.ToString()} not found in the local camera list");

                foreach (var camera in cameras)
                {
                    fqid = MilestoneServer.GetCameraByName(camera.MilestoneName);

                    if (fqid == null)
                    {
                        Program.Logger.Log.Info($"No mapping found for camera: {plateInfo.CameraId.ToString()}");
                        continue; // As Matt suggest, this will remove this job from the queue
                    }

                    bookmark = BookmarkService.Instance.BookmarkCreate(
                                        fqid,
                                        plateInfo.EpochStart.AddSeconds(-EpochStartSecondsBefore),  //subtracted 3 secondes from the start time to give more chances to capture the video
                                        plateInfo.EpochStart,                                       //timeTrigged
                                        plateInfo.EpochEnd.AddSeconds(EpochEndSecondsAfter),        //added 3 secondes to give more chances to capture the video
                                        "openalpr",                                                 //so we can reterive openalpr bookmarks only in the plug-in
                                        plateInfo.BestPlateNumber,
                                        $"Make={plateInfo.Make};MakeModel={plateInfo.MakeModel};BodyType={plateInfo.BodyType};Color={plateInfo.Color};BestRegion={plateInfo.BestRegion};Candidates={plateInfo.CandidatesPlate}");

                    if (bookmark == null)
                        Program.Logger.Log.Warn($"Failed to create a Bookmark for Plate number: {plateInfo.BestPlateNumber}");
                    else
                    {
                        bookmarks.Add(bookmark);
                        Program.Logger.Log.Info($"Created Bookmark for Plate number: {plateInfo.BestPlateNumber}");
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
            }

            // sending Alert
            FQID BookmarkFQID = null;
            if (bookmarks.Count != 0)
                BookmarkFQID = bookmarks[0].BookmarkFQID;

            var temp = AlertListHelper.GetLastWriteTime();
            if (temp != lastUpdateTime)
            {
                AlertListHelper.LoadAlertList(dicBlack);
                lastUpdateTime = temp;
                Program.Logger.Log.Info("Reload Alert list");
            }

            var plateFromAlertList = plateInfo.BestPlateNumber;

            var existsInAlertList = dicBlack.ContainsKey(plateInfo.BestPlateNumber);
            if (!existsInAlertList)
            {
                Program.Logger.Log.Info($"{plateFromAlertList} not listed in the alert list.");
                Program.Logger.Log.Info($"looking if any candidates listed in the alert list");

                existsInAlertList = plateInfo.CandidatesPlate.Split(',').Any(p => dicBlack.ContainsKey(p));
                if (existsInAlertList)
                {
                    plateFromAlertList = plateInfo.CandidatesPlate.Split(',').FirstOrDefault(p => dicBlack.ContainsKey(p));
                    Program.Logger.Log.Info($"Candidate {plateFromAlertList} listed in the alert list");
                }
                else
                    Program.Logger.Log.Info($"No any candidates plate number listed in the alert list");
            }
            else
                Program.Logger.Log.Info($"{plateFromAlertList} found in the alert list");

            if (existsInAlertList)
            {
                var descFromAlertList = dicBlack[plateFromAlertList];

                try
                {
                    SendAlarm(plateInfo, fqid, BookmarkFQID, plateFromAlertList, descFromAlertList);
                }
                catch (Exception ex)
                {
                    Program.Logger.Log.Error(null, ex);
                }
            }

            return true;
        }

        private void SendAlarm(PlateInfo plateInfo, FQID fqid, FQID bookmarkFQID, string plateFromAlertList, string descFromAlertList)
        {
            Program.Logger.Log.Info($"Sending an alert for {plateInfo.BestPlateNumber}");

            var cameraName = MilestoneServer.GetCameraName(fqid.ObjectId);

            var eventSource = new EventSource()
            {
                FQID = fqid,
                Name = cameraName,
                //Description = "",
                //ExtensionData = 
            };

            var eventHeader = new EventHeader()
            {
                //The unique ID of the event.
                ID = Guid.NewGuid(),

                //The class of the event, e.g. "Analytics", "Generic", "User-defined".
                Class = "Analytics",

                //The type - a sub-classification - of the event, if applicable.
                Type = null,

                //The time of the event.
                Timestamp = plateInfo.EpochStart,

                //The event message. This is the field that will be matched with the AlarmDefinition message when sending this event to the Event Server. 
                Message = "OpenALPR Alarm",

                //The event name.
                Name = plateInfo.BestPlateNumber,

                //The source of the event. This can represent e.g. a camera, a microphone, a user-defined event, etc.
                Source = eventSource,

                //The priority of the event.
                Priority = 2,

                //The priority name of the event.
                PriorityName = "Medium",

                //optional
                //The message id of the event. The message id coorsponds to the ID part returned in ItemManager.GetKnownEventTypes.
                MessageId = Guid.Empty,

                //A custom tag set by the user to filter the events.
                CustomTag = plateFromAlertList,// the value we got from the config file

                //The expire time of the event. The event will be deleted from the event database when the time is reached. When creating events a value of DateTime.MinValue (default value) indicates that the default event expire time should be used. 
                ExpireTimestamp = DateTime.Now.AddDays(EventExpireAfterDays),

                //ExtensionData = System.Runtime .Serialization.
                //The version of this document schema.
                Version = null
            };

            var alarm = new Alarm()
            {
                //The EventHeader, containing information common for all Milestone events.
                EventHeader = eventHeader,

                //The current state name. 
                StateName = "In progress",

                //The current state of the alarm. 0: Any 1: New 4: In progress 9: On hold 11: Closed. 
                State = 4,

                //The user to which the alarm is currently assigned. Can be seen in the Smart Client dropdown
                AssignedTo = null,//Environment.UserName,

                // Other fields could be filled out, e.g. objectList

                //The current category of the alarm. This should be created first on the Client Management under Alarms then Alaem Data Settings
                //Category = 0,

                //The current category name.
                //CategoryName = null,//"Critical",

                //The count value, if the alarm is a counting alarm. Default: 0 (no count).
                Count = 0,

                //The description of the alarm.
                Description = descFromAlertList,

                //The end time of the alarm, if it takes plate over a period of time.
                EndTime = plateInfo.EpochStart.AddSeconds(EpochEndSecondsAfter),

                //  ExtensionData = 

                //The location of the alarm (this will typically be the same as the camera's location). 
                //Location = null,

                //The ObjectList, containing information about the detected object(s) in the scene. //new AnalyticsObjectList()
                //ObjectList = null,

                // The ReferenceList, containing any number of references to other entities in the system, e.g. alarms or cameras, by FQID. 
                ReferenceList = new ReferenceList { new Reference { FQID = bookmarkFQID } },  // save bookmark id

                //The RuleList, containing information contains information about the rule(s), which triggered the alarm. 
                //RuleList = null,//new RuleList(),

                //The SnapshotList, containing any number of images related to the alarm. If the Source is a camera, it is not neccesary to attach a snapshot from that camera at the time of the alarm. 
                //SnapshotList = null,// new SnapshotList(),

                //The start time of the alarm, if it takes plate over a period of time.
                StartTime = plateInfo.EpochStart.AddSeconds(-EpochStartSecondsBefore),

                //The Vendor, containing information about the analytics vendor including any custom data.
                Vendor = new Vendor { CustomData = plateInfo.ToString() } // save json data
            };

            // Send the Alarm directly to the EventServer, to store in the Alarm database. No rule is being activated.
            try
            {
                var result = EnvironmentManager.Instance.SendMessage(new Message(MessageId.Server.NewAlarmCommand) { Data = alarm });
                if(result.Count != 0)
                {

                }
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
            }
        }

        public void Close()
        {
            if (worker != null)
                worker.Dispose();
        }
    }
}