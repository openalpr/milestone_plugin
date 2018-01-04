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

namespace OpenALPRQueueConsumer.BeanstalkWorker
{
    internal class Worker
    {
        //http://www.jsonutils.com/
        private IDisposable worker = null;
        private IDictionary<string, OpenALPRmilestoneCameraName> cameraDictionary;

        public Worker()
        {
            cameraDictionary = new Dictionary<string, OpenALPRmilestoneCameraName>();
        }

        public async Task DoWork()
        {
            var options = new WorkerOptions
            {
                Tubes = new List<string>(1) { "alprd" },
                //TaskScheduler = TaskScheduler.Current,
            };

            worker = await BeanstalkConnection.ConnectWorkerAsync("localhost:11300", options, async (iworker, job) =>
            {
                bool done = true;

                if (job != null)
                {
                    var json = Encoding.UTF8.GetString(job.Data, 0, job.Data.Length);
                    done = ProcessJob(json);
                }

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
                            done = ProcessAlprGroupOrResults(palteInfo);
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

                if (!string.IsNullOrEmpty(palteInfo.Best_plate_number) && palteInfo.Camera_id != 0)
                    return AddNewBookmark(bestPlateInfo);
            }

            return true;
        }

        private bool ProcessAlprHeartbeat(Heartbeats heartbeats)
        {
            if (heartbeats != null)
            {
                for (int i = 0; i < heartbeats.Video_streams.Count; i++)
                {
                    var videoStream = heartbeats.Video_streams[i];
                    if (videoStream != null)
                    {
                        var cameraId = videoStream.Camera_id;
                        if (!string.IsNullOrEmpty(videoStream.Url)) ////rtsp://mhill:cosmos@192.168.0.152/onvif-media / media.amp ? profile = balanced_h264 & sessiontimeout = 60 & streamtype = unicast
                        {
                            var match = Regex.Match(videoStream.Url, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b");
                            if (match.Success)
                            {
                                if (!cameraDictionary.ContainsKey(videoStream.Camera_id.ToString()))
                                {
                                    var milestoneCamera = MilestoneServer.GetCameraItem(match.Captures[0].Value);
                                    if (milestoneCamera != null)
                                    {
                                        var openALPRmilestoneCameraName = new OpenALPRmilestoneCameraName { OpenALPRname = videoStream.Camera_name, MilestoneId = milestoneCamera.FQID };
                                        cameraDictionary.Add(videoStream.Camera_id.ToString(), openALPRmilestoneCameraName);
                                        CameraMapper.AddCamera(milestoneCamera.Name, videoStream.Camera_name, videoStream.Camera_id.ToString());
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
            if (plateInfo != null)
            {
                try
                {
                    FQID fqid = null;
                    var camera = cameraDictionary.FirstOrDefault(c => c.Key == plateInfo.CameraId.ToString());

                    fqid = camera.Key == null || camera.Value == null ?
                            GetCameraNameFromMapping(plateInfo.CameraId.ToString()):
                            camera.Value.MilestoneId;

                    if (fqid == null)
                    {
                        Program.Logger.Log.Info($"No mapping found for camera: {plateInfo.CameraId.ToString()}");
                        return true; // As Matt suggest, this will remove this job from the queue
                    }


                    var bookmark = BookmarkService.Instance.BookmarkCreate(
                                        fqid,
                                        plateInfo.EpochStart.AddSeconds(-3),    //subtracted 3 secondes from the start time to give more chances to capture the video
                                        plateInfo.EpochStart,                   //timeTrigged
                                        plateInfo.EpochEnd.AddSeconds(3),       //added 3 secondes to give more chances to capture the video
                                        "openalpr",                             //so we can reterive openalpr bookmarks only in the plug-in
                                        plateInfo.BestPlateNumber,
                                        $"Make={plateInfo.Make};MakeModel:{plateInfo.MakeModel};BodyType={plateInfo.BodyType};Color={plateInfo.Color};BestRegion={plateInfo.BestRegion};Candidates={plateInfo.CandidatesPlate}");

                    if (bookmark == null)
                    {
                        Program.Logger.Log.Info($"Failed to create a Bookmark for Plate number: {plateInfo.BestPlateNumber}");
                        return false;
                    }

                    Program.Logger.Log.Info($"Created Bookmark for Plate number: {plateInfo.BestPlateNumber}");
                }
                catch (Exception ex)
                {
                    Program.Logger.Log.Error(null, ex);
                }
            }

            return true;
        }

        private FQID GetCameraNameFromMapping(string alprCameraId)
        {
            try
            {
                var lines = CameraMapper.GetCameraMapping();
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (!string.IsNullOrEmpty(line))
                    {
                        var entry = line.Split(new char[] { '|' });
                        var currentAlprCameraId = string.Empty;
                        var currentMilestoneCameraName = string.Empty;

                        if (entry.Length != 0)
                            currentMilestoneCameraName = entry[0];

                        if (entry.Length > 2)
                            currentAlprCameraId = entry[2];

                        if (currentAlprCameraId == alprCameraId)
                        {
                            if (currentMilestoneCameraName.Length != 0)
                            {
                                var fqid = MilestoneServer.GetCameraByName(currentMilestoneCameraName);
                                if (fqid != null)
                                    cameraDictionary.Add(alprCameraId, new OpenALPRmilestoneCameraName { MilestoneId = fqid, OpenALPRname = currentMilestoneCameraName });

                                return fqid;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
            }

            return null;
        }

        public void Close()
        {
            if (worker != null)
                worker.Dispose();
        }
    }
}