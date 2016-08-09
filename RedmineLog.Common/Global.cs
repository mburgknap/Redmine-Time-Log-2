namespace RedmineLog.Common
{
    public static class Global
    {
        public const string Brocker = "GlobalEventsBrocker";
        
        public static class Events
        {
            public const string Info = "topic://Global/Info";
            public const string Restart = "topic://Global/Restart";
        }
        public interface IView
        {
        }
    }
}