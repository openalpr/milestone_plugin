// Copyright OpenALPR Technology, Inc. 2018

using Database;
using OpenALPRQueueConsumer.Milestone;
using OpenALPRQueueConsumer.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;

namespace OpenALPRQueueConsumer.BeanstalkWorker
{
    internal class Worker
    {
        public static bool AddBookmarks = true;
        public static bool AutoMapping = true;
        public static int EventExpireAfterDays = 7;
        public static int EpochStartSecondsBefore = 3;
        public static int EpochEndSecondsAfter = 3;
        private IDictionary<string, string> dicBlack;
        private DateTime lastAlertUpdateTime;
        private DateTime lastMappingUpdateTime;
        private HttpListener listener;
        private IList<OpenALPRmilestoneCameraName> cameraList;
        private List<KeyValuePair<string, string>> openALPRList;
        private bool listening;

        public Worker()
        {
            cameraList = new List<OpenALPRmilestoneCameraName>();
            CameraMapper.LoadCameraList(cameraList);
            lastMappingUpdateTime = CameraMapper.GetLastWriteTime();

            openALPRList = new List<KeyValuePair<string, string>>();
            OpenALPRLNameHelper.LoadCameraNameList(openALPRList);

            dicBlack = new Dictionary<string, string>();
            AlertListHelper.LoadAlertList(dicBlack);
            lastAlertUpdateTime = AlertListHelper.GetLastWriteTime();
        }


        public void DoWork()
        {
            Settings settings = GetSettings();

            string openALPRServerUrl = settings.OpenALPRServerUrl;
            if (string.IsNullOrEmpty(openALPRServerUrl) || string.IsNullOrWhiteSpace(openALPRServerUrl))
                openALPRServerUrl = "http://localhost:48125/";

            Program.Log.Info($"OpenALPR Server url used: {openALPRServerUrl}");
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            Listener(openALPRServerUrl);

            listening = true;
            HttpListenerContext context = null;

            while (!ServiceStarter.IsClosing)
            {
                try
                {
                    try
                    {
                        context = listener.GetContext();
                    }
                    catch (HttpListenerException)
                    {
                        //if (listener == null)
                        //    Listener(openALPRServerUrl);

                        if (ServiceStarter.IsClosing)
                            break;
                    }

                    HttpListenerRequest request = context.Request;
                    string json;

                    using (Stream receiveStream = request.InputStream)
                    {
                        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            json = readStream.ReadToEnd();
                        }
                    }

                    if (json != null)
                    {
                        Program.Log.Info($"JSON: {json}");
                        Console.WriteLine($"Received request for {request.Url}");
                        ProcessJob(json);
                    }
                    else
                    {
                        Program.Log.Warn("json received was null.");
                    }

                    HttpListenerResponse response = context.Response;
                    string responseString = "OK";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception during processing " + ex.ToString());
                    Program.Log.Error(null, ex);
                }

                Thread.Sleep(1);
            }

            listening = false;
            Program.Log.Info("Stop listening.");
        }

        private void Listener(string openALPRServerUrl)
        {
            listener = new HttpListener();

            try
            {
                listener.Prefixes.Add(openALPRServerUrl);
                listener.Start();
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }
        }

        public bool Test(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(json))
                    return ProcessJob(json);
                else
                    Program.Log.Info("Empty json data.");
            }

            return false;
        }

        private bool ProcessJob(string json)
        {
            bool done = true;
            OpenALPRData palteInfo = JsonHelper.ToClass<OpenALPRData>(json);

            if (palteInfo != null)
            {
                switch (palteInfo.Data_type)
                {
                    case "alpr_group":
                    case "alpr_results":
                        try
                        {
                            if (!palteInfo.Is_parked)
                                done = ProcessAlprGroupOrResults_New(palteInfo);
                            else
                                Program.Log.Info($"{palteInfo.Best_plate_number} is parked, no Bookmark or Alert will be created.");
                        }
                        catch (Exception ex)
                        {
                            done = false;
                            Program.Log.Error(null, ex);
                        }
                        break;

                    case "heartbeat":
                        Heartbeats heartbeats = JsonHelper.ToClass<Heartbeats>(json);
                        try
                        {
                            done = ProcessAlprHeartbeat(heartbeats);
                        }
                        catch (Exception ex)
                        {
                            done = false;
                            Program.Log.Error(null, ex);
                        }
                        break;

                    default:
                        done = true;
                        break;
                }
            }

            //************

            return done;
        }

        private bool ProcessAlprGroupOrResults(OpenALPRData palteInfo)
        {
            if (palteInfo != null)
            {
                List<string> candidates = palteInfo.Candidates.GroupBy(candidate => candidate.Plate)
                   .Select(grp => grp.First())
                   .Select(p => p.Plate)
                   .ToList();

                PlateInfo bestPlateInfo = new PlateInfo
                {
                    DataType = palteInfo.Data_type,
                    CameraId = palteInfo.Camera_id,
                    BestPlateNumber = palteInfo.Best_plate_number,
                    BestRegion = palteInfo.Best_region,
                    CandidatesPlate = string.Join(",", candidates)
                };

                bestPlateInfo.EpochStart = Epoch2LocalDateTime(palteInfo.Epoch_start);
                bestPlateInfo.EpochEnd = Epoch2LocalDateTime(palteInfo.Epoch_end);

                if (palteInfo.Vehicle != null)
                {
                    if (palteInfo.Vehicle.Make != null)
                    {
                        Make make = palteInfo.Vehicle.Make.OrderByDescending(m => m.Confidence).FirstOrDefault();
                        if (make != null)
                            bestPlateInfo.Make = make.Name;
                    }

                    if (palteInfo.Vehicle.Body_type != null)
                    {
                        BodyType body_type = palteInfo.Vehicle.Body_type.OrderByDescending(m => m.Confidence).FirstOrDefault();
                        if (body_type != null)
                            bestPlateInfo.BodyType = body_type.Name;
                    }

                    if (palteInfo.Vehicle.Make_model != null)
                    {
                        MakeModel makeModel = palteInfo.Vehicle.Make_model.OrderByDescending(m => m.Confidence).FirstOrDefault();
                        if (makeModel != null)
                            bestPlateInfo.MakeModel = makeModel.Name;
                    }

                    if (palteInfo.Vehicle.Color != null)
                    {
                        Color color = palteInfo.Vehicle.Color.OrderBy(c => c.Confidence).FirstOrDefault();
                        if (color != null)
                            bestPlateInfo.Color = color.Name;
                    }
                }
                else
                    Program.Log.Warn("Vehicle json data returned null object.");

                if (!string.IsNullOrEmpty(palteInfo.Best_plate_number) && palteInfo.Camera_id != 0)
                {
                    FQID bookmarkFQID = null;
                    IList<OpenALPRmilestoneCameraName> cameras = GetCameraFromMapping(bestPlateInfo.CameraId.ToString());

                    if (AddBookmarks)
                        bookmarkFQID = AddNewBookmark(bestPlateInfo, cameras);

                    if (cameras.Count != 0)
                        SendAlarm(bestPlateInfo, cameras[cameras.Count - 1].MilestoneName, bookmarkFQID); // Send Alert for the last Camera since we recieved the bookmarkFQID for the last camera used in AddNewBookmark.
                }
                else
                    Program.Log.Warn("Best_plate_number is empty or Camera_id == 0");
            }

            return true;
        }

        private bool ProcessAlprGroupOrResults_New(OpenALPRData palteInfo)
        {
            if (palteInfo != null)
            {
                if (!string.IsNullOrEmpty(palteInfo.Best_plate_number) && palteInfo.Camera_id != 0)
                {
                    IList<OpenALPRmilestoneCameraName> cameras = GetCameraFromMapping(palteInfo.Camera_id.ToString());

                    if (AddBookmarks)
                    {
                        FQID bookmarkFQID = null;

                        List<BookmarkItem> bookmarkItems = CreateBookmarkItem(palteInfo, cameras);
                        bookmarkFQID = AddNewBookmark_New(bookmarkItems);

                        if (cameras.Count != 0 && bookmarkFQID != null)
                            SendAlarm_New(palteInfo, cameras[cameras.Count - 1].MilestoneName, bookmarkFQID); // Send Alert for the last Camera since we recieved the bookmarkFQID for the last camera used in AddNewBookmark.
                    }
                }
                else
                    Program.Log.Warn("Best_plate_number is empty or Camera_id == 0");
            }

            return true;
        }


        private IList<OpenALPRmilestoneCameraName> GetCameraFromMapping(string cameraId)
        {
            DateTime temp = CameraMapper.GetLastWriteTime();
            if (temp != lastMappingUpdateTime)
            {
                CameraMapper.LoadCameraList(cameraList);
                lastMappingUpdateTime = temp;
                Program.Log.Info("Reload camera mapping list");
            }

            List<OpenALPRmilestoneCameraName> cameras = cameraList.Where(c => c.OpenALPRId == cameraId).ToList();
            if (cameras.Count == 0)
                Program.Log.Warn($"{cameraId} not found in the local camera list");

            return cameras;
        }

        // Auto mapping happened when there is an ip address in videoStream.Url : rtsp://mhill:cosmos@192.168.0.152/onvif-media / media.amp ? profile = balanced_h264 & sessiontimeout = 60 & streamtype = unicast
        private bool ProcessAlprHeartbeat(Heartbeats heartbeats)
        {
            if (heartbeats != null)
            {
                for (int i = 0; i < heartbeats.Video_streams.Count; i++)
                {
                    VideoStream videoStream = heartbeats.Video_streams[i];
                    if (videoStream != null)
                    {
                        KeyValuePair<string, string> kv = new KeyValuePair<string, string>(videoStream.Camera_id.ToString(), videoStream.Camera_name);
                        if (!openALPRList.Contains(kv))
                        {
                            openALPRList.Add(kv);
                            OpenALPRLNameHelper.SaveCameraNameList(openALPRList);
                        }

                        long cameraId = videoStream.Camera_id;
                        if (!string.IsNullOrEmpty(videoStream.Url) && AutoMapping) ////rtsp://mhill:cosmos@192.168.0.152/onvif-media / media.amp ? profile = balanced_h264 & sessiontimeout = 60 & streamtype = unicast
                        {
                            Match match = Regex.Match(videoStream.Url, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b");
                            if (match.Success)
                            {
                                // 1st case (No Camera_id found): AXIS M1054 Network Camera (192.168.0.36) - Camera 1|TestCamera|
                                bool exists = cameraList.Any(m => m.OpenALPRId == videoStream.Camera_id.ToString());

                                if (!exists)
                                {
                                    exists = cameraList.Any(m => m.OpenALPRname == videoStream.Camera_name);
                                    if (exists)
                                    {
                                        IEnumerable<OpenALPRmilestoneCameraName> cameras = cameraList.Where(m => m.OpenALPRname == videoStream.Camera_name);
                                        foreach (OpenALPRmilestoneCameraName camera in cameras)
                                        {
                                            camera.OpenALPRId = videoStream.Camera_id.ToString();
                                        }

                                        CameraMapper.SaveCameraList(cameraList);
                                    }
                                }

                                // 2nd case (No Camera_id No Camera_Name found): AXIS M1054 Network Camera (192.168.0.36) - Camera 1
                                if (!exists)
                                {
                                    Item milestoneCamera = MilestoneServer.GetCameraItem(match.Captures[0].Value);
                                    if (milestoneCamera != null)
                                    {
                                        exists = cameraList.Any(m => m.MilestoneName == milestoneCamera.Name);
                                        if (exists)
                                        {
                                            IEnumerable<OpenALPRmilestoneCameraName> cameras = cameraList.Where(m => m.MilestoneName == milestoneCamera.Name);
                                            foreach (OpenALPRmilestoneCameraName camera in cameras)
                                            {
                                                camera.OpenALPRId = videoStream.Camera_id.ToString();
                                                camera.OpenALPRname = videoStream.Camera_name;
                                            }
                                            CameraMapper.SaveCameraList(cameraList);
                                        }
                                        else
                                        {
                                            OpenALPRmilestoneCameraName camera = new OpenALPRmilestoneCameraName
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

        private static Settings GetSettings()
        {
            Settings settings = new Settings();

            using (DB db = new DB("OpenALPRQueueMilestone", true))
            {
                settings = db.GetSettings();
            }

            return settings;
        }

        private static DateTime Epoch2LocalDateTime(long epoch)
        {
            try
            {
                Settings settings = GetSettings();

                if(settings.UseUTC)
                    return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(epoch);
                else
                    return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(epoch).ToLocalTime();
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }

            return DateTime.Now;
        }

        private FQID AddNewBookmark(PlateInfo plateInfo, IList<OpenALPRmilestoneCameraName> cameras)
        {
            Bookmark bookmark = null;

            foreach (OpenALPRmilestoneCameraName camera in cameras)
            {
                try
                {
                    FQID fqid = MilestoneServer.GetCameraByName(camera.MilestoneName);

                    if (fqid == null)
                    {
                        Program.Log.Info($"No mapping found for camera: {plateInfo.CameraId.ToString()}");
                        continue; // As Matt suggest, this will remove this job from the queue
                    }

                    bookmark = null;
                    bookmark = BookmarkService.Instance.BookmarkCreate(
                                fqid,
                                plateInfo.EpochStart.AddSeconds(-EpochStartSecondsBefore),  //subtracted 3 secondes from the start time to give more chances to capture the video
                                plateInfo.EpochStart,                                       //timeTrigged
                                plateInfo.EpochEnd.AddSeconds(EpochEndSecondsAfter),        //added 3 secondes to give more chances to capture the video
                                "openalpr",                                                 //so we can reterive openalpr bookmarks only in the plug-in
                                plateInfo.BestPlateNumber,
                                $"Make={plateInfo.Make};MakeModel={plateInfo.MakeModel};BodyType={plateInfo.BodyType};Color={plateInfo.Color};BestRegion={plateInfo.BestRegion};Candidates={plateInfo.CandidatesPlate}");


                    if (bookmark == null)
                        Program.Log.Warn($"Failed to create a Bookmark for Plate number: {plateInfo.BestPlateNumber}");
                    else
                        Program.Log.Info($"Created Bookmark for Plate number: {plateInfo.BestPlateNumber}");
                }
                catch (Exception ex)
                {
                    Program.Log.Error(null, ex);
                }
            }

            return bookmark?.BookmarkFQID; // bookmark for the last camera used.
        }

        private List<BookmarkItem> CreateBookmarkItem(OpenALPRData plateInfo, IList<OpenALPRmilestoneCameraName> cameras)
        {
            List<BookmarkItem> bookmarkItem = new List<BookmarkItem>();

            foreach (OpenALPRmilestoneCameraName camera in cameras)
            {
                try
                {
                    FQID fqid = MilestoneServer.GetCameraByName(camera.MilestoneName);

                    if (fqid == null)
                    {
                        Program.Log.Info($"No mapping found for camera: {plateInfo.Camera_id.ToString()}");
                        continue;
                    }

                    StringBuilder reference = new StringBuilder();
                    StringBuilder header = new StringBuilder();
                    StringBuilder description = new StringBuilder();

                    DateTime timeBegin = Epoch2LocalDateTime(plateInfo.Epoch_start).AddSeconds(-EpochStartSecondsBefore);
                    DateTime timrTrigged = Epoch2LocalDateTime(plateInfo.Epoch_start);
                    DateTime timeEnd = Epoch2LocalDateTime(plateInfo.Epoch_end).AddSeconds(EpochEndSecondsAfter);
                    reference.AppendFormat("openalpr");
                    header.AppendFormat(plateInfo.Best_plate_number);

                    string candidates = string.Join(",", plateInfo.Candidates.GroupBy(candidate => candidate.Plate)
                                            .Select(grp => grp.First())
                                            .Select(p => p.Plate)
                                            .ToList());

                    string coordinates = string.Empty;

                    foreach (Coordinate coordinate in plateInfo.Best_plate.Coordinates)
                    {
                        coordinates += $"(X={coordinate.X},Y={coordinate.Y}),";
                    }

                    coordinates = coordinates.Substring(0, coordinates.Length - 1);

                    string desc = string.Format(@"Make={0};BodyType={1};Color={2};BestRegion={3};Candidates={4};TravelDirection={5};PlateNumber={6};Coordinates=({7});Timestamp={8}",
                        plateInfo.Vehicle.Make.OrderByDescending(m => m.Confidence).FirstOrDefault().Name,
                        plateInfo.Vehicle.Body_type.OrderByDescending(m => m.Confidence).FirstOrDefault().Name,
                        plateInfo.Vehicle.Color.OrderBy(c => c.Confidence).FirstOrDefault().Name,
                        plateInfo.Best_region,
                        candidates,
                        plateInfo.Travel_direction,
                        plateInfo.Best_plate_number,
                        coordinates,
                        timrTrigged);

                    DateTime endTime = timeBegin.Date.AddDays(2).AddSeconds(-1);
                    DateTime startTime = timeBegin.Date;

                    List<Bookmark> bookmarks = BookmarkService.Instance.BookmarkSearchTime(
                                                    fqid.ServerId,
                                                    timeBegin.Date,
                                                    (endTime.Ticks - startTime.Ticks) / 10,
                                                    999,
                                                    new Guid[] { Kind.Camera, Kind.Microphone, Kind.Speaker },
                                                    new FQID[] { fqid },
                                                    null,
                                                    "openalpr"
                                                ).ToList();

                    Bookmark bookmark = bookmarks.FirstOrDefault(b => b.Description == desc);

                    if (bookmark == null)
                    {
                        description.AppendFormat(desc);

                        bookmarkItem.Add(new BookmarkItem()
                        {
                            FQID = fqid,
                            TimeBegin = timeBegin,
                            TimrTrigged = timrTrigged,
                            TimeEnd = timeEnd,
                            Reference = reference,
                            Header = header,
                            Description = description,
                            PlateInfo = plateInfo
                        });
                    }
                }
                catch (Exception ex)
                {
                    Program.Log.Error(null, ex);
                }
            }

            return bookmarkItem;
        }

        private FQID AddNewBookmark_New(List<BookmarkItem> bookmarkItem)
        {
            Bookmark bookmark = null;
            VideoOS.Platform.SDK.Environment.Initialize();

            foreach(BookmarkItem item in bookmarkItem)
            {
                bookmark = null;

                try
                {
                    bookmark = BookmarkService.Instance.BookmarkCreate(
                                        item.FQID,
                                        item.TimeBegin,
                                        item.TimrTrigged,
                                        item.TimeEnd,
                                        item.Reference.ToString(),
                                        item.Header.ToString(),
                                        item.Description.ToString());
                    Program.Log.Info($"Created Bookmark for Plate number: {item.PlateInfo.Best_plate_number}");
                }
                catch (Exception ex)
                {
                    Program.Log.Warn($"Failed to create a Bookmark for Plate number: {item.PlateInfo.Best_plate_number}{Environment.NewLine}");
                    Program.Log.Warn($"Bookmark Failed Message: {ex.Message}");
                }
            }

            return bookmark?.BookmarkFQID;
        }

        private void SendAlarm(PlateInfo plateInfo, string milestoneCameraName, FQID bookmarkFQID)//, string plateFromAlertList, string descFromAlertList)
        {
            FQID fqid = MilestoneServer.GetCameraByName(milestoneCameraName);

            DateTime temp = AlertListHelper.GetLastWriteTime();
            if (temp != lastAlertUpdateTime)
            {
                AlertListHelper.LoadAlertList(dicBlack);
                lastAlertUpdateTime = temp;
                Program.Log.Info("Reload Alert list");
            }

            string plateFromAlertList = plateInfo.BestPlateNumber;

            bool existsInAlertList = dicBlack.ContainsKey(plateInfo.BestPlateNumber);
            if (!existsInAlertList)
            {
                Program.Log.Info($"{plateFromAlertList} not listed in the alert list.");
                Program.Log.Info($"looking if any candidates listed in the alert list");

                existsInAlertList = plateInfo.CandidatesPlate.Split(',').Any(p => dicBlack.ContainsKey(p));
                if (existsInAlertList)
                {
                    plateFromAlertList = plateInfo.CandidatesPlate.Split(',').FirstOrDefault(p => dicBlack.ContainsKey(p));
                    Program.Log.Info($"Candidate {plateFromAlertList} listed in the alert list");
                }
                else
                    Program.Log.Info($"No any candidates plate number listed in the alert list");
            }
            else
                Program.Log.Info($"{plateFromAlertList} found in the alert list");

            if (existsInAlertList)
            {
                string descFromAlertList = dicBlack[plateFromAlertList];

                Program.Log.Info($"Sending an alert for {plateInfo.BestPlateNumber}");

                string cameraName = MilestoneServer.GetCameraName(fqid.ObjectId);

                EventSource eventSource = new EventSource()
                {
                    FQID = fqid,
                    Name = cameraName
                };

                EventHeader eventHeader = new EventHeader()
                {
                    ID = Guid.NewGuid(),
                    Class = "Analytics",
                    Type = null,
                    Timestamp = plateInfo.EpochStart,
                    Message = "OpenALPR Alarm",
                    Name = plateInfo.BestPlateNumber,
                    Source = eventSource,
                    Priority = 2,
                    PriorityName = "Medium",
                    MessageId = Guid.Empty,
                    CustomTag = plateFromAlertList,// the value we got from the config file
                    ExpireTimestamp = DateTime.Now.AddDays(EventExpireAfterDays),
                    Version = null
                };

          
                AnalyticsEvent analyticsEvent = new AnalyticsEvent();
                analyticsEvent.EventHeader = eventHeader;
                analyticsEvent.Location = cameraName;
                analyticsEvent.Description = "OpenALPR Alarm Event";
                analyticsEvent.Vendor = new Vendor { CustomData = plateInfo.ToString() };
                //analyticsEvent.Vendor.Name = "OpenAlpr";
        

                EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.Server.NewEventCommand)
                { Data = analyticsEvent });

                Alarm alarm = new Alarm()
                {
                    EventHeader = eventHeader,
                    StateName = "In progress",
                    State = 4,
                    AssignedTo = null,
                    Count = 0,
                    Description = descFromAlertList,
                    EndTime = plateInfo.EpochStart.AddSeconds(EpochEndSecondsAfter),
                    ReferenceList = new ReferenceList { new Reference { FQID = bookmarkFQID } },
                    StartTime = plateInfo.EpochStart.AddSeconds(-EpochStartSecondsBefore),
                    Vendor = new Vendor { CustomData = plateInfo.ToString() }
                };

                try
                {
                    using (Impersonation impersonation = new Impersonation(BuiltinUser.NetworkService))
                        EnvironmentManager.Instance.SendMessage(new Message(MessageId.Server.NewAlarmCommand) { Data = alarm });
                }
                catch (Exception ex)
                {
                    Program.Log.Error(null, ex);
                }
            }
        }

        private void SendAlarm_New(OpenALPRData plateInfo, string milestoneCameraName, FQID bookmarkFQID)//, string plateFromAlertList, string descFromAlertList)
        {
            FQID fqid = MilestoneServer.GetCameraByName(milestoneCameraName);

            DateTime temp = AlertListHelper.GetLastWriteTime();
            if (temp != lastAlertUpdateTime)
            {
                AlertListHelper.LoadAlertList(dicBlack);
                lastAlertUpdateTime = temp;
                Program.Log.Info("Reload Alert list");
            }

            string plateFromAlertList = plateInfo.Best_plate_number;

            bool existsInAlertList = dicBlack.ContainsKey(plateInfo.Best_plate_number);
            if (!existsInAlertList)
            {
                Program.Log.Info($"{plateFromAlertList} not listed in the alert list.");
                Program.Log.Info($"looking if any candidates listed in the alert list");

                if (plateInfo.Candidates.Count > 0)
                {
                    plateFromAlertList = plateInfo.Candidates.FirstOrDefault().Plate;
                    Program.Log.Info($"Candidate {plateFromAlertList} listed in the alert list");
                }
                else
                    Program.Log.Info($"No any candidates plate number listed in the alert list");
            }
            else
                Program.Log.Info($"{plateFromAlertList} found in the alert list");

            if (existsInAlertList)
            {
                string descFromAlertList = dicBlack[plateFromAlertList];

                Program.Log.Info($"Sending an alert for {plateInfo.Best_plate_number}");

                if (fqid == null)
                    fqid = MilestoneServer.GetCameraByName(milestoneCameraName);

                string cameraName = MilestoneServer.GetCameraName(fqid.ObjectId);

                EventSource eventSource = new EventSource()
                {
                    FQID = fqid,
                    Name = cameraName,
                    //Description = "",
                    //ExtensionData = 
                };

                EventHeader eventHeader = new EventHeader()
                {
                    //The unique ID of the event.
                    ID = Guid.NewGuid(),

                    //The class of the event, e.g. "Analytics", "Generic", "User-defined".
                    Class = "Analytics",

                    //The type - a sub-classification - of the event, if applicable.
                    Type = null,

                    //The time of the event.
                    Timestamp = Epoch2LocalDateTime(plateInfo.Epoch_start).AddSeconds(-EpochStartSecondsBefore),

                    //The event message. This is the field that will be matched with the AlarmDefinition message when sending this event to the Event Server. 
                    Message = "OpenALPR Alarm",

                    //The event name.
                    Name = plateInfo.Best_plate_number,

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

                Alarm alarm = new Alarm()
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
                    EndTime = Epoch2LocalDateTime(plateInfo.Epoch_start).AddSeconds(-EpochStartSecondsBefore),

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
                    StartTime = Epoch2LocalDateTime(plateInfo.Epoch_start).AddSeconds(-EpochStartSecondsBefore),

                    //The Vendor, containing information about the analytics vendor including any custom data.
                    Vendor = new Vendor { CustomData = plateInfo.ToString() } // save json data
                };

                // Send the Alarm directly to the EventServer, to store in the Alarm database. No rule is being activated.
                try
                {
                    using (Impersonation impersonation = new Impersonation(BuiltinUser.NetworkService))
                        EnvironmentManager.Instance.SendMessage(new Message(MessageId.Server.NewAlarmCommand) { Data = alarm });
                }
                catch (Exception ex)
                {
                    Program.Log.Error(null, ex);
                }
            }
        }

        public void Close()
        {
            try
            {
                if (listener != null)
                {
                    if (listener.IsListening)
                    {
                        listener.Stop();
                        Console.WriteLine("Listener stopped");
                    }
                }
            }
            catch //(Exception ex)
            {

            }

            int i = 0;
            while (listening && i++ < 20)
                Thread.Sleep(200);

            try
            {
                if (listener != null)
                    listener.Close();
            }
            catch //(Exception ex)
            {

            }
        }
    }
}