using System;
using System.Configuration;
using SystemTrayApp;

namespace OpenALPR.SystemTrayApp.Utility
{
    internal static class Common
    {
        internal static string ReadConfigKey(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                Program.Log.Error("ReadConfigKey", ex);
            }

            return string.Empty;
        }

    }
}
