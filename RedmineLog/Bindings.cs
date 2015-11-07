using Appccelerate.EventBroker;
using Ninject.Modules;
using NLog;
using RedmineLog.Common;
using Ninject.Extensions.AppccelerateEventBroker;
using RedmineLog.Logic.Common;
using RedmineLog.UI;

namespace RedmineLog
{
    internal class Bindings : NinjectModule
    {
        public override void Load()
        {
            Kernel.AddGlobalEventBroker(Global.Events.Brocker);
            Bind<IAppSettings>().To<AppSettings>().InSingletonScope();
            Bind<Logger>().ToConstant(AppLogger.Log).InSingletonScope();
            Bind<AppTime.IClock>().To<AppTimer>().InSingletonScope().RegisterOnGlobalEventBroker();
        }
    }
}