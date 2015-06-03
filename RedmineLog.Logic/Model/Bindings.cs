using Ninject.Modules;
using RedmineLog.Common;

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