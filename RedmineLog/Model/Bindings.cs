using Ninject.Modules;
using RedmineLog.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Model
{
    class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettingsModel>().To<SettingsModel>().InSingletonScope();
        }
    }
}
