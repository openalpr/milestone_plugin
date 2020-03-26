// Copyright OpenALPR Technology, Inc. 2018

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace OpenALPRQueueConsumer.Utility
{
    internal static class AlertListHelper
    {
        public static DateTime GetLastWriteTime()
        {
            try
            {
                string path = GetFilePath();

                DateTime lastWriteTime = File.GetLastWriteTime(path);
                DateTime creationTime = File.GetCreationTime(path);

                if (lastWriteTime > creationTime)
                    return lastWriteTime;

                return creationTime;

            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
                Thread.Sleep(5000);
            }

            return DateTime.MinValue;
        }

        public static void LoadAlertList(IDictionary<string, string> dicBlack)
        {
            string path = GetFilePath();

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    LoadAlertList(dicBlack, path);
                    break;
                }
                catch (Exception ex)
                {
                    Program.Log.Error(null, ex);
                    Thread.Sleep(5000);
                }
            }
        }

        private static void LoadAlertList(IDictionary<string, string> dicBlack, string blackFilePath)
        {
            if (dicBlack == null)
                throw new ArgumentNullException(nameof(dicBlack), "Argument Null Exception");

            if (string.IsNullOrEmpty(blackFilePath))
                throw new ArgumentNullException(nameof(blackFilePath), "Argument Null Exception");

            if (File.Exists(blackFilePath))
            {
                dicBlack.Clear();

                string[] lines = File.ReadAllLines(blackFilePath);
                if (lines != null && lines.Length != 0)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        if (line != null)
                            line = line.Trim();

                        if (!string.IsNullOrEmpty(line) && !line.StartsWith("#") && !line.StartsWith("Plate Number"))
                        {
                            string key = string.Empty;

                            string[] values = line.Split(new char[] { ',' });
                            if (values.Length != 0)
                                key = values[0].Trim();

                            if(key.Length != 0)
                            {
                                if (key.Contains(" "))
                                    key = key.Replace(" ", string.Empty);

                                if (key.Contains("-"))
                                    key = key.Replace("-", string.Empty);

                                key = key.ToUpper();
                            }

                            if (key.Length != 0 && !dicBlack.ContainsKey(key))
                            {
                                string value = string.Empty;

                                if (values.Length > 1)
                                    value = values[1].Trim();

                                dicBlack.Add(key, value);
                            }
                        }
                    }
                }
            }
            else
            {
                Program.Log.Warn($"Path does not exists: {blackFilePath}");
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

            return Path.Combine(mappingPath, "AlertList.txt");
        }
    }
}
