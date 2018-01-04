using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace OpenALPRQueueConsumer.Utility
{
    internal static class Helper
    {
        public static void SetDirectoryNetworkServiceAccessControl(string path)
        {
            if (!Directory.Exists(path))
            {
                Program.Logger.Log.Warn($"Path is not exists: {path}");
                return;
            }

            //WorldSid = "everyone"
            //SID: S - 1 - 5 - 20
            //Name: NT Authority
            //Description: Network Service
            const string sddlForm = "S-1-5-20";
            var sid = string.Empty;

            try
            {
                var dirInfo = new DirectoryInfo(path);
                var dirSecurity = dirInfo.GetAccessControl();
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
                    var networkService = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null); // or new SecurityIdentifier(sddlForm);
                    var networkServiceIdentity = networkService.Translate(typeof(SecurityIdentifier));
                    var accessRule = new FileSystemAccessRule(networkServiceIdentity,
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
                Program.Logger.Log.Error(path, ex);
            }

            if (!SetAcl(path))
                Program.Logger.Log.Warn($"Failed to set acl for path: {path}");
        }

        private static bool SetAcl(string path)
        {
            const string Users = "Users";
            try
            {
                var Rights = FileSystemRights.FullControl;

                // *** Add Access Rule to the actual directory itself
                var AccessRule = new FileSystemAccessRule(Users, Rights,
                                            InheritanceFlags.None,
                                            PropagationFlags.NoPropagateInherit,
                                            AccessControlType.Allow);

                var Info = new DirectoryInfo(path);
                var Security = Info.GetAccessControl(AccessControlSections.Access);

                Security.ModifyAccessRule(AccessControlModification.Set, AccessRule, out bool Result);

                if (!Result)
                    return false;

                // *** Always allow objects to inherit on a directory
                var iFlags = InheritanceFlags.ObjectInherit;
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
                Program.Logger.Log.Error(path, ex);
            }

            return false;
        }

        internal static string ReadConfigKey(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(key, ex);
            }

            return string.Empty;
        }

        internal static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                    settings.Add(key, value);
                else
                    settings[key].Value = value;

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
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
                    var guid = new Guid(value);
                    return (T)Convert.ChangeType(guid, typeof(T), CultureInfo.InvariantCulture);
                }

                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Program.Logger.Log.Error(null, ex);
                return default(T);
            }
        }


    }
}
