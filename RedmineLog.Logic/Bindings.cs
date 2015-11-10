using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.Logic.Common;
using RedmineLog.Logic.Manage;
using Ninject.Extensions.AppccelerateEventBroker;

namespace RedmineLog.Logic
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IRedmineClient>().To<RedmineClient>().InSingletonScope();

            Bind<ILogic<SubIssue.IView>>().To<SubIssueFormLogic>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<SubIssueFormLogic>().ToSelf();

            Bind<ILogic<Settings.IView>>().To<SettingFormLogic>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<SettingFormLogic>().ToSelf();

            Bind<ILogic<Main.IView>>().To<MainFormLogic>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<MainFormLogic>().ToSelf();

            Bind<ILogic<Small.IView>>().To<SmallFormLogic>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<SmallFormLogic>().ToSelf();

            Bind<ILogic<IssueLog.IView>>().To<IssueLogFormLogic>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<IssueLogFormLogic>().ToSelf();

            Bind<ILogic<BugLog.IView>>().To<BugLogFormLogic>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<BugLogFormLogic>().ToSelf();

            Bind<ILogic<WorkLog.IView>>().To<WorkLogFormLogic>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<WorkLogFormLogic>().ToSelf();

            Bind<ILogic<EditLog.IView>>().To<EditLogFormLogic>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<EditLogFormLogic>().ToSelf();
        }
    }
}