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
            Url = new DataProperty<StringBuilder>();
        }

        public DataProperty<StringBuilder> ApiKey { get; private set; }

        public DataProperty<DisplayData> Display { get; private set; }

        public DataProperty<StringBuilder> Url { get; private set; }
    }
}