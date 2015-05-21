using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Model
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<Settings.IModel>().To<SettingsModel>().InSingletonScope();
        }
    }
}
