﻿using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Logic.Model;

namespace RedmineLog.Model
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<Settings.IModel>().To<SettingsModel>().InSingletonScope();
            Bind<EditLogTime.IModel>().To<EditLogTimeModel>().InSingletonScope();
            Bind<Main.IModel>().To<MainModel>().InSingletonScope();
            Bind<Search.IModel>().To<SearchModel>().InSingletonScope();
            Bind<Small.IModel>().To<SmallModel>().InSingletonScope();
            Bind<WorkLog.IModel>().To<WorkLogModel>().InSingletonScope();
        }
    }
}