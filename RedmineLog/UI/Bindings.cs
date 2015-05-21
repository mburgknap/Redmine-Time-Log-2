using Ninject.Modules;
using RedmineLog.Common;
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
            Bind<Settings.IView>().To<SettingsView>().InSingletonScope();
            Bind<SettingsView>().ToSelf();
        }
    }
}
