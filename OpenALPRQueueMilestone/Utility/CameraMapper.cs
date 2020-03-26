// Copyright OpenALPR Technology, Inc. 2018

using OpenALPRQueueConsumer.BeanstalkWorker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenALPRQueueConsumer.Utility;

namespace OpenALPRQueueConsumer.Utility
{
    internal static class CameraMapper
    {
        public static DateTime GetLastWriteTime()
        {
            try
            {
                string path = GetFilePath();
                if (File.Exists(path))
                {
                    DateTime lastWriteTime = File.GetLastWriteTime(path);
                    DateTime creationTime = File.GetCreationTime(path);

                    if (lastWriteTime > creationTime)
                        return lastWriteTime;

                    return creationTime;
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
                Thread.Sleep(5000);
            }

            return DateTime.MinValue;
        }

        //AXIS M1054 Network Camera (192.168.0.33) - Camera 1|TestCamera|237528343
        internal static void LoadCameraList(IList<OpenALPRmilestoneCameraName> cameraList)
        {
            cameraList.Clear();
            string[] lines = GetCameraMapping();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!string.IsNullOrEmpty(line))
                {
                    string[] entry = line.Split(new char[] { '|' });
                    if (entry.Length != 0)
                    {
                        OpenALPRmilestoneCameraName camera = new OpenALPRmilestoneCameraName { MilestoneName = entry[0] };
                        if (entry.Length > 1)
                            camera.OpenALPRname = entry[1];

                        if (entry.Length > 2)
                            camera.OpenALPRId = entry[2];

                        cameraList.Add(camera);
                    }
                }
            }
        }

        internal static string[] GetCameraMapping()
        {
            string filePath = GetFilePath();
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                string[] cameras = File.ReadAllLines(filePath);
                //Logger.Log.Info($"GetCameraMapping 3: {string.Join(",", cameras)}");
                return cameras;
            }

            return new string[0];
        }

        internal static void SaveCameraList(IList<OpenALPRmilestoneCameraName> cameraList)
        {
            try
            {
                string filePath = GetFilePath();
                if (!string.IsNullOrEmpty(filePath))
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
                    {
                        for (int i = 0; i < cameraList.Count; i++)
                        {
                            outputFile.WriteLine($"{cameraList[i].MilestoneName}|{cameraList[i].OpenALPRname}|{cameraList[i].OpenALPRId}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }
        }

        internal static string GetFilePath()
        {
            const string PlugName = "OpenALPR";

            string mappingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), PlugName, "Mapping");

            if (!Directory.Exists(mappingPath))
            {
                Directory.CreateDirectory(mappingPath);
                Helper.SetDirectoryNetworkServiceAccessControl(mappingPath);
            }

            return Path.Combine(mappingPath, "CameraMapping.txt");
        }
    }
}
