// Copyright OpenALPR Technology, Inc. 2018

using Database;
using OpenALPRQueueConsumer.BeanstalkWorker;
using OpenALPRQueueConsumer.Chat;
using OpenALPRQueueConsumer.Chatter;
using OpenALPRQueueConsumer.Chatter.Proxy;
using OpenALPRQueueConsumer.Milestone;
using OpenALPRQueueConsumer.Utility;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
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
        private readonly User currentPerson;
        Settings settings = new Settings();

        public ServiceStarter()
        {
            Console.WriteLine("\nEnter ServiceStarter ctor\n");
            #region Use SQLite
            using (DB db = new DB("OpenALPRQueueMilestone", 3))
            {
                Console.WriteLine("\nDB setup enter\n");
                db.CreateTable("Settings");
                Console.WriteLine("\nDB create table\n");
                settings = db.GetSettings("Settings").LastOrDefault();
                Console.WriteLine("\nDB got settings\n");
                if (settings == null)
                {
                    Console.WriteLine("\nDB settings == null\n");
                    settings = db.Defaults();
                    db.SaveSettings("Settings", db.Defaults());
                }
            }
            Console.WriteLine("\nDB setup exit\n");
            ProxySingleton.Port = settings.ServicePort.ToString();
            #endregion
            currentPerson = new User(User.AutoExporterServiceName);
            ProxySingleton.HostName = Dns.GetHostName();
            Chatting.Initialize(currentPerson);
            Chatting.UserEnter += ServerConnection_UserEnter;
            Chatting.UserLeave += ServerConnection_UserLeave;
            Chatting.InfoArrived += ServerConnection_MessageArrived;
            Task.Run(() => Chatting.MonitorClientToServerQueue());
            Chatting.WhisperGui(new Info { MsgId = MessageId.ConnectedToMilestoneServer, Bool = true });
            Console.WriteLine("\nExit ServiceStarter ctor\n");
        }

        #region Start Service

        public void OnStartServiceAsync()
        {
            try
            {
                Console.WriteLine("\nOnStartServiceAsync\n");
                LocalStartService();
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);

                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine(
                    "\nHelpLink ---\n{0}", ex.HelpLink);
                Console.WriteLine("\nSource ---\n{0}", ex.Source);
                Console.WriteLine(
                    "\nStackTrace ---\n{0}", ex.StackTrace);
                Console.WriteLine(
                    "\nTargetSite ---\n{0}", ex.TargetSite);

                OpenALPRQueueMilestone.ServiceInstance.Stop();
                Environment.Exit(1);
            }
        }

        private void LocalStartService()
        {
            Program.Log.Info("Start the service");
            Console.WriteLine("\nStart the service\n");

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal appprincipal = Thread.CurrentPrincipal as WindowsPrincipal;
            Program.Log.Info($"Is in administrators: {appprincipal.IsInRole("Administrators")}");
            Program.Log.Info($"Windows identity: {WindowsIdentity.GetCurrent().Name}"); //Windows identity: NT AUTHORITY\SYSTEM  

            LogSystemInfo();
            WriteExecutingAssemblyVersion();
#if !DEBUG
            using (Impersonation impersonation = new Impersonation(BuiltinUser.NetworkService))
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
                Program.Log.Error(null, ex);
                Program.Log.Info($"Windows identity: {WindowsIdentity.GetCurrent().Name}"); //Windows identity: NT AUTHORITY\NETWORK SERVICE

                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine(
                    "\nHelpLink ---\n{0}", ex.HelpLink);
                Console.WriteLine("\nSource ---\n{0}", ex.Source);
                Console.WriteLine(
                    "\nStackTrace ---\n{0}", ex.StackTrace);
                Console.WriteLine(
                    "\nTargetSite ---\n{0}", ex.TargetSite);
            }
#if !DEBUG
            }
#endif
        }

        private void TryConnectingToMilestoneServer(string serverName = null, string userName = null, string password = null)
        {
            if (MilestoneServer.ConnectedToMilestone)
                return;

            #region Use SQLite
            if (string.IsNullOrEmpty(serverName))
                serverName = settings.MilestoneServerName;

            if (string.IsNullOrEmpty(userName))
                userName = settings.MilestoneUserName;

            if (string.IsNullOrEmpty(password))
                password = settings.MilestonePassword;
            #endregion

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

                #region Use SQLite
                string temp = settings.MilestoneServerName;
                if (temp != serverName)
                {
                    using (DB db = new DB("OpenALPRQueueMilestone", 3))
                    {
                        settings.MilestoneServerName = serverName;
                        db.UpdateSettings("Settings", settings);
                    }
                }

                Worker.AddBookmarks = settings.AddBookmarks;
                Worker.AutoMapping = settings.AutoMapping;
                Worker.EventExpireAfterDays = settings.EventExpireAfterDays;
                Worker.EpochStartSecondsBefore = settings.EpochStartSecondsBefore;
                Worker.EpochEndSecondsAfter = settings.EpochEndSecondsAfter;
                #endregion

                if (worker == null)
                {
                    worker = new Worker();
                    workerTask = Task.Run(() => worker.DoWork());
                }
            }
        }

        private static void LogSystemInfo()
        {
            Program.Log.Info($"OS platform: {Environment.OSVersion.Platform.ToString()}");
            Program.Log.Info($"OS version: {Environment.OSVersion.Version.ToString()}");
            Program.Log.Info($"Service pack version: {Environment.OSVersion.ServicePack.ToString()}");
            Program.Log.Info($"Version string: {Environment.OSVersion.VersionString.ToString()}");
            Program.Log.Info($"Processor count: {Environment.ProcessorCount.ToString(CultureInfo.InvariantCulture)}");
            Program.Log.Info($"CLR version: {Environment.Version.ToString()}");
        }

        private static void WriteExecutingAssemblyVersion()
        {
            FileVersionInfo FileVersion = null;
            try
            {
#if DEBUG
                string name = Assembly.GetExecutingAssembly().GetName().Name;
                FileVersion = FileVersionInfo.GetVersionInfo(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), $"{name}.exe"));
#else
                FileVersion = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName);
#endif
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }

            if (FileVersion != null)
            {
                string version = $"{Program.ProductName} service version: {FileVersion.FileVersion}";
                Program.Log.Info(version);
            }

            string bit = "The future is now!";
            if (IntPtr.Size == 4)
                bit = "32-bit";
            else if (IntPtr.Size == 8)
                bit = "64-bit";

            Program.Log.Info(bit);
        }

        #endregion Start Service

        #region Chat connection

        private void ServerConnection_UserEnter(object sender, ChatEventArgs e)
        {
            if (e != null && e.User != null && !string.IsNullOrEmpty(e.User.Name))
            {
                if (e.User.Name != User.AutoExporterServiceName)
                {
                    string msg = $"New Client: {e.User.Name}";
                    Console.WriteLine(msg);
                    Program.Log.Info(msg);
                }

                //Info info = new Info { MsgId = MessageId.Init, Message1 = CodecList };
                //Chatting.Whisper(e.User.Name, info);
            }
        }

        private void ServerConnection_UserLeave(object sender, ChatEventArgs e)
        {
            Program.Log.Info($"Client left: {e.User.Name}");
        }

        private void ServerConnection_MessageArrived(object sender, ChatEventArgs e)
        {
            switch (e.Message.MessageInfo.MsgId)
            {
                case MessageId.ConnectedToMilestoneToServer:
                    Chatting.WhisperGui(new Info { MsgId = MessageId.ConnectedToMilestoneServer, Bool = IsConnectedToMilestoneServer });
                    break;
            }
        }

        #endregion Chat connection


        #region OnStop

        public void OnStop()
        {
            Chatting.WhisperGui(new Info { MsgId = MessageId.ConnectedToMilestoneServer, Bool = false });
            IsClosing = true;
            Program.Log.Debug("On stop");
            Program.Log.Info($"Stopping {Program.ProductName} service");

            if (worker != null)
                worker.Close();

            if(workerTask != null && workerTask.Status != TaskStatus.RanToCompletion)
            {
                workerTask.Dispose();
                workerTask = null;
            }

            if (milestoneServer != null)
                milestoneServer.Close();

            try
            {
                Chatting.UserEnter -= ServerConnection_UserEnter;
                Chatting.UserLeave -= ServerConnection_UserLeave;
                Chatting.InfoArrived -= ServerConnection_MessageArrived;
                Chatting.Close();
            }
            catch (Exception ex)
            {
                Program.Log.Error("OnStop", ex);
            }
        }

        #endregion OnStop


    }
}
