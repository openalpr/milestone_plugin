// Copyright OpenALPR Technology, Inc. 2018

using log4net;
using OpenALPRQueueConsumer.Utility;
using System;
using System.IO;
using System.ServiceProcess;

namespace OpenALPRQueueConsumer
{
    public static class Program
    {
        internal static string ProductName = "OpenALPRQueueMilestone";
        internal static string CompanyName = "OpenALPR";
        internal static ILog Log { get; private set; }
        private static Logging Logger;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            OpenALPRQueueMilestone servicesToRun = null;

            try
            {
                string VideoOS = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                
                string dir = Path.Combine(VideoOS, @"VideoOS\MIPPlugins\OpenALPR");
                if (Directory.Exists(dir))
                    Directory.SetCurrentDirectory(dir);
                Console.WriteLine($"Current Directory: {Environment.CurrentDirectory}");

                Logger = new Logging("Service");
                if (Logger != null)
                {
                    Log = Logger.Log;

                    Console.WriteLine($"Log: {Logger.LogPath}");
                }

                servicesToRun = new OpenALPRQueueMilestone();

                if (Environment.UserInteractive)// Run Service as a Console Application.(for Debugging)
                {
                    Console.WriteLine("Press any key to stop program");
                    servicesToRun.StartConsole(args);
                    Console.Read();
                    servicesToRun.StopConsole();
                }
                else                            // Run as a Service.
                    ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (Logger != null)
                    Logger.Log.Error(null, ex);

                if (servicesToRun != null)
                    servicesToRun.Stop();
            }
        }
    }
}
