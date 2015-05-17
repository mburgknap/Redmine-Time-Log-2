using Ninject.Modules;
using RedmineLog.UI.Common;
using RedmineLog.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.UI
{
    class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettingsView>().To<SettingsView>().InSingletonScope();
            Bind<SettingsView>().ToSelf();
        }
    }
}
