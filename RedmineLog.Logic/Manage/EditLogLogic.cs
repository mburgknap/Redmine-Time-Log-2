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
    internal class EditLogLogic : ILogic<EditLog.IView>
    {
        private EditLog.IModel model;
        private IRedmineClient redmine;
        private EditLog.IView view;
        [Inject]
        public EditLogLogic(EditLog.IView inView, EditLog.IModel inModel, IRedmineClient inClient, IEventBroker inEvents)
        {
            view = inView;
            model = inModel;
            redmine = inClient;
            inEvents.Register(this);
        }


        [EventSubscription(EditLog.Events.Save, typeof(Subscribe<EditLog.IView>))]
        public void OnSaveEvent(object sender, EventArgs arg)
        {
            model.EditItem.Hours = Math.Round(Convert.ToDecimal(model.Time.Hours) + Convert.ToDecimal(model.Time.Minutes) / 60m, 2, MidpointRounding.ToEven);
            redmine.UpdateLog(model.EditItem);
        }

        [EventSubscription(WorkLog.Events.Edit, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<WorkLogItem> arg)
        {
            LoadData(arg.Data);
        }

        private void LoadData(WorkLogItem workLogItem)
        {
            model.EditItem = workLogItem;
            model.Time = new TimeSpan((int)workLogItem.Hours, (int)((workLogItem.Hours % 1) * 60), 0);

            model.WorkActivities.Clear();
            model.WorkActivities.AddRange(redmine.GetWorkActivityTypes());

            view.Load();
        }
    }
}
