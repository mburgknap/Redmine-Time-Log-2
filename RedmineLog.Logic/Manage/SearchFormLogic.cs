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
    internal class SearchFormLogic : ILogic<Search.IView>
    {
        private IDbIssue dbIssue;
        private IDbRedmineIssue dbRedmineIssue;
        private Search.IModel model;
        private Search.IView view;
        private IRedmineClient redmine;
        private IDbConfig dbConfig;

        [Inject]
        public SearchFormLogic(Search.IView inView, Search.IModel inModel, IDbIssue inDbIssue, IDbConfig inDbConfig, IRedmineClient inClient, IDbRedmineIssue inDbRedmineIssue)
        {
            view = inView;
            model = inModel;
            dbIssue = inDbIssue;
            dbConfig = inDbConfig;
            redmine = inClient;
            dbRedmineIssue = inDbRedmineIssue;
        }

        [EventSubscription(Search.Events.Clear, typeof(OnPublisher))]
        public void OnClearEvent(object sender, EventArgs arg)
        {
            model.Issues.Value.Clear();
            model.Issues.Update();
        }

        [EventSubscription(Search.Events.Load, typeof(OnPublisher))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.Issues.Value.Clear();
        }


        [EventSubscription(Search.Events.Search, typeof(OnPublisher))]
        public void OnSearchEvent(object sender, Args<string> arg)
        {
            foreach (var issue in redmine.Search(dbConfig.GetIdUser(), arg.Data))
            {
                if (!model.Issues.Value.Where(x => x.Issue.Id == issue.Issue.Id).Any())
                {
                    issue.IssueUri = redmine.IssueUrl(issue.Issue.Id);
                    model.Issues.Value.Add(issue);
                }
            }

            model.Issues.Update();
        }
    }
}
