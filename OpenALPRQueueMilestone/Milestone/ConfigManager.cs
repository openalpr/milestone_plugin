using OpenALPRQueueConsumer.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace OpenALPRQueueConsumer.Milestone
{
    /// <summary>
    /// This class helps getting the configuration updated for a single site, 
    /// it is not intended to be used in a MFA setup.
    /// </summary>
    public class ConfigManager
    {
        private MessageCommunication _messageCommunication;
        private object _systemConfigurationChangedIndicationRefefence;
        private Timer _catchUpTimer;

        public void Init()
        {
            _catchUpTimer = new Timer(CatchUpTimerHandler);

            try
            {
                MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
                _messageCommunication = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);

                _systemConfigurationChangedIndicationRefefence = _messageCommunication.RegisterCommunicationFilter(SystemConfigChangedHandler2,
                    new VideoOS.Platform.Messaging.CommunicationIdFilter(MessageId.System.SystemConfigurationChangedIndication));

            }
            catch (MIPException ex)
            {
                Trace.WriteLine("Message Communcation not supported:" + ex.Message);
            }
        }

        public void Close()
        {
            _messageCommunication.UnRegisterCommunicationFilter(_systemConfigurationChangedIndicationRefefence);
            MessageCommunicationManager.Stop(EnvironmentManager.Instance.MasterSite.ServerId);
        }

        /// <summary>
        /// This method is called, when the EventServer did not report the detailed configuration message.
        /// Can happen for older systems, or because the EventServer is very busy.
        /// 
        /// As a fail-safe we reload the entire configuration after 60 seconds
        /// </summary>
        /// <param name="o"></param>
        private void CatchUpTimerHandler(object o)
        {
            // Disable timer
            _catchUpTimer.Change(Timeout.Infinite, Timeout.Infinite);
            // Reload everything
            VideoOS.Platform.SDK.Environment.ReloadConfiguration(Configuration.Instance.ServerFQID);
        }

        /// <summary>
        /// Message comming from EventServer with information about what is changed
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private object SystemConfigChangedHandler2(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            System.Collections.Generic.List<FQID> fqids = message.Data as System.Collections.Generic.List<FQID>;
            if (fqids == null)
            {
                // Start timer, when the detailed info is not available.  
                _catchUpTimer.Change(TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));
                return null;
            }

            // Detailed info received - stop catchup timer
            _catchUpTimer.Change(Timeout.Infinite, Timeout.Infinite);     // Disable timer, as we now have the detailed changes

            List<FQID> recorderFQIDList = new List<FQID>();
            foreach (FQID fqid in fqids)
            {
                Item item = Configuration.Instance.GetItem(fqid);
                if (item != null)
                {
                    Trace.WriteLine("SystemConfigurationChangedIndication - received -- for: " + item.Name);
                    FQID recorderFQID;
                    if (fqid.Kind == Kind.Server)
                        recorderFQID = fqid;
                    else
                        recorderFQID = fqid.GetParent();
                    if (recorderFQID != null && recorderFQIDList.Contains(recorderFQID) == false)
                        recorderFQIDList.Add(recorderFQID);
                }
                else
                {
                    Trace.WriteLine("SystemConfigurationChangedIndication - received -- for: Unknown Item");
                }
            }


            Thread reloadThread = new Thread(new ParameterizedThreadStart(ReloadConfigurationThread));
            reloadThread.Start(recorderFQIDList);
            
            return null;

        }

        /// <summary>
        /// Make sure we reload configuration from another thread
        /// </summary>
        /// <param name="obj"></param>
        private void ReloadConfigurationThread(object obj)
        {
            List<FQID> recorderFQIDList = obj as List<FQID>;
            if (recorderFQIDList != null)
            {
                // Now ask SDK to reload configuration from server, this will issue the "LocalConfigurationChangedIndication"
                foreach (FQID recorderFQID in recorderFQIDList)
                    VideoOS.Platform.SDK.Environment.ReloadConfiguration(recorderFQID);                
            }

        }
    }

    internal class MilestoneVersion
    {
        private ConfigManager _configManager;

        public MilestoneVersion()
        {
            _configManager = new ConfigManager();
            _configManager.Init();
        }
        public int ProductCode { get; set; } = EnvironmentManager.Instance.SystemLicense.ProductCode;
        public string SLC { get; set; } = EnvironmentManager.Instance.SystemLicense.SLC;
        public DateTime Expire { get; set; } = EnvironmentManager.Instance.SystemLicense.Expire;
        public DateTime Issued { get; set; } = EnvironmentManager.Instance.SystemLicense.Issued;
        public string Name { get; set; } = ((MipVersion)Enum.ToObject(typeof(MipVersion), EnvironmentManager.Instance.SystemLicense.ProductCode)).ToString();
        public bool Bookmarking { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.Bookmarking.ToString());
        public bool Audio { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.Audio.ToString()); 
        public bool DeviceChannels { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.DeviceChannels.ToString()); 
        public bool DLKQuota { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.DLKQuota.ToString()); 
        public bool Sites { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.Sites.ToString()); 
        public bool Metadata { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.Metadata.ToString());
        public bool FederatedSites { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.FederatedSites.ToString());
        public bool InterconnectedProducts { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.InterconnectedProducts.ToString());
        public bool AddToFederatedHierarchy { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.AddToFederatedHierarchy.ToString()); 
        public bool AACAudioSupport { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.AACAudioSupport.ToString()); 
        public bool RemoteConnectedServices { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.RemoteConnectedServices.ToString()); 
        public bool AxisOneClick { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.AxisOneClick.ToString()); 
        public bool HardwareLicensing { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.HardwareLicensing.ToString()); 
        public bool RecordingServerFailover { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.RecordingServerFailover.ToString()); 
        public bool SmartClientProfiles { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.SmartClientProfiles.ToString()); 
        public bool ManagementClientProfiles { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.ManagementClientProfiles.ToString()); 
        public bool DifferentiatedAdministratorSecurity { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.DifferentiatedAdministratorSecurity.ToString()); 
        public bool SNMP { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.SNMP.ToString()); 
        public bool TimeControlledAccessRights { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.TimeControlledAccessRights.ToString()); 
        public bool EvidenceLock { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.EvidenceLock.ToString()); 
        public bool MultiStageVideoStorage { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.MultiStageVideoStorage.ToString()); 
        public bool ArchiveReducedFrameRate { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.ArchiveReducedFrameRate.ToString()); 
        public bool RecordingStorageEncryption { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.RecordingStorageEncryption.ToString()); 
        public bool RecordingStorageSigning { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.RecordingStorageSigning.ToString()); 
        public bool MarkedData { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.MarkedData.ToString());
        public bool RuleBasedBookmarking { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.RuleBasedBookmarking.ToString()); 
        public bool LiveMultiStreaming { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.LiveMultiStreaming.ToString()); 
        public bool Multicast { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.Multicast.ToString()); 
        public bool ManageAlarms { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.ManageAlarms.ToString()); 
        public bool DualAuthorization { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.DualAuthorization.ToString()); 
        public bool PTZPriority { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.PTZPriority.ToString()); 
        public bool EdgeStorage { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.EdgeStorage.ToString()); 
        public bool ConfigurationReports { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.ConfigurationReports.ToString()); 
        public bool SystemMonitor { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.SystemMonitor.ToString()); 
        public bool SmartClientLogin { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.SmartClientLogin.ToString()); 
        public bool MobileLogin { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.MobileLogin.ToString()); 
        public bool MIPUseInternally { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.MIPUseInternally.ToString()); 
        public bool MIPUseExternally { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.MIPUseExternally.ToString()); 
        public bool SmartMap { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.SmartMap.ToString()); 
        public bool ExtendedPtz { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.ExtendedPtz.ToString()); 
        public bool CustomerDashboard { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.CustomerDashboard.ToString()); 
        public bool HardwareAcceleratedVMD { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.HardwareAcceleratedVMD.ToString()); 
        public bool TwoStepVerification { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.TwoStepVerification.ToString()); 
        public bool DLNAOutServer { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.DLNAOutServer.ToString()); 
        public bool ExtendedPrivacyMasking { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.ExtendedPrivacyMasking.ToString()); 
        public bool NvidiaVMD { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.NvidiaVMD.ToString()); 
        public bool DevicePasswordManagement { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.DevicePasswordManagement.ToString()); 
        public bool SearchCategories { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.SearchCategories.ToString()); 
        public bool SaveSearches { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.SaveSearches.ToString()); 
        public bool AdaptiveStreaming { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.AdaptiveStreaming.ToString());
        public bool SmartWall { get; set; } = EnvironmentManager.Instance.SystemLicense.IsFeatureEnabled(MipFeatures.SmartWall.ToString());
        public bool Connection { get; set; } = false;
    }
}
