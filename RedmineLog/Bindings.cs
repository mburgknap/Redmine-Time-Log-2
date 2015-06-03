using Appccelerate.EventBroker;
using Ninject.Modules;
using NLog;
using RedmineLog.Common;

namespace RedmineLog
{
    internal class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IEventBroker>().To<EventBroker>().InSingletonScope();
            Bind<Logger>().ToConstant(AppLogger.Log).InSingletonScope();
        }
    }
}