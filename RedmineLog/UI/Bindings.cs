using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using Ninject.Extensions.AppccelerateEventBroker;
using RedmineLog.UI.Items;

namespace RedmineLog.UI
{
    internal class Bindings : NinjectModule
    {
        public override void Load()
        {

            Bind<SubIssue.IView>().To<SubIssueView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<SubIssueView>().ToSelf();

            Bind<Settings.IView>().To<SettingsView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<SettingsView>().ToSelf();

            Bind<EditLog.IView>().To<EditTimeLogView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<EditTimeLogView>().ToSelf();

            Bind<Main.IView>().To<MainView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<MainView>().ToSelf();

            Bind<IssueLog.IView>().To<IssueLogView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<IssueLogView>().ToSelf();

            Bind<BugLog.IView>().To<BugLogView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<BugLogView>().ToSelf();

            Bind<Small.IView>().To<SmallView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<SmallView>().ToSelf();

            Bind<WorkLog.IView>().To<WorkLogView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<WorkLogView>().ToSelf();

            Bind<Search.IView>().To<SearchView>().InSingletonScope().RegisterOnGlobalEventBroker();
            Bind<SearchView>().ToSelf();

            Bind<IssueLogItemView>().To<IssueLogItemView>().RegisterOnGlobalEventBroker();
            Bind<BugLogItemView>().To<BugLogItemView>().RegisterOnGlobalEventBroker();
            Bind<IssueItemView>().To<IssueItemView>().RegisterOnGlobalEventBroker();
            Bind<WorkLogItemView>().To<WorkLogItemView>().RegisterOnGlobalEventBroker();
            Bind<SearchItemView>().To<SearchItemView>().RegisterOnGlobalEventBroker();

        }
    }
}