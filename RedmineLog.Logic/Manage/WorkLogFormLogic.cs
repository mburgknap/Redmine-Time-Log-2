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
        private WorkLog.IView view;
        private WorkLog.IModel model;
        private IRedmineClient redmine;
        private IDbConfig dbConfig;

        [Inject]
        public WorkLogFormLogic(WorkLog.IView inView, WorkLog.IModel inModel, IRedmineClient inClient, IDbConfig inDbConfig, IEventBroker inEvents)
        {
            view = inView;
            model = inModel;
            redmine = inClient;
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
            model.WorkLogs.AddRange(redmine.GetWorkLogs(dbConfig.GetIdUser(), model.LoadedTime));
            if (model.WorkLogs.Count > 0)
                model.Sync.Value(SyncTarget.View, "WorkLogs");
        }
    }
}
