using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenALPRPlugin.Utility
{
    public static class Milestone
    {
        public static MilestoneVersion Version { get; set; } = new MilestoneVersion();
    }

    public class MilestoneVersion
    {
        public int ProductCode { get; set; }
        public string SLC { get; set; }
        public DateTime Expire { get; set; }
        public DateTime Issued { get; set; }
        public string Name { get; set; }
        public bool Bookmarking { get; set; }
        public bool Audio { get; set; }
        public bool DeviceChannels { get; set; }
        public bool DLKQuota { get; set; }
        public bool Sites { get; set; }
        public bool Metadata { get; set; }
        public bool FederatedSites { get; set; }
        public bool InterconnectedProducts { get; set; }
        public bool AddToFederatedHierarchy { get; set; }
        public bool AACAudioSupport { get; set; }
        public bool RemoteConnectedServices { get; set; }
        public bool AxisOneClick { get; set; }
        public bool HardwareLicensing { get; set; }
        public bool RecordingServerFailover { get; set; }
        public bool SmartClientProfiles { get; set; }
        public bool ManagementClientProfiles { get; set; }
        public bool DifferentiatedAdministratorSecurity { get; set; }
        public bool SNMP { get; set; }
        public bool TimeControlledAccessRights { get; set; }
        public bool EvidenceLock { get; set; }
        public bool MultiStageVideoStorage { get; set; }
        public bool ArchiveReducedFrameRate { get; set; }
        public bool RecordingStorageEncryption { get; set; }
        public bool RecordingStorageSigning { get; set; }
        public bool MarkedData { get; set; }
        public bool RuleBasedBookmarking { get; set; }
        public bool LiveMultiStreaming { get; set; }
        public bool Multicast { get; set; }
        public bool ManageAlarms { get; set; }
        public bool DualAuthorization { get; set; }
        public bool PTZPriority { get; set; }
        public bool EdgeStorage { get; set; }
        public bool ConfigurationReports { get; set; }
        public bool SystemMonitor { get; set; }
        public bool SmartClientLogin { get; set; }
        public bool MobileLogin { get; set; }
        public bool MIPUseInternally { get; set; }
        public bool MIPUseExternally { get; set; }
        public bool SmartMap { get; set; }
        public bool ExtendedPtz { get; set; }
        public bool CustomerDashboard { get; set; }
        public bool HardwareAcceleratedVMD { get; set; }
        public bool TwoStepVerification { get; set; }
        public bool DLNAOutServer { get; set; }
        public bool ExtendedPrivacyMasking { get; set; }
        public bool NvidiaVMD { get; set; }
        public bool DevicePasswordManagement { get; set; }
        public bool SearchCategories { get; set; }
        public bool SaveSearches { get; set; }
        public bool AdaptiveStreaming { get; set; }
        public bool SmartWall { get; set; }
        public bool Connection { get; set; }
    }

}
