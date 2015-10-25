using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic;

namespace RedmineLog.Model
{
    internal class SettingsModel : Settings.IModel
    {
        [Inject]
        public SettingsModel()
        {
            Sync = new ModelSync<Settings.IModel>();
        }

        public string Url { get; set; }

        public string ApiKey { get; set; }

        public int IdUser { get; set; }

        public DisplayData Display { get; set; }

        public IModelSync Sync { get; private set; }
    }
}