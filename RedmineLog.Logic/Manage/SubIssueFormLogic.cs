using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
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
    internal class SubIssueFormLogic : ILogic<SubIssue.IView>
    {

        private SubIssue.IModel model;
        private SubIssue.IView view;
        private IDbCache dbCache;

        [Inject]
        public SubIssueFormLogic(SubIssue.IView inView, SubIssue.IModel inModel, IDbCache inDbCache, IEventBroker inEvents)
        {
            view = inView;
            model = inModel;
            dbCache = inDbCache;
            inEvents.Register(this);
        }



        [EventSubscription(SubIssue.Events.Load, typeof(Subscribe<SubIssue.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.Priorities.Clear();
            model.Priorities.AddRange(dbCache.GetPriorities());
            model.Sync.Value(SyncTarget.View, "Priorities");

            model.Trackers.Clear();
            model.Trackers.AddRange(dbCache.GetTrackers());
            model.Sync.Value(SyncTarget.View, "Trackers");

            model.Users.Clear();
            model.Users.AddRange(dbCache.GetUsers());
            model.Sync.Value(SyncTarget.View, "Users");
        }

        [EventSubscription(SubIssue.Events.SetSubIssue, typeof(OnPublisher))]
        public void OnSetSubIssueEvent(object sender, Args<RedmineIssueData> arg)
        {
            model.ParentId = arg.Data.Id;
        }

    }
}
