﻿// Copyright OpenALPR Technology, Inc. 2018

using OpenALPRQueueConsumer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Proxy.Server;
using VideoOS.Platform.Data;
using VideoOS.Platform.License;
using VideoOS.Platform.Messaging;

using SDKEnvironment = VideoOS.Platform.SDK.Environment;

namespace OpenALPRQueueConsumer.Milestone
{
    internal class MilestoneServer
    {
        internal static string ServerName;
        internal static Guid ServerId;
        internal static string Token;
        internal static ServerCommandService scs;
        internal static int ProductCode;

        private static IDictionary<Guid, Item> AllCameras = new Dictionary<Guid, Item>();
        private static LoginSettings loginSettings;
        private static string milestoneUserName;
        private static string milestonePassword;
        private static bool connectedToMilestone;

        private static readonly object LoginLock = new object();

        public MilestoneServer()
        {

        }

        public static bool ConnectedToMilestone
        {
            get { return connectedToMilestone; }
            set
            {
                lock (LoginLock)
                {
                    if (connectedToMilestone != value)
                    {
                        connectedToMilestone = value;
                        if (!value && !ServiceStarter.IsClosing)
                            OnPropertyChanged(nameof(ConnectedToMilestone));
                    }
                }
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected static void OnPropertyChanged(string name)
        {
            using (Impersonation impersonation = new Impersonation(BuiltinUser.NetworkService))
            {
                Logout();
                LoginUsingCurrentCredentials(ServerName, milestoneUserName, milestonePassword);
            }
        }

        internal static void LoginUsingCurrentCredentials(string serverName, string userName, string password)
        {
            try
            {
                bool firstTimeMessage = true;
                while (!connectedToMilestone && !ServiceStarter.IsClosing)
                {
                    PrepareToLogin(serverName, userName, password);
                    if (!connectedToMilestone && firstTimeMessage)
                    {
                        firstTimeMessage = false;
                        Program.Log.Warn("Failed to connect to Milestone");
                    }

                    Thread.Sleep(1000);
                }

                if (connectedToMilestone)
                    Program.Log.Info("Connected to Milestone");
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
                Program.Log.Error(null, ex.InnerException);
            }
        }

        private static void PrepareToLogin(string serverName, string userName, string password)
        {
            ServerName = serverName;
            milestoneUserName = userName;
            milestonePassword = password;

            if (string.IsNullOrEmpty(ServerName))
            {
                string msg = "Server address cannot be emnpty";
                Program.Log.Warn(msg);
                Console.WriteLine(msg);
                return;
            }

            Uri uri = null;
            CredentialCache cc = null;
            NetworkCredential nc = null;
            bool logged = false;

            if (!string.IsNullOrEmpty(milestoneUserName))
            {
                // You need this to apply Enterprise "basic" credentials.
                uri = new UriBuilder(ServerName).Uri;
                cc = Util.BuildCredentialCache(uri, milestoneUserName, milestonePassword, "Negotiate");//"Basic" : "Negotiate"
                SDKEnvironment.AddServer(uri, cc);
                // use the same credentiel as when logge on to the Management Server
                logged = Login(uri);
            }

            if (!logged)
            {
                try
                {
                    uri = new UriBuilder(ServerName).Uri;
                }
                catch (Exception ex)
                {
                    Program.Log.Error(ServerName, ex);
                    Program.Log.Error(ServerName, ex.InnerException);
                    return;
                }

                // This will reuse the Windows credentials you are logged in with
                nc = CredentialCache.DefaultNetworkCredentials;
                SDKEnvironment.AddServer(uri, nc);

                //Pick up the login settings from the management server to be used when logging into the Recording Server.
                loginSettings = LoginSettingsCache.GetLoginSettings(ServerName);

                logged = Login(uri);
            }

            if (!logged)
            {
                uri = new Uri($"http://{Configuration.Instance.ServerFQID.ServerId.ServerHostname}:{Configuration.Instance.ServerFQID.ServerId.Serverport}/");
                SDKEnvironment.AddServer(uri, nc);
                logged = Login(uri);
            }

            if (!logged && loginSettings != null)
            {
                SDKEnvironment.AddServer(uri, nc);
                logged = Login(loginSettings.Uri);
            }

            if (!logged)
            {
                Program.Log.Error("Doesn't logged in.");
                return;
            }

            try
            {
                if (!SDKEnvironment.IsServerConnected(uri))
                {
                    string msg = $"Failed to connect to {uri}";
                    Program.Log.Warn(msg);
                    return;
                }

                scs = new ServerCommandService();
                scs.Timeout *= 3;                                       // The default is 100,000 milliseconds.

                // use the same credentiel as when logge on to the Management Server
                if (cc != null)
                    scs.Credentials = cc;
                else if (nc != null)
                    scs.Credentials = nc;

                connectedToMilestone = true;
                List<Item> items = Configuration.Instance.GetItems();
                EnumerateElementChildren(items, AllCameras);
                ProductCode = EnvironmentManager.Instance.SystemLicense.ProductCode;
                ServerId = Configuration.Instance.ServerFQID.ServerId.Id;
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Internal error connecting to: {uri.DnsSafeHost}", ex);
            }

            if (loginSettings != null)
                Token = loginSettings.Token;

            if (connectedToMilestone)
            {
                MilestoneVersion milestoneVersion = new MilestoneVersion();

                //OpenALPRPlugin.Utility.Milestone.Version = (OpenALPRPlugin.Utility.MilestoneVersion)(new MappingHelper(milestoneVersion, new OpenALPRPlugin.Utility.MilestoneVersion()).Value);

                scs.Url = $"{ServerName}ServerAPI/ServerCommandService.asmx";
                MilestoneInfo();
                string msg = $"Connected to Milestone server: {ServerName}";
                Console.WriteLine(msg);
                Program.Log.Info(msg);
            }
            else
                Console.WriteLine($"Failed to connect to {ServerName}");
        }

        private static bool Login(Uri uri, bool masterOnly = false)
        {
            if (EnvironmentManager.Instance.MasterSite.ServerId.ServerType == VideoOS.Platform.ServerId.EnterpriseServerType)
            {
                Console.WriteLine("Bookmark is not supported on this product.");
                Console.ReadKey();
                return true;
            }

            try
            {
                SDKEnvironment.Login(uri);
                return true;
            }
            catch (Exception exception)
            {
                if (exception.Message.Length != 0)
                {
                    Program.Log.Info($"{Environment.NewLine}URI: {uri}{Environment.NewLine}Source: {exception.Source}{Environment.NewLine}Message: {exception.Message}{Environment.NewLine}Exception: {exception.ToString()}");
                    if (exception.InnerException != null)
                    {
                        Program.Log.Info($"{Environment.NewLine}Inner Exception:{Environment.NewLine}Message: {exception.InnerException.Message}{Environment.NewLine}Source: {exception.InnerException.Source}{Environment.NewLine}");
                    }
                }

                SDKEnvironment.RemoveServer(uri);
            }

            try
            {
                SDKEnvironment.Login(uri, true);
                return true;
            }
            catch (Exception exception)
            {
                if (exception.Message.Length != 0)
                {
                    Program.Log.Info($"{Environment.NewLine}URI (true): {uri}{Environment.NewLine}Source: {exception.Source}{Environment.NewLine}Message: {exception.Message}{Environment.NewLine}Exception: {exception.ToString()}");
                    if (exception.InnerException != null)
                    {
                        Program.Log.Info($"{Environment.NewLine}Inner Exception:{Environment.NewLine}Message: {exception.InnerException.Message}{Environment.NewLine}Source: {exception.InnerException.Source}{Environment.NewLine}");
                    }
                }

                SDKEnvironment.RemoveServer(uri);
            }

            return false;
        }

        private static void MilestoneInfo()
        {
            try
            {
                Program.Log.Info($"Version: {scs.GetVersion()}");
                Program.Log.Info($"Server version: {scs.GetServerVersion()}");
                Program.Log.Info($"Smart Client name: {scs.GetSmartClientVersion().DisplayName}");
                Program.Log.Info($"Smart Client version: {scs.GetSmartClientVersion().Major.ToString()}.{scs.GetSmartClientVersion().Minor.ToString()}.{scs.GetSmartClientVersion().Revision.ToString()}");
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex.InnerException);
                Program.Log.Error(null, ex);
            }
        }

        internal static Item[] GetCameras(Guid[] objectsId)
        {
            IList<Item> list = new List<Item>(objectsId.Length);
            foreach (Guid objectId in objectsId)
            {
                KeyValuePair<Guid, Item> item = AllCameras.FirstOrDefault(c => c.Key == objectId);
                if (item.Key != Guid.Empty)
                    list.Add(item.Value);
            }

            return list.ToArray();
        }

        private static object LocalConfigUpdatedHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            // Update UI - if name changed
            return null;
        }

        private static object ConfigUpdatedHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            return null;
        }

        internal static string GetCameraName(Guid objectId)
        {
            KeyValuePair<Guid, Item> item = AllCameras.FirstOrDefault(c => c.Key == objectId);
            if (item.Key == objectId)
                return item.Value.Name;

            return string.Empty;
        }

        internal static Item GetCameraItem(string ip)
        {
            KeyValuePair<Guid, Item> item = AllCameras.FirstOrDefault(c => c.Value.Name.Contains(ip));
            if (item.Value != null)
                return item.Value;

            return null;
        }

        internal static FQID GetCameraByName(string name)
        {
            IDictionary<Guid, Item> allCameras = AllCameras;

            KeyValuePair<Guid, Item> item = AllCameras.FirstOrDefault(c => c.Value.Name == name);
            if (item.Value != null)
                return item.Value.FQID;

            return null;
        }

        private static void EnumerateElementChildren(List<Item> items, IDictionary<Guid, Item> dicFoundConfigItems)
        {
            foreach (Item item in items ?? new List<Item>())
            {
                if (item.FQID.Kind == Kind.Camera && item.FQID.FolderType == FolderType.No)
                {
                    if (!dicFoundConfigItems.ContainsKey(item.FQID.ObjectId))
                        dicFoundConfigItems.Add(item.FQID.ObjectId, item);
                }

                if (item.HasChildren != HasChildren.No &&
                    (item.FQID.Kind == Kind.Server || item.FQID.Kind == Kind.Folder || item.FQID.Kind == Kind.Camera))
                {
                    List<Item> subs = item.GetChildren();
                    EnumerateElementChildren(subs, dicFoundConfigItems);
                }
            }
        }

        internal static IDictionary<Guid, Item> AllAvilabelCameras
        {
            get { return AllCameras; }
        }

        internal static int CamerasCount
        {
            get { return AllCameras.Count; }
        }

        public void Close()
        {
            connectedToMilestone = false;
            if (loginSettings != null)
                loginSettings = null;

            Logout();
        }

        private static void Logout()
        {
            try
            {
                SDKEnvironment.Logout(new Uri(ServerName));
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }

            try
            {
                if (scs != null)
                {
                    scs.Abort();
                    scs.Dispose();
                    scs = null;
                }

                SDKEnvironment.RemoveAllServers();
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }
        }


    }
}