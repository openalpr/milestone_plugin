﻿// Copyright OpenALPR Technology, Inc. 2018

using OpenALPRQueueConsumer.NativeMethods;
using System;
using System.Security.Principal;

namespace OpenALPRQueueConsumer.Utility
{
    /// <summary>
    /// An impersonation class (modified from http://born2code.net/?page_id=45) that supports LocalService and NetworkService logons.
    /// Note: To use these built-in logons the code must be running under the local system account.
    /// </summary>
    internal class Impersonation : IDisposable
    {
        #region Private members
        /// <summary>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Holds the created impersonation context and will be used
        /// for reverting to previous user.
        /// </summary>
        private WindowsImpersonationContext _impersonationContext;
        #endregion

        #region Ctor & Dtor

        /// <summary>
        /// Initializes a new instance of the <see cref="Impersonation"/> class and
        /// impersonates as a built in service account.
        /// </summary>
        /// <param name="builtinUser">The built in user to impersonate - either
        /// Local Service or Network Service. These users can only be impersonated
        /// by code running as System.</param>
        public Impersonation(BuiltinUser builtinUser)
            : this(string.Empty, "NT AUTHORITY", string.Empty, LogonType.Service, builtinUser)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Impersonation"/> class and
        /// impersonates with the specified credentials.
        /// </summary>
        /// <param name="username">his is the name of the user account to log on 
        /// to. If you use the user principal name (UPN) format, 
        /// user@DNS_domain_name, the lpszDomain parameter must be <c>null</c>.</param>
        /// <param name="domain">The name of the domain or server whose account 
        /// database contains the lpszUsername account. If this parameter is 
        /// <c>null</c>, the user name must be specified in UPN format. If this 
        /// parameter is ".", the function validates the account by using only the 
        /// local account database.</param>
        /// <param name="password">The plaintext password for the user account.</param>
        public Impersonation(string username, string domain, string password)
            : this(username, domain, password, LogonType.Interactive, BuiltinUser.None)
        {
        }

        private Impersonation(string username, string domain, string password, LogonType logonType, BuiltinUser builtinUser)
        {
            switch (builtinUser)
            {
                case BuiltinUser.None:
                    if (string.IsNullOrEmpty(username))
                        return;
                    break;

                case BuiltinUser.LocalService:
                    username = "LOCAL SERVICE";
                    break;

                case BuiltinUser.NetworkService:
                    username = "NETWORK SERVICE";
                    break;
            }

            IntPtr userToken = IntPtr.Zero;
            IntPtr userTokenDuplication = IntPtr.Zero;

            // Logon with user and get token.
            bool loggedOn = NativeMethod.LogonUser(username, domain, password, logonType, LogonProvider.Default, out userToken);
            if (loggedOn)
            {
                try
                {
                    // Create a duplication of the usertoken, this is a solution
                    // for the known bug that is published under KB article Q319615.
                    if (NativeMethod.DuplicateToken(userToken, 2, ref userTokenDuplication))
                    {
                        // Create windows identity from the token and impersonate the user.
                        WindowsIdentity identity = new WindowsIdentity(userTokenDuplication);
                        _impersonationContext = identity.Impersonate();
                    }
                    //else
                    //{
                    //    // Token duplication failed!
                    //    // Use the default ctor overload
                    //    // that will use Mashal.GetLastWin32Error();
                    //    // to create the exceptions details.
                    //    throw new Win32Exception();
                    //}
                }
                finally
                {
                    // Close usertoken handle duplication when created.
                    if (!userTokenDuplication.Equals(IntPtr.Zero))
                    {
                        // Closes the handle of the user.
                        NativeMethod.CloseHandle(userTokenDuplication);
                        userTokenDuplication = IntPtr.Zero;
                    }

                    // Close usertoken handle when created.
                    if (!userToken.Equals(IntPtr.Zero))
                    {
                        // Closes the handle of the user.
                        NativeMethod.CloseHandle(userToken);
                        userToken = IntPtr.Zero;
                    }
                }
            }
            //else
            //{
            //    // Logon failed!
            //    // Use the default ctor overload that 
            //    // will use Mashal.GetLastWin32Error();
            //    // to create the exceptions details.
            //    throw new Win32Exception();
            //}
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Born2Code.Net.Impersonation"/> is reclaimed by garbage collection.
        /// </summary>
        ~Impersonation()
        {
            Dispose(false);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Reverts to the previous user.
        /// </summary>
        public void Revert()
        {
            if (_impersonationContext != null)
            {
                // Revert to previour user.
                _impersonationContext.Undo();
                _impersonationContext = null;
            }
        }
        #endregion

        #region IDisposable implementation.
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources and will revent to the previous user when
        /// the impersonation still exists.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources and will revent to the previous user when
        /// the impersonation still exists.
        /// </summary>
        /// <param name="disposing">Specify <c>true</c> when calling the method directly
        /// or indirectly by a user’s code; Otherwise <c>false</c>.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Revert();

                _disposed = true;
            }
        }
        #endregion
    }

    #region Enums

    public enum LogonType : int
    {
        /// <summary>
        /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
        /// by a terminal server, remote shell, or similar process.
        /// This logon type has the additional expense of caching logon information for disconnected operations;
        /// therefore, it is inappropriate for some client/server applications,
        /// such as a mail server.
        /// </summary>
        Interactive = 2,

        /// <summary>
        /// This logon type is intended for high performance servers to authenticate plaintext passwords.
        /// The LogonUser function does not cache credentials for this logon type.
        /// </summary>
        Network = 3,

        /// <summary>
        /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without
        /// their direct intervention. This type is also for higher performance servers that process many plaintext
        /// authentication attempts at a time, such as mail or Web servers.
        /// The LogonUser function does not cache credentials for this logon type.
        /// </summary>
        Batch = 4,

        /// <summary>
        /// Indicates a service-type logon. The account provided must have the service privilege enabled.
        /// </summary>
        Service = 5,

        /// <summary>
        /// This logon type is for GINA DLLs that log on users who will be interactively using the computer.
        /// This logon type can generate a unique audit record that shows when the workstation was unlocked.
        /// </summary>
        Unlock = 7,

        /// <summary>
        /// This logon type preserves the name and password in the authentication package, which allows the server to make
        /// connections to other network servers while impersonating the client. A server can accept plaintext credentials
        /// from a client, call LogonUser, verify that the user can access the system across the network, and still
        /// communicate with other servers.
        /// NOTE: Windows NT:  This value is not supported.
        /// </summary>
        NetworkCleartText = 8,

        /// <summary>
        /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
        /// The new logon session has the same local identifier but uses different credentials for other network connections.
        /// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
        /// NOTE: Windows NT:  This value is not supported.
        /// </summary>
        NewCredentials = 9,
    }

    public enum LogonProvider : int
    {
        /// <summary>
        /// Use the standard logon provider for the system.
        /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name
        /// is not in UPN format. In this case, the default provider is NTLM.
        /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
        /// </summary>
        Default = 0,
    }

    public enum BuiltinUser
    {
        None,
        LocalService,
        NetworkService
    }

    public enum MipVersion
    {
        Unknown = 000,
        Go = 100,
        Essential = 200,
        Express = 250,
        Professional = 300,
        Enterprise = 400,
        EssentialPlus  = 440,
        ExpressPlus = 460,
        ProfessionalPlus = 480,
        Expert = 500,
        Corporate = 600,
    }

    public enum MipFeatures
    {
        Audio, 
        DeviceChannels, 
        DLKQuota, 
        Sites, 
        Metadata,
        FederatedSites,InterconnectedProducts,
        AddToFederatedHierarchy, 
        AACAudioSupport, 
        RemoteConnectedServices, 
        AxisOneClick, 
        HardwareLicensing, 
        RecordingServerFailover, 
        SmartClientProfiles, 
        ManagementClientProfiles, 
        DifferentiatedAdministratorSecurity, 
        SNMP, 
        TimeControlledAccessRights, 
        EvidenceLock, 
        MultiStageVideoStorage, 
        ArchiveReducedFrameRate, 
        RecordingStorageEncryption, 
        RecordingStorageSigning, 
        MarkedData, 
        Bookmarking, 
        RuleBasedBookmarking, 
        LiveMultiStreaming, 
        Multicast, 
        ManageAlarms, 
        DualAuthorization, 
        PTZPriority, 
        EdgeStorage, 
        ConfigurationReports, 
        SystemMonitor, 
        SmartClientLogin, 
        MobileLogin, 
        MIPUseInternally, 
        MIPUseExternally, 
        SmartMap, 
        ExtendedPtz, 
        CustomerDashboard, 
        HardwareAcceleratedVMD, 
        TwoStepVerification, 
        DLNAOutServer, 
        ExtendedPrivacyMasking, 
        NvidiaVMD, 
        DevicePasswordManagement, 
        SearchCategories, 
        SaveSearches, 
        AdaptiveStreaming, 
        SmartWall
    }

    #endregion

}
