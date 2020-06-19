using System.Diagnostics;
using System.Reflection;
using OpenALPR.SystemTrayApp.Utility;

namespace OpenALPR.SystemTrayApp
{
    public class OpenALPRQueueMilestoneDefinition
    {
        internal static string PlugName = "SystemTrayApp";
        internal static string ProductName = "OpenALPR System Tray Application";
        internal static bool IsHostedbySmartClient = true;
        internal static string OpenALPRPluginVersionString = "1.0.0.0";
        internal static float Version = 0;
        internal static FileVersionInfo fileVersion;
        internal static string applicationPath;


        static OpenALPRQueueMilestoneDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            Logger.Initialize(name);

#if DEBUG
            assembly = Assembly.GetExecutingAssembly();
            applicationPath = assembly.Location.Replace("\\OpenALPR.SystemTrayApp.exe", "");
            if (assembly != null)
                fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
#else
            
            string path = @"C:\Program Files\VideoOS\MIPPlugins\OpenALPR\OpenALPR.SystemTrayApp.exe";
            //applicationPath = @"C:\Program Files\VideoOS\MIPPlugins\OpenALPR\Service";
            applicationPath = $"{assembly.Location.Replace("\\OpenALPR.SystemTrayApp.exe", "")}\\Service";
            fileVersion = System.IO.File.Exists(path) ?
                FileVersionInfo.GetVersionInfo(path) :
                FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName);
#endif
        }
    }
}
