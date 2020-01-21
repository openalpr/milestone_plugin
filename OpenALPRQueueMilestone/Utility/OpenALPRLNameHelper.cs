// Copyright OpenALPR Technology, Inc. 2018

using System;
using System.Collections.Generic;
using System.IO;
using OpenALPRPlugin.Utility;

namespace OpenALPRQueueConsumer.Utility
{
    internal static class OpenALPRLNameHelper
    {
        public static void LoadCameraNameList(List<KeyValuePair<string, string>> list)
        {
            var lines = GetCameraMapping();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!string.IsNullOrEmpty(line))
                {
                    var entries = line.Split(new char[] { '|' });
                    var cameraId = string.Empty;
                    var cameraName = string.Empty;

                    if (entries.Length != 0)
                        cameraId = entries[0].Trim();

                    if (entries.Length > 1)
                        cameraName = entries[1].Trim();

                    if (!string.IsNullOrEmpty(cameraName))
                        list.Add(new KeyValuePair<string, string>(cameraId, cameraName));
                }
            }
        }

        public static void SaveCameraNameList(List<KeyValuePair<string, string>> list)
        {
            try
            {
                var filePath = GetFilePath();
                if (!string.IsNullOrEmpty(filePath))
                {
                    using (var outputFile = new StreamWriter(filePath, false))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            outputFile.WriteLine($"{list[i].Key}|{list[i].Value}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }
        }

        internal static string[] GetCameraMapping()
        {
            var filePath = GetFilePath();
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                string[] cameras = File.ReadAllLines(filePath);
                //Logger.Log.Info($"GetCameraMapping 4: {string.Join(",", cameras)}");
                return cameras;
            }

            return new string[0];
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

            return Path.Combine(mappingPath, "OpenALPRCameraName.txt");
        }
    }
}
