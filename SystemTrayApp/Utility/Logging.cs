using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using SystemTrayApp;

namespace OpenALPR.SystemTrayIcon.Utility
{
    internal class Logging
    {
        internal ILog Log { get; private set; }
        internal string LogPath;

        internal Logging(string logName, string subFolder = "")
        {
            //LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Program.CompanyName, Program.ProductName, "Log");
            LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Program.CompanyName, "Log");

            if (!string.IsNullOrEmpty(subFolder) && subFolder.Length != 0)
                LogPath = $"{LogPath}\\{subFolder}";

            string fullName = Path.Combine(LogPath, $"{logName}.log");

            log4net.Config.XmlConfigurator.Configure();
            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            hierarchy.Threshold = Level.Debug;

            Logger logger = hierarchy.LoggerFactory.CreateLogger(hierarchy, logName);
            logger.Hierarchy = hierarchy;
            logger.AddAppender(CreateFileAppender(logName, fullName));
            logger.Repository.Configured = true;
            logger.Level = Level.Debug;
            Log = new LogImpl(logger);
            Log.Info("---------------------------------------------------");
            Log.Info($"{logName} logging started");
        }

        private static IAppender CreateFileAppender(string name, string fileName)
        {
            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "%date{yyyy-MMM-dd HH:mm:ss,fff} [%t][%M] %-5level - %message  %newline"
            };
            patternLayout.ActivateOptions();

            RollingFileAppender appender = new RollingFileAppender()
            {
                Name = name,
                File = fileName,
                AppendToFile = true,
                MaxSizeRollBackups = -1,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                MaximumFileSize = "15MB",
                Layout = patternLayout,
                LockingModel = new FileAppender.MinimalLock(),
                StaticLogFileName = true,
                DatePattern = "yyyyMMdd",
                PreserveLogFileNameExtension = true,
                MaxFileSize = 15 * 1024 * 1024,
                CountDirection = 1000,
                Threshold = Level.All
            };

            appender.ActivateOptions();
            return appender;
        }

        internal void Close()
        {
            Log.Logger.Repository.Shutdown();
        }
    }
}