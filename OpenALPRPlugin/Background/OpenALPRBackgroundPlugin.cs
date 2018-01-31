using OpenALPRPlugin.Utility;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Data;

namespace OpenALPRPlugin.Background
{
    class OpenALPRBackgroundPlugin : BackgroundPlugin
    {
        internal static bool IsDebuggerPresent = false;
        internal static bool Stop = false;
        internal static int CamerasCount;
        internal static int ProductCode;
        internal static Bookmark[] Bookmarks;
        internal static bool MyOwnBookmarksOnly;
        internal static string SearchString;
        internal const string openalprRefrence = "openalpr";
        internal static event EventHandler<MessageEventArgs> ServiceEvent;

        private static Timer aTimer;

        public override void Init()
        {
            Logger.Log.Info($"Init {nameof(OpenALPRBackgroundPlugin)}");

            LogSystemInfo();
            var versionString = EnvironmentManager.Instance.EnvironmentProduct;//Milestone XProtect Smart Client9.0c
            versionString = versionString.Replace("Milestone XProtect Smart Client", string.Empty);
            versionString = versionString.Substring(0, versionString.Length - 1);
            float.TryParse(versionString, out float version);
            OpenALPRPluginDefinition.Version = version;
            FindCamerasCount();

            Stop = false;

            if (EnvironmentManager.Instance.MasterSite.ServerId.ServerType == ServerId.EnterpriseServerType)
                Logger.Log.Warn("There is no bookmark support in Enterprise");

            SetTimer();
        }

        private static void LogSystemInfo()
        {
            ProductCode = EnvironmentManager.Instance.LicenseManager.ProductCode;
            Logger.Log.Info($"ProductCode: {ProductCode.ToString()}");
            Logger.Log.Info($"Product: {EnvironmentManager.Instance.EnvironmentProduct}");
            Logger.Log.Info($"Type: {EnvironmentManager.Instance.EnvironmentType.ToString()}");
            Logger.Log.Info($"Mode: {EnvironmentManager.Instance.Mode.ToString()}");
        }

        private static void SetTimer()
        {
            aTimer = new Timer(60000); // 1 min.
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                var service = new ServiceController("OpenALPRMilestone");

                if (service != null)
                {
                    if (service.Status != ServiceControllerStatus.Running)
                    {
                        Logger.Log.Info("starting OpenALPRMilestone service");
                        ServiceEvent?.Invoke(null, new MessageEventArgs($"{DateTime.Now.ToString()}: OpenALPR Milestone service was not running, restarted now."));

                        service.Start();
                    }
                }
                else
                    Logger.Log.Warn("service object is null");
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }
        }

        private void FindCamerasCount()
        {
            var cameraItems = new List<Item>();
            FindAllCameras(Configuration.Instance.GetItemsByKind(Kind.Camera), cameraItems);
            CamerasCount = cameraItems.Count;
        }

        internal static void FindAllCameras(List<Item> searchItems, List<Item> foundCameraItems)
        {
            for (int i = 0; i < searchItems.Count; i++)
            {
                var searchItem = searchItems[i];
                if (searchItem.FQID.Kind == Kind.Camera && searchItem.FQID.FolderType == FolderType.No)
                {
                    bool cameraAlreadyFound = false;
                    foreach (Item foundCameraItem in foundCameraItems)
                    {
                        if (foundCameraItem.FQID.Equals(searchItem.FQID))
                        {
                            cameraAlreadyFound = true;
                            break;
                        }
                    }
                    if (!cameraAlreadyFound)
                        foundCameraItems.Add(searchItem);
                }
                else
                    FindAllCameras(searchItem.GetChildren(), foundCameraItems);
            }
        }

        public override Guid Id
        {
            get { return OpenALPRPluginDefinition.BackgroundPlugin; }
        }

        public override string Name
        {
            get { return OpenALPRPluginDefinition.PlugName; }
        }

        public override List<EnvironmentType> TargetEnvironments
        {
            get { return new List<EnvironmentType>() { EnvironmentType.SmartClient }; }
        }

        /// <summary>
        /// Called by the Environment when the user log's out.
        /// You should close all remote sessions and flush cache information, as the
        /// user might log on to another server next time.
        /// </summary>
        public override void Close()
        {
            Stop = true;
            if (aTimer != null)
            {
                aTimer.Elapsed -= OnTimedEvent;
                aTimer.Stop();
                aTimer.Dispose();
            }
        }
    }

    internal class MessageEventArgs : EventArgs
    {
        private readonly string message;

        public MessageEventArgs(string message)
        {
            this.message = message;
        }

        public string Message
        {
            get { return message; }
        }
    }
}