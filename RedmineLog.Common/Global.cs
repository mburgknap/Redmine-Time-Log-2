namespace RedmineLog.Common
{
    public static class Global
    {
        public static class Events
        {
            public const string Brocker = "GlobalEventsBrocker";
            public const string Info = "topic://Global/Info";
        }
        public interface IView
        {
        }
    }
}