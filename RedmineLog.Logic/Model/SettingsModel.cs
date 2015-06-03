using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic;

namespace RedmineLog.Model
{
    internal class SettingsModel : Settings.IModel
    {
        private IDbRedmine dbRedmine;
        private IDbConfig dbConfig;

        [Inject]
        public SettingsModel(IDbRedmine inDbRedmine, IDbConfig inDbConfig)
        {
            dbRedmine = inDbRedmine;
            dbConfig = inDbConfig;
        }

        public string Url { get { return dbRedmine.GetUrl(); } set { dbRedmine.SetUrl(value); } }

        public string ApiKey { get { return dbRedmine.GetApiKey(); } set { dbRedmine.SetApiKey(value); } }

        public int IdUser { get { return dbConfig.GetIdUser(); } set { dbConfig.SetIdUser(value); } }
    }
}