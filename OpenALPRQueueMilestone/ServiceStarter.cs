using OpenALPRQueueConsumer.BeanstalkWorker;
using OpenALPRQueueConsumer.Milestone;
using OpenALPRQueueConsumer.Utility;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using SDKEnvironment = VideoOS.Platform.SDK.Environment;

namespace OpenALPRQueueConsumer
{
    internal class ServiceStarter
    {
        private MilestoneServer milestoneServer;
        private Worker worker;
        private Task workerTask;
        internal static bool IsConnectedToMilestoneServer;
        internal static string milestoneHostName = "localhost";
        internal static volatile bool IsClosing;
        private readonly object connectLock = new object();
        private const string MilestoneServerNameString = "MilestoneServerName";
        private const string EventExpireAfterDaysString = "EventExpireAfterDays";
        private const string EpochStartSecondsBeforeString = "EpochStartSecondsBefore";
        private const string EpochEndSecondsAfterString = "EpochEndSecondsAfter";
        private const string AddBookmarksString = "AddBookmarks";
        private const string AutoMappingString = "AutoMapping";

        public ServiceStarter()
        {
        }

        #region Start Service

        public void OnStartService()
        {
            try
            {
                LocalStartService();
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
                OpenALPRQueueMilestone.ServiceInstance.Stop();
                Environment.Exit(1);
            }
        }

        private void LocalStartService()
        {
            Program.Logger.Log.Info("Start the service");

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            var appprincipal = Thread.CurrentPrincipal as WindowsPrincipal;
            Program.Logger.Log.Info($"Is in administrators: {appprincipal.IsInRole("Administrators")}");
            Program.Logger.Log.Info($"Windows identity: {WindowsIdentity.GetCurrent().Name}"); //Windows identity: NT AUTHORITY\SYSTEM  

            LogSystemInfo();
            WriteExecutingAssemblyVersion();

#if !DEBUG
            using (var impersonation = new Impersonation(BuiltinUser.NetworkService))
            {
#endif
                try
                {
                    SDKEnvironment.Initialize();// General initialize. Always required
                    SDKEnvironment.RemoveAllServers();
                    TryConnectingToMilestoneServer();
                }
                catch (Exception ex)
                {
                    Program.Logger.Log.Error(null, ex);
                    Program.Logger.Log.Info($"Windows identity: {WindowsIdentity.GetCurrent().Name}"); //Windows identity: NT AUTHORITY\NETWORK SERVICE
                }
#if !DEBUG
            }
#endif
        }

        private void TryConnectingToMilestoneServer(string serverName = null, string userName = null, string password = null)
        {
            if (MilestoneServer.ConnectedToMilestone)
                return;

            if (string.IsNullOrEmpty(serverName))
                serverName = Helper.ReadConfigKey(MilestoneServerNameString);

            if (string.IsNullOrEmpty(userName))
                userName = Helper.ReadConfigKey("MilestoneUserName");

            if (string.IsNullOrEmpty(password))
                password = Helper.ReadConfigKey("MilestonePassword");

            if (string.IsNullOrEmpty(serverName))
                serverName = "http://localhost:80/";

            if (!serverName.StartsWith("http://"))
                serverName = $"http://{serverName}";// $"http://{serverName}:80/";

            if (milestoneServer == null)
                milestoneServer = new MilestoneServer();

            MilestoneServer.LoginUsingCurrentCredentials(serverName, userName, password);

            if (MilestoneServer.ConnectedToMilestone)
            {
                IsConnectedToMilestoneServer = true;

                var temp = Helper.ReadConfigKey(MilestoneServerNameString);
                if (temp != serverName)
                    Helper.AddUpdateAppSettings(MilestoneServerNameString, serverName);

                temp = Helper.ReadConfigKey(AddBookmarksString);
                bool.TryParse(temp, out Worker.AddBookmarks);

                temp = Helper.ReadConfigKey(AutoMappingString);
                bool.TryParse(temp, out Worker.AutoMapping);

                temp = Helper.ReadConfigKey(EventExpireAfterDaysString);
                int.TryParse(temp, out Worker.EventExpireAfterDays);

                temp = Helper.ReadConfigKey(EpochStartSecondsBeforeString);
                int.TryParse(temp, out Worker.EpochStartSecondsBefore);

                temp = Helper.ReadConfigKey(EpochEndSecondsAfterString);
                int.TryParse(temp, out Worker.EpochEndSecondsAfter);

                if (worker == null)
                {
                    worker = new Worker();
                    workerTask = Task.Run(() => worker.DoWork());

                    //comment the above line to test using json files
                    //worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\heartbeat.json");
                    //while (!IsClosing)
                    //{
                    //    worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\alpr_group1.json");
                    //    worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\alpr_group2.json");
                    //    worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\alpr_group3.json");
                    //    worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\alpr_group4.json");
                    //    worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\alpr_group1.json");
                    //    worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\alpr_group2.json");
                    //    worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\alpr_group3.json");
                    //    worker.Test(@"C:\OpenALPR\OpenALPRMilestone\JsonTestFiles\alpr_group4.json");
                    //    Thread.Sleep(5000);
                    //}
                    //Console.WriteLine("Done...");
                }
            }
        }

        private static void LogSystemInfo()
        {
            Program.Logger.Log.Info($"OS platform: {Environment.OSVersion.Platform.ToString()}");
            Program.Logger.Log.Info($"OS version: {Environment.OSVersion.Version.ToString()}");
            Program.Logger.Log.Info($"Service pack version: {Environment.OSVersion.ServicePack.ToString()}");
            Program.Logger.Log.Info($"Version string: {Environment.OSVersion.VersionString.ToString()}");
            Program.Logger.Log.Info($"Processor count: {Environment.ProcessorCount.ToString(CultureInfo.InvariantCulture)}");
            Program.Logger.Log.Info($"CLR version: {Environment.Version.ToString()}");
        }

        private static void WriteExecutingAssemblyVersion()
        {
            FileVersionInfo FileVersion = null;
            try
            {
#if DEBUG
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                FileVersion = FileVersionInfo.GetVersionInfo(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), $"{name}.exe"));
#else
                FileVersion = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName);
#endif
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
            }

            if (FileVersion != null)
            {
                var version = $"{Program.ProductName} service version: {FileVersion.FileVersion}";
                Program.Logger.Log.Info(version);
            }

            string bit = "The future is now!";
            if (IntPtr.Size == 4)
                bit = "32-bit";
            else if (IntPtr.Size == 8)
                bit = "64-bit";

            Program.Logger.Log.Info(bit);
        }

        #endregion Start Service

        #region OnStop

        public void OnStop()
        {
            IsClosing = true;
            Program.Logger.Log.Debug("On stop");
            Program.Logger.Log.Info($"Stopping {Program.ProductName} service");

            if (worker != null)
                worker.Close();

            if(workerTask != null && workerTask.Status != TaskStatus.RanToCompletion)
            {
                workerTask.Dispose();
                workerTask = null;
            }

            if (milestoneServer != null)
                milestoneServer.Close();
        }

        #endregion OnStop


    }
}
