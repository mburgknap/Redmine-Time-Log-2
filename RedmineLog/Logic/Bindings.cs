using Ninject.Modules;
using RedmineLog.Logic.Common;
using RedmineLog.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogic<ISettingsView>>().To<SettingsLogic>().InSingletonScope();
            Bind<SettingsLogic>().ToSelf();

        }
    }
}
