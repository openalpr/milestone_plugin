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
                var path = GetFilePath();
                if (File.Exists(path))
                    return File.GetLastWriteTime(path);
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
                Thread.Sleep(5000);
            }

            return DateTime.MinValue;
        }

        public static void LoadAlertList(IDictionary<string, string> dicBlack)
        {
            var path = GetFilePath();

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    LoadAlertList(dicBlack, path);
                    break;
                }
                catch (Exception ex)
                {
                    Program.Logger.Log.Error(null, ex);
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

                var lines = File.ReadAllLines(blackFilePath);
                if (lines != null && lines.Length != 0)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        if (line != null)
                            line = line.Trim();

                        if (!string.IsNullOrEmpty(line) && !line.StartsWith("#") && !line.StartsWith("Plate Number"))
                        {
                            var key = string.Empty;

                            var values = line.Split(new char[] { ',' });
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
                                var value = string.Empty;

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
                Program.Logger.Log.Warn($"Path does not exists: {blackFilePath}");
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

            return Path.Combine(mappingPath, "AlertList.txt");
        }
    }
}
