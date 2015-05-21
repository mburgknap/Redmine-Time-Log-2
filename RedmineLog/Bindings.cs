using Appccelerate.EventBroker;
using Ninject.Modules;
using NLog;
using RedmineLog.Logic;
using RedmineLog.UI.Views;
using RedmineLog.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog
{
    class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventBroker>().To<EventBroker>().InSingletonScope();
            Bind<Logger>().ToConstant(AppLogger.Log).InSingletonScope();
        }
    }
}
