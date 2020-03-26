using log4net;
using OpenALPR.SystemTrayIcon.Utility;
using System;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace SystemTrayApp
{
    static class Program
    {
        internal static string CompanyName = "OpenALPR";
        internal static string ProductName = "OpenALPR";
        internal static string Url = "http://OpenALPR.com/";
        internal static ILog Log;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logging SystemTrayIconLogger = new Logging("SystemTrayApp");
            if (SystemTrayIconLogger != null)
                Console.WriteLine($"Log: {SystemTrayIconLogger.LogPath}");

            Log = SystemTrayIconLogger.Log;

            // Use the assembly GUID as the name of the mutex which we use to detect if an application instance is already running
            bool createdNew = false;
            string mutexName = Assembly.GetExecutingAssembly().GetType().GUID.ToString();
            using (Mutex mutex = new Mutex(false, mutexName, out createdNew))
            {
                if (!createdNew)
                {
                    // Only allow one instance
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                STAApplicationContext applicationContext = new STAApplicationContext();
                try
                {
                    Application.Run(applicationContext);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    Program.Log.Error("Main", ex);
                }
            }

            if (SystemTrayIconLogger != null)
                SystemTrayIconLogger.Close();
        }

        private static void CreateFolders()
        {
            string mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyName);

            try
            {
                if (!Directory.Exists(mainPath))
                {
                    Directory.CreateDirectory(mainPath);
                    SetDirectoryNetworkServiceAccessControl(mainPath);
                }
            }
            catch (Exception ex)
            {
                Log.Error("CreateFolders - 1", ex);
            }

            mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyName, ProductName);

            try
            {
                if (!Directory.Exists(mainPath))
                {
                    Directory.CreateDirectory(mainPath);
                    SetDirectoryNetworkServiceAccessControl(mainPath);
                }
            }
            catch (Exception ex)
            {
                Log.Error("CreateFolders - 2", ex);
            }

            try
            {
                string path = Path.Combine(mainPath, "Log");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    SetDirectoryNetworkServiceAccessControl(path);
                }
            }
            catch (Exception ex)
            {
                Log.Error("CreateFolders - 3", ex);
            }

            try
            {
                string path = Path.Combine(mainPath, "Jobs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    SetDirectoryNetworkServiceAccessControl(path);
                }
            }
            catch (Exception ex)
            {
                Log.Error("CreateFolders - 4", ex);
            }
        }

        private static void SetDirectoryNetworkServiceAccessControl(string path)
        {
            if (!Directory.Exists(path))
            {
                Log.Warn($"Path does not exists: {path}");
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
                    SecurityIdentifier networkService = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
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
                Log.Error("SetDirectoryNetworkServiceAccessControl", ex);
            }

            if (!SetAcl(path))
                Log.Warn($"Failed to set acl for path: {path}");
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
                Log.Error("SetAcl", ex);
            }

            return false;
        }
    }
}