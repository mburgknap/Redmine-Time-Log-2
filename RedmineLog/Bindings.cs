using Appccelerate.EventBroker;
using Ninject.Modules;
using NLog;
using RedmineLog.Common;
using RedmineLog.Logic.Common;

namespace RedmineLog
{
    internal class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IAppSettings>().To<AppSettings>().InSingletonScope();
            Bind<IEventBroker>().To<EventBroker>().InSingletonScope();
            Bind<Logger>().ToConstant(AppLogger.Log).InSingletonScope();
        }
    }
}