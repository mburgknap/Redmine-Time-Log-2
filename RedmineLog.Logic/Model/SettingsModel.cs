using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic;
using System.Text;

namespace RedmineLog.Model
{
    internal class SettingsModel : Settings.IModel
    {
        [Inject]
        public SettingsModel()
        {
            ApiKey = new DataProperty<StringBuilder>();
            Display = new DataProperty<DisplayData>();
            Timer = new DataProperty<TimerType>();
            Url = new DataProperty<StringBuilder>();
            WorkDayHours = new DataProperty<int>();
            DbPath = new DataProperty<StringBuilder>();
        }

        public DataProperty<StringBuilder> ApiKey { get; private set; }

        public DataProperty<DisplayData> Display { get; private set; }

        public DataProperty<TimerType> Timer { get; private set; }

        public DataProperty<StringBuilder> Url { get; private set; }

        public DataProperty<int> WorkDayHours { get; private set; }

        public DataProperty<StringBuilder> DbPath { get; private set; }
    }
}