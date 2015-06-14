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
    internal class SearchFormLogic : ILogic<Search.IView>
    {
        private Search.IView view;
        private Search.IModel model;
        private IDbIssue dbIssue;
        private IDbRedmineIssue dbRedmineIssue;

        [Inject]
        public SearchFormLogic(Search.IView inView, Search.IModel inModel, IEventBroker inEvents, IDbIssue inDbIssue, IDbRedmineIssue inDbRedmineIssue)
        {
            view = inView;
            model = inModel;
            model.Sync.Bind(SyncTarget.Source, this);
            dbIssue = inDbIssue;
            dbRedmineIssue = inDbRedmineIssue;
            inEvents.Register(this);
        }

        [EventSubscription(Search.Events.Load, typeof(Subscribe<Search.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            RedmineIssueData tmp = null;

            model.Issues.Clear();

            foreach (var item in dbIssue.GetList())
            {
                tmp = dbRedmineIssue.Get(item.Id);

                if (tmp.IdParent.HasValue)
                    model.Issues.Add(item, tmp, dbRedmineIssue.Get(tmp.IdParent.Value));
                else
                    model.Issues.Add(item, tmp, null);
            }

            model.Sync.Value(SyncTarget.View, "Issues");
        }

        [EventSubscription(Search.Events.Select, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<WorkingIssue> arg)
        {
        }
    }
}
