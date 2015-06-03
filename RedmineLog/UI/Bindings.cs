using Ninject.Modules;
using RedmineLog.Common;

namespace RedmineLog.UI
{
    internal class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<Settings.IView>().To<SettingsView>().InSingletonScope();
            Bind<SettingsView>().ToSelf();
        }
    }
}