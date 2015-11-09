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
        private IRedmineClient redmine;

        [Inject]
        public SubIssueFormLogic(SubIssue.IView inView, SubIssue.IModel inModel, IDbCache inDbCache, IRedmineClient inClient)
        {
            view = inView;
            model = inModel;
            dbCache = inDbCache;
            redmine = inClient;
        }



        [EventSubscription(SubIssue.Events.Load, typeof(Subscribe<SubIssue.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.Priorities.Value.Clear();
            model.Priorities.Value.AddRange(dbCache.GetPriorities());
            model.Priorities.Update();

            model.Trackers.Value.Clear();
            model.Trackers.Value.AddRange(dbCache.GetTrackers());
            model.Trackers.Update();

            model.Users.Value.Clear();
            model.Users.Value.AddRange(dbCache.GetUsers());
            model.Users.Update();
        }

        [EventSubscription(SubIssue.Events.SetSubIssue, typeof(OnPublisher))]
        public void OnSetSubIssueEvent(object sender, Args<int> arg)
        {
            model.SubIssueData.Value.ParentId = arg.Data;
        }


    }
}
