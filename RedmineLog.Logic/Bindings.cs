using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.Logic.Common;
using RedmineLog.Logic.Manage;

namespace RedmineLog.Logic
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {

            Bind<IRedmineClient>().To<RedmineClient>().InSingletonScope();

            Bind<ILogic<SubIssue.IView>>().To<SubIssueFormLogic>().InSingletonScope();
            Bind<SubIssueFormLogic>().ToSelf();

            Bind<ILogic<Settings.IView>>().To<SettingFormLogic>().InSingletonScope();
            Bind<SettingFormLogic>().ToSelf();

            Bind<ILogic<Main.IView>>().To<MainFormLogic>().InSingletonScope();
            Bind<MainFormLogic>().ToSelf();

            Bind<ILogic<Small.IView>>().To<SmallFormLogic>().InSingletonScope();
            Bind<SmallFormLogic>().ToSelf();

            Bind<ILogic<IssueLog.IView>>().To<IssueLogFormLogic>().InSingletonScope();
            Bind<IssueLogFormLogic>().ToSelf();

            Bind<ILogic<BugLog.IView>>().To<BugLogFormLogic>().InSingletonScope();
            Bind<BugLogFormLogic>().ToSelf();

            Bind<ILogic<WorkLog.IView>>().To<WorkLogFormLogic>().InSingletonScope();
            Bind<WorkLogFormLogic>().ToSelf();

            Bind<ILogic<EditLog.IView>>().To<EditLogFormLogic>().InSingletonScope();
            Bind<EditLogFormLogic>().ToSelf();
        }
    }
}