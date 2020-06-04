using System.Diagnostics;
using System.Reflection;

namespace Database
{
    public class DatabaseDefinition
    {
        internal static string PlugName = "OpenALPRMilestone";
        internal static string ProductName = "OpenALPR Milestone Plug-in";
        internal static bool IsHostedbySmartClient = true;
        internal static string OpenALPRPluginVersionString = "1.0.0.0";
        internal static float Version = 0;
        internal static FileVersionInfo fileVersion;
        public static string applicationPath;

        static DatabaseDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            Logger.Initialize(name);

#if DEBUG
            assembly = Assembly.GetExecutingAssembly();
            applicationPath = assembly.Location.Replace("\\Database.dll", "");
            if (assembly != null)
                fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
#else
            
            string path = @"C:\Program Files\VideoOS\MIPPlugins\OpenALPR\OpenALPRQueueMilestone.exe";
            applicationPath = $"{assembly.Location.Replace("\\OpenALPRQueueMilestone.exe", "")}\\Service";
            fileVersion = File.Exists(path) ?
                FileVersionInfo.GetVersionInfo(path) :
                FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName);
#endif
        }
    }
}
