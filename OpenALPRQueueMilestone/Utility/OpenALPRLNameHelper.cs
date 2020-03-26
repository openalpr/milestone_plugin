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
            string[] lines = GetCameraMapping();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!string.IsNullOrEmpty(line))
                {
                    string[] entries = line.Split(new char[] { '|' });
                    string cameraId = string.Empty;
                    string cameraName = string.Empty;

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
                string filePath = GetFilePath();
                if (!string.IsNullOrEmpty(filePath))
                {
                    using (StreamWriter outputFile = new StreamWriter(filePath, false))
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
            string filePath = GetFilePath();
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

            string mappingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), PlugName, "Mapping");

            if (!Directory.Exists(mappingPath))
            {
                Directory.CreateDirectory(mappingPath);
                Helper.SetDirectoryNetworkServiceAccessControl(mappingPath);
            }

            return Path.Combine(mappingPath, "OpenALPRCameraName.txt");
        }
    }
}
