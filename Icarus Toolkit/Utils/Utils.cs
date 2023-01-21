using Serilog;
using System.Configuration;
using System;

namespace Icarus_Toolkit.Utils
{
    internal class Utils
    {
        private static bool isLogInit = false;
        public const string LogPath = "logs\\icarus_toolkit.log";

        internal static void InitLog()
        {
            if (isLogInit) { return; }
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(LogPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10, shared: true)
                .CreateLogger();
            isLogInit = true;
            Log.Information("");
            Log.Information("LOG INIT");
            Log.Information("");
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ce)
            {
                Log.Error(ce.Message);
            }
        }
    }
}
