using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.Logic.Model;
using Ninject.Extensions.AppccelerateEventBroker;

namespace RedmineLog.Model
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<SubIssue.IModel>().To<SubIssueModel>().InSingletonScope();
            Bind<Settings.IModel>().To<SettingsModel>().InSingletonScope();
            Bind<EditLog.IModel>().To<EditLogModel>().InSingletonScope();
            Bind<Main.IModel>().To<MainModel>().InSingletonScope();
            Bind<IssueLog.IModel>().To<IssueLogModel>().InSingletonScope();
            Bind<Small.IModel>().To<SmallModel>().InSingletonScope();
            Bind<WorkLog.IModel>().To<WorkLogModel>().InSingletonScope();
            Bind<BugLog.IModel>().To<BugLogModel>().InSingletonScope();
            Bind<Search.IModel>().To<SearchModel>().InSingletonScope();
        }
    }
}