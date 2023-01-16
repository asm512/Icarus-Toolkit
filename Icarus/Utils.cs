using Serilog;

namespace Icarus
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
        }
    }
}
