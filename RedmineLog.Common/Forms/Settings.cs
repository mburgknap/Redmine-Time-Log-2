using System.Text;
namespace RedmineLog.Common
{
    public static class Settings
    {
        public static class Events
        {
            public const string Load = "topic://Settings/Load";
            public const string Save = "topic://Settings/Connect";
            public const string ReloadCache = "topic://Settings/ReloadCache";
            public const string ImportDb = "topic://Settings/ImportDb";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            DataProperty<StringBuilder> Url { get; }

            DataProperty<StringBuilder> ApiKey { get; }

            DataProperty<DisplayData> Display { get; }

            DataProperty<TimerType> Timer { get; }

            DataProperty<int> WorkDayHours { get; }

            DataProperty<StringBuilder> DbPath { get; }
        }
    }
}