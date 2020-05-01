// Copyright OpenALPR Technology, Inc. 2018

using OpenALPRPlugin.Client;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using VideoOS.Platform;

namespace OpenALPRPlugin.Utility
{
    internal static class Helper
    {
        public static string datePatt = @"d/m/yyyy hh:mm:ss tt";
        public static void Display(string title, DateTime inputDt)
        {
            DateTime dispDt = inputDt;
            string dtString;

            dtString = dispDt.ToString(datePatt);
            Debug.WriteLine("{0} {1}, Kind = {2}", title, dtString, dispDt.Kind);
        }

        public static BookmarkDescription ParseBookmarkDescription(string description)
        {
            BookmarkDescription bookmarkDescription = new BookmarkDescription();

            string[] items = description.Split(';');

            bookmarkDescription.Make = items[0].Split('=').LastOrDefault();
            bookmarkDescription.BodyType = items[1].Split('=').LastOrDefault();
            bookmarkDescription.Color = items[2].Split('=').LastOrDefault();
            bookmarkDescription.BestRegion = items[3].Split('=').LastOrDefault();
            bookmarkDescription.Candidates = items[4].Split('=').LastOrDefault();
            bookmarkDescription.TravelDirection = Convert.ToDouble(items[5].Split('=').LastOrDefault());
            bookmarkDescription.PlateNumber = items[6].Split('=').LastOrDefault();
            bookmarkDescription.Coordinates = items[7].Replace("Coordinates=", "");
            bookmarkDescription.Timestamp = Convert.ToDateTime(items[8].Split('=').LastOrDefault());

            return bookmarkDescription;
        }

        public static void SetDirectoryNetworkServiceAccessControl(string path)
        {
            if (!Directory.Exists(path))
            {
                Logger.Log.Warn($"Path is not exists: {path}");
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
                Logger.Log.Error(path, ex);
            }

            if (!SetAcl(path))
                Logger.Log.Warn($"Failed to set acl for path: {path}");
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
                Logger.Log.Error(path, ex);
            }

            return false;
        }

        internal static void UIThread(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
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
                Logger.Log.Error(null, ex);
                return default(T);
            }
        }


    }
}
