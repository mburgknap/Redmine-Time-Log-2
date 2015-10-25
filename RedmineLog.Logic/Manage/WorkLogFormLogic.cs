using Appccelerate.EventBroker;
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
    internal class WorkLogFormLogic : ILogic<WorkLog.IView>
    {
        private IDbRedmineIssue dbRedmineIssue;
        private IDbConfig dbConfig;
        private WorkLog.IModel model;
        private IRedmineClient redmine;
        private WorkLog.IView view;
        [Inject]
        public WorkLogFormLogic(WorkLog.IView inView, WorkLog.IModel inModel, IRedmineClient inClient, IDbRedmineIssue inDbRedmineIssue, IDbConfig inDbConfig, IEventBroker inEvents)
        {
            view = inView;
            model = inModel;
            redmine = inClient;
            dbRedmineIssue = inDbRedmineIssue;
            dbConfig = inDbConfig;
            inEvents.Register(this);
        }


        [EventSubscription(WorkLog.Events.Load, typeof(Subscribe<WorkLog.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.LoadedTime = DateTime.Now;
            model.WorkLogs.Clear();
            LoadItems();
        }


        [EventSubscription(WorkLog.Events.LoadMore, typeof(Subscribe<WorkLog.IView>))]
        public void OnLoadMoreEvent(object sender, EventArgs arg)
        {
            model.LoadedTime = model.LoadedTime.AddDays(-1);
            LoadItems();
        }
        private void LoadItems()
        {
            RedmineIssueData tmpIssue = null;
            RedmineIssueData tmpParent = null;

            foreach (var workLog in redmine.GetWorkLogs(dbConfig.GetIdUser(), model.LoadedTime))
            {
                tmpIssue = dbRedmineIssue.Get(workLog.IdIssue);

                if (tmpIssue == null)
                {
                    tmpIssue = redmine.GetIssue(workLog.IdIssue);

                    if (tmpIssue != null)
                        dbRedmineIssue.Update(tmpIssue);
                }

                workLog.Issue = tmpIssue != null ? tmpIssue.Subject : workLog.IdIssue.ToString();

                if (tmpIssue != null && tmpIssue.IdParent.HasValue)
                {
                    tmpParent = dbRedmineIssue.Get(tmpIssue.IdParent.Value);

                    if (tmpParent == null)
                    {
                        tmpParent = redmine.GetIssue(tmpIssue.IdParent.Value);

                        if (tmpParent != null)
                            dbRedmineIssue.Update(tmpParent);
                    }

                    workLog.ParentIssue = tmpParent != null ? tmpParent.Subject : tmpIssue.IdParent.Value.ToString();
                }
                else
                    workLog.ParentIssue = "";

                workLog.IssueUri = redmine.IssueUrl(workLog.IdIssue);
                model.WorkLogs.Add(workLog);
            }


            if (model.WorkLogs.Count > 0)
                model.Sync.Value(SyncTarget.View, "WorkLogs");
        }
    }
}
