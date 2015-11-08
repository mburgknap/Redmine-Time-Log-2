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
            redmine = inClient;
            dbConfig = inDbConfig;
            dbRedmineIssue = inDbRedmineIssue;
        }

        [EventSubscription(BugLog.Events.Load, typeof(Subscribe<BugLog.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.Bugs.Value.Clear();

            foreach (var bug in redmine.GetUserBugs(dbConfig.GetIdUser()))
            {
                bug.Uri = redmine.IssueUrl(bug.Id);
                model.Bugs.Value.Add(bug);
            }

            if (model.Bugs.Value.Count > 0)
                model.Bugs.Update();
        }
    }
}
