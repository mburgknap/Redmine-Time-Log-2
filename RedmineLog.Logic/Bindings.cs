using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Logic.Common;

namespace RedmineLog.Logic
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabase>().To<Database>().InSingletonScope();
            Bind<IDbRedmine>().To<RedmineSetting>().InSingletonScope();
            Bind<IDbConfig>().To<AppConfig>().InSingletonScope();

            Bind<IRedmineClient>().To<RedmineClient>().InSingletonScope();
            Bind<RedmineClient>().ToSelf();

            Bind<ILogic<Settings.IView>>().To<SettingsLogic>().InSingletonScope();
            Bind<SettingsLogic>().ToSelf();
        }
    }
}