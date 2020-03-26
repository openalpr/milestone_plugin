// Copyright OpenALPR Technology, Inc. 2018

using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using OpenALPRPlugin;
using System;
using System.IO;

namespace OpenALPRQueueConsumer.Utility
{
    internal static class Logger
    {
        internal static ILog Log;
        internal static string LogPath;

        internal static void Initialize(string logName, string subFolder = "")
        {
            LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OpenALPRQueueMilestoneDefinition.PlugName, "Log");
            if (!string.IsNullOrEmpty(subFolder) && subFolder.Length != 0)
                LogPath = $"{LogPath}\\{subFolder}";

            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
                Helper.SetDirectoryNetworkServiceAccessControl(LogPath);
            }

            string fullName = Path.Combine(LogPath, $"{logName}.log");

            log4net.Config.XmlConfigurator.Configure();
            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            hierarchy.Threshold = Level.Debug;

            log4net.Repository.Hierarchy.Logger logger = hierarchy.LoggerFactory.CreateLogger(hierarchy, logName);
            logger.Hierarchy = hierarchy;
            logger.AddAppender(CreateFileAppender(logName, fullName));
            logger.Repository.Configured = true;
            logger.Level = Level.Debug;
            Log = new LogImpl(logger);
            Log.Info("**********************************************");
            Log.Info($"{logName} logging started");
        }

        private static IAppender CreateFileAppender(string name, string fileName)
        {
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date{yyyy-MMM-dd HH:mm:ss,fff} [%t][%M] %-5level - %message  %newline";
            patternLayout.ActivateOptions();

            RollingFileAppender appender = new RollingFileAppender()
            {
                Name = name,
                File = fileName,
                AppendToFile = true,
                MaxSizeRollBackups = -1,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                MaximumFileSize = "50MB",
                Layout = patternLayout,
                LockingModel = new FileAppender.MinimalLock(),
                StaticLogFileName = true,
                DatePattern = "yyyyMMdd",
                PreserveLogFileNameExtension = true,
                MaxFileSize = 10 * 1024 * 1024,
                CountDirection = 1000,
                Threshold = Level.All
                //ImmediateFlush = true,// TODO , comment this line to enhance the performance.
            };

            appender.ActivateOptions();
            return appender;
        }

        internal static void Close()
        {
            Log.Logger.Repository.Shutdown();
        }
    }
}
