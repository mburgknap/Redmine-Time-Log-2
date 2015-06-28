using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Logic.Common;

namespace RedmineLog.Logic
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {

            Bind<IRedmineClient>().To<RedmineClient>().InSingletonScope();

            Bind<ILogic<Settings.IView>>().To<SettingFormLogic>().InSingletonScope();
            Bind<SettingFormLogic>().ToSelf();

            Bind<ILogic<Main.IView>>().To<MainFormLogic>().InSingletonScope();
            Bind<MainFormLogic>().ToSelf();

            Bind<ILogic<Small.IView>>().To<SmallFormLogic>().InSingletonScope();
            Bind<SmallFormLogic>().ToSelf();

            Bind<ILogic<IssueLog.IView>>().To<SearchFormLogic>().InSingletonScope();
            Bind<SearchFormLogic>().ToSelf();

            Bind<ILogic<WorkLog.IView>>().To<WorkLogFormLogic>().InSingletonScope();
            Bind<WorkLogFormLogic>().ToSelf();

            Bind<ILogic<EditLog.IView>>().To<EditLogLogic>().InSingletonScope();
            Bind<EditLogLogic>().ToSelf();
        }
    }
}