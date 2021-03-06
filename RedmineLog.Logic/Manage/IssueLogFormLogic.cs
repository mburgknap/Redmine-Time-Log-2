﻿using Appccelerate.EventBroker;
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
        public IssueLogFormLogic(IssueLog.IView inView, IssueLog.IModel inModel, IDbIssue inDbIssue, IRedmineClient inClient, IDbRedmineIssue inDbRedmineIssue)
        {
            view = inView;
            model = inModel;
            dbIssue = inDbIssue;
            redmine = inClient;
            dbRedmineIssue = inDbRedmineIssue;
        }

        [EventSubscription(IssueLog.Events.Load, typeof(OnPublisher))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            RedmineIssueData tmp = null;

            model.Issues.Value.Clear();

            foreach (var item in dbIssue.GetList())
            {
                if (item.Id < 0)
                    continue;

                tmp = dbRedmineIssue.Get(item.Id);

                if (tmp.IdParent.HasValue)
                    model.Issues.Value.Add(item, tmp, dbRedmineIssue.Get(tmp.IdParent.Value));
                else
                    model.Issues.Value.Add(item, tmp, null);

                model.Issues.Value.Last().IssueUri = redmine.IssueUrl(item.Id);
            }


            model.Issues.Update();
        }

        [EventSubscription(IssueLog.Events.Select, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<WorkingIssue> arg)
        {
        }
    }
}
