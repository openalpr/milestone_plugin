// Copyright OpenALPR Technology, Inc. 2018

using OpenALPRQueueConsumer.BeanstalkWorker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace OpenALPRQueueConsumer.Utility
{
    internal static class CameraMapper
    {
        public static DateTime GetLastWriteTime()
        {
            try
            {
                var path = GetFilePath();
                if (File.Exists(path))
                {
                    var lastWriteTime = File.GetLastWriteTime(path);
                    var creationTime = File.GetCreationTime(path);

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
            var lines = GetCameraMapping();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!string.IsNullOrEmpty(line))
                {
                    var entry = line.Split(new char[] { '|' });
                    if (entry.Length != 0)
                    {
                        var camera = new OpenALPRmilestoneCameraName { MilestoneName = entry[0] };
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
            var filePath = GetFilePath();
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                return File.ReadAllLines(filePath);

            return new string[0];
        }

        internal static void SaveCameraList(IList<OpenALPRmilestoneCameraName> cameraList)
        {
            try
            {
                var filePath = GetFilePath();
                if (!string.IsNullOrEmpty(filePath))
                {
                    using (var outputFile = new StreamWriter(filePath, false))
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

            var mappingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), PlugName, "Mapping");

            if (!Directory.Exists(mappingPath))
            {
                Directory.CreateDirectory(mappingPath);
                Helper.SetDirectoryNetworkServiceAccessControl(mappingPath);
            }

            return Path.Combine(mappingPath, "CameraMapping.txt");
        }
    }
}
