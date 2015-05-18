using RedmineLog.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Model
{
    class SettingsModel : ISettingsModel
    {
        public string Url { get { return App.Context.Config.Url; } set { App.Context.Config.Url = value; } }

        public string ApiKey { get { return App.Context.Config.ApiKey; } set { App.Context.Config.ApiKey = value; } }

        public int IdUser { get { return App.Context.Config.IdUser; } set { App.Context.Config.IdUser = value; } }

    }
}
