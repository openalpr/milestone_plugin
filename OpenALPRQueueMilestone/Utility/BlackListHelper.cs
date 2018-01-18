using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace OpenALPRQueueConsumer.Utility
{
    internal static class BlackListHelper
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

        public static void TryLoadBlackList(IDictionary<string, string> dicBlack)
        {
            var path = GetFilePath();

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    LoadBlackList(dicBlack, path);
                    break;
                }
                catch (Exception ex)
                {
                    Program.Logger.Log.Error(null, ex);
                    Thread.Sleep(5000);
                }
            }
        }

        private static void LoadBlackList(IDictionary<string, string> dicBlack, string blackFilePath)
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
                        if (!string.IsNullOrEmpty(line))
                        {
                            var key = string.Empty;

                            var values = line.Split(new char[] { ',' });
                            if (values.Length != 0)
                                key = values[0];

                            if (!dicBlack.ContainsKey(key))
                            {
                                var value = string.Empty;

                                if (values.Length > 1)
                                    value = values[1];

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

            return Path.Combine(mappingPath, "BlackList.txt");
        }
    }
}
