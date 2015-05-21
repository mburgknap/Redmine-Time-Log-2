using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogic<Settings.IView>>().To<SettingsLogic>().InSingletonScope();
            Bind<SettingsLogic>().ToSelf();

        }
    }
}
