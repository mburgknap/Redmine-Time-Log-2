using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    internal class IssueLogFormLogic : ILogic<IssueLog.IView>
    {
        private IDbIssue dbIssue;
        private IDbRedmineIssue dbRedmineIssue;
        private IssueLog.IModel model;
        private IssueLog.IView view;
        private IRedmineClient redmine;

        [Inject]
        public IssueLogFormLogic(IssueLog.IView inView, IssueLog.IModel inModel, IEventBroker inEvents, IDbIssue inDbIssue, IRedmineClient inClient, IDbRedmineIssue inDbRedmineIssue)
        {
            view = inView;
            model = inModel;
            model.Sync.Bind(SyncTarget.Source, this);
            dbIssue = inDbIssue;
            redmine = inClient;
            dbRedmineIssue = inDbRedmineIssue;
            inEvents.Register(this);
        }

        [EventSubscription(IssueLog.Events.Load, typeof(Subscribe<IssueLog.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            RedmineIssueData tmp = null;

            model.Issues.Clear();

            foreach (var item in dbIssue.GetList())
            {
                if (item.Id < 0)
                    continue;

                tmp = dbRedmineIssue.Get(item.Id);

                if (tmp.IdParent.HasValue)
                    model.Issues.Add(item, tmp, dbRedmineIssue.Get(tmp.IdParent.Value));
                else
                    model.Issues.Add(item, tmp, null);

                model.Issues[model.Issues.Count - 1].IssueUri = redmine.IssueUrl(item.Id);
            }


            model.Sync.Value(SyncTarget.View, "Issues");
        }

        [EventSubscription(IssueLog.Events.Select, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<WorkingIssue> arg)
        {
        }
    }
}
