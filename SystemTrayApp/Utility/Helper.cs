// Copyright OpenALPR Technology, Inc. 2018

using log4net.Repository.Hierarchy;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using SystemTrayApp;

namespace OpenALPR.SystemTrayApp.Utility
{
    internal static class Helper
    {
        public static void SetDirectoryNetworkServiceAccessControl(string path)
        {
            if (!Directory.Exists(path))
            {
                Program.Log.Warn($"Path does not exists: {path}");
                return;
            }

            //WorldSid = "everyone"
            //SID: S - 1 - 5 - 20
            //Name: NT Authority
            //Description: Network Service
            const string sddlForm = "S-1-5-20";
            string sid = string.Empty;

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                DirectorySecurity dirSecurity = dirInfo.GetAccessControl();
                foreach (AuthorizationRule r in dirSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    if (r.IdentityReference.ToString() == sddlForm)
                    {
                        sid = r.IdentityReference.ToString();
                        break;
                    }
                }

                if (sid.Length == 0)
                {
                    SecurityIdentifier networkService = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null); // or new SecurityIdentifier(sddlForm);
                    IdentityReference networkServiceIdentity = networkService.Translate(typeof(SecurityIdentifier));
                    FileSystemAccessRule accessRule = new FileSystemAccessRule(networkServiceIdentity,
                         fileSystemRights: FileSystemRights.FullControl,
                         inheritanceFlags: InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                         propagationFlags: PropagationFlags.NoPropagateInherit,
                         type: AccessControlType.Allow);


                    dirSecurity.AddAccessRule(accessRule);
                    dirInfo.SetAccessControl(dirSecurity);
                    dirInfo.Refresh();
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(path, ex);
            }

            if (!SetAcl(path))
                Program.Log.Warn($"Failed to set acl for path: {path}");
        }

        private static bool SetAcl(string path)
        {
            const string Users = "Users";
            try
            {
                FileSystemRights Rights = FileSystemRights.FullControl;

                // *** Add Access Rule to the actual directory itself
                FileSystemAccessRule AccessRule = new FileSystemAccessRule(Users, Rights,
                                            InheritanceFlags.None,
                                            PropagationFlags.NoPropagateInherit,
                                            AccessControlType.Allow);

                DirectoryInfo Info = new DirectoryInfo(path);
                DirectorySecurity Security = Info.GetAccessControl(AccessControlSections.Access);

                Security.ModifyAccessRule(AccessControlModification.Set, AccessRule, out bool Result);

                if (!Result)
                    return false;

                // *** Always allow objects to inherit on a directory
                InheritanceFlags iFlags = InheritanceFlags.ObjectInherit;
                iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

                // *** Add Access rule for the inheritance
                AccessRule = new FileSystemAccessRule(Users, Rights,
                                            iFlags,
                                            PropagationFlags.InheritOnly,
                                            AccessControlType.Allow);
                Result = false;
                Security.ModifyAccessRule(AccessControlModification.Add, AccessRule, out Result);

                if (!Result)
                    return false;

                Info.SetAccessControl(Security);
                Info.Refresh();

                return true;
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }

            return false;
        }

        internal static string ReadConfigKey(string key, string file = null)
        {
            try
            {
                string fullPath = $"{OpenALPRQueueMilestoneDefinition.applicationPath}\\{file}";
                //string fullPath = $"{file}";

                if (string.IsNullOrEmpty(fullPath) || string.IsNullOrWhiteSpace(fullPath))
                {
                    return ConfigurationManager.AppSettings[key];
                }
                else
                {
                    Configuration configFile = ConfigurationManager.OpenExeConfiguration(fullPath);
                    return configFile.AppSettings.Settings[key].Value;
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);

                return string.Empty;
            }
        }

        internal static void AddUpdateAppSettings(string key, string value, string file = "")
        {
            try
            {
                Configuration configFile;
                
                string fullPath = $"{OpenALPRQueueMilestoneDefinition.applicationPath}\\{file}";
                //string fullPath = $"{file}";


                if (string.IsNullOrEmpty(fullPath) || string.IsNullOrWhiteSpace(fullPath))
                {
                    configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                else
                {
                    configFile = ConfigurationManager.OpenExeConfiguration(fullPath);
                }

                KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                    settings.Add(key, value);
                else
                    settings[key].Value = value;

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
            }
        }

        internal static T GetObject<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);

            try
            {
                if (typeof(T) == typeof(Guid))
                {
                    Guid guid = new Guid(value);
                    return (T)Convert.ChangeType(guid, typeof(T), CultureInfo.InvariantCulture);
                }

                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Program.Log.Error(null, ex);
                return default(T);
            }
        }

        public static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            SecureString securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}
