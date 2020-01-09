using System.Diagnostics;
using System.Reflection;
using OpenALPRQueueConsumer.Utility;

namespace OpenALPRQueueConsumer
{
    public class OpenALPRQueueMilestoneDefinition
    {
        internal static string PlugName = "OpenALPRMilestone";
        internal static string ProductName = "OpenALPR Milestone Plug-in";
        internal static bool IsHostedbySmartClient = true;
        internal static string OpenALPRPluginVersionString = "1.0.0.0";
        internal static float Version = 0;
        internal static FileVersionInfo fileVersion;


        static OpenALPRQueueMilestoneDefinition()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            Logger.Initialize(name);

#if DEBUG
            assembly = Assembly.GetExecutingAssembly();
            if (assembly != null)
                fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
#else
            
            var path = @"C:\Program Files\VideoOS\MIPPlugins\OpenALPR\OpenALPRQueueMilestone.exe";

            fileVersion = File.Exists(path) ?
                FileVersionInfo.GetVersionInfo(path) :
                FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName);
#endif
        }
    }
}
