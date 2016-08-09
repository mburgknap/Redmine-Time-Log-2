using Appccelerate.EventBroker;
using Ninject.Modules;
using NLog;
using RedmineLog.Common;
using Ninject.Extensions.AppccelerateEventBroker;
using RedmineLog.Logic.Common;
using RedmineLog.UI;
using RedmineLog.Common.Interfaces;
using RedmineLog.Utils;

namespace RedmineLog
{
    internal class Bindings : NinjectModule
    {
        public override void Load()
        {
            Kernel.AddGlobalEventBroker(Global.Brocker);
            Bind<IAppSettings>().To<AppSettings>().InSingletonScope();
            Bind<ILog>().To<AppLog>().InSingletonScope();
            Bind<IUpdater>().To<AppUpdater>().InSingletonScope();
            Bind<WebRedmine>().To<WebRedmine>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<AppTime.IClock>().To<AppTimer>().InSingletonScope().RegisterOnGlobalEventBroker();
        }
    }
}