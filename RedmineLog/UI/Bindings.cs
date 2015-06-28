﻿using Ninject.Modules;
using RedmineLog.Common;

namespace RedmineLog.UI
{
    internal class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<Settings.IView>().To<SettingsView>().InSingletonScope();
            Bind<SettingsView>().ToSelf();

            Bind<EditLog.IView>().To<EditTimeLogView>().InSingletonScope();
            Bind<EditTimeLogView>().ToSelf();

            Bind<Main.IView>().To<MainView>().InSingletonScope();
            Bind<MainView>().ToSelf();

            Bind<IssueLog.IView>().To<IssueLogView>().InSingletonScope();
            Bind<IssueLogView>().ToSelf();

            Bind<Small.IView>().To<SmallView>().InSingletonScope();
            Bind<SmallView>().ToSelf();

            Bind<WorkLog.IView>().To<WorkLogView>().InSingletonScope();
            Bind<WorkLogView>().ToSelf();
        }
    }
}