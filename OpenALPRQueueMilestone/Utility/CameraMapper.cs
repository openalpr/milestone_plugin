using System;
using System.IO;

namespace OpenALPRQueueConsumer.Utility
{
    internal static class CameraMapper
    {
        internal static string[] GetCameraMapping()
        {
            var filePath = CameraMappingFile();
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                return File.ReadAllLines(filePath);

            return new string[0];
        }

        internal static void AddCamera(string milestoneCameraName, string alprCameraName, string alprCameraId)
        {
            try
            {
                var lines = GetCameraMapping();
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (!string.IsNullOrEmpty(line))
                    {
                        var entry = line.Split(new char[] { '|' });
                        var currentAlprCameraName = string.Empty;

                        if (entry.Length > 1)
                            currentAlprCameraName = entry[1];

                        if (currentAlprCameraName == alprCameraName)
                            return; // no need to add it
                    }
                }

                // This mean we need to add it to the file

                var filePath = CameraMappingFile();
                if (!string.IsNullOrEmpty(filePath))
                {
                    using (var outputFile = new StreamWriter(filePath, true))
                        outputFile.WriteLine($"{milestoneCameraName}|{alprCameraName}|{alprCameraId}");
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
            }
        }

        internal static string CameraMappingFile()
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
