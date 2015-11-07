using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Manage
{
    internal class BugLogFormLogic : ILogic<BugLog.IView>
    {
        private BugLog.IView view;
        private BugLog.IModel model;
        private IDbRedmineIssue dbRedmineIssue;
        private IRedmineClient redmine;
        private IDbConfig dbConfig;

        [Inject]
        public BugLogFormLogic(BugLog.IView inView, BugLog.IModel inModel, IRedmineClient inClient, IDbConfig inDbConfig, IDbRedmineIssue inDbRedmineIssue)
        {
            view = inView;
            model = inModel;
            model.Sync.Bind(SyncTarget.Source, this);
            redmine = inClient;
            dbConfig = inDbConfig;
            dbRedmineIssue = inDbRedmineIssue;
        }

        [EventSubscription(BugLog.Events.Load, typeof(Subscribe<BugLog.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.Bugs.Clear();

            foreach (var bug in redmine.GetUserBugs(dbConfig.GetIdUser()))
            {
                bug.Uri = redmine.IssueUrl(bug.Id);
                model.Bugs.Add(bug);
            }

            if (model.Bugs.Count > 0)
                model.Sync.Value(SyncTarget.View, "Bugs");
        }
    }
}
