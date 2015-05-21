using NLog;

namespace RedmineLog.Utils
{
    public static class AppLogger
    {
        public static Logger Log { get; private set; }

        static AppLogger()
        {
            Log = LogManager.GetCurrentClassLogger();
        }
    }
}