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
    internal class EditLogFormLogic : ILogic<EditLog.IView>
    {
        private EditLog.IModel model;
        private IRedmineClient redmine;
        private EditLog.IView view;
        private IDbCache dbCache;
        [Inject]
        public EditLogFormLogic(EditLog.IView inView, EditLog.IModel inModel, IRedmineClient inClient, IDbCache inDbCache)
        {
            view = inView;
            model = inModel;
            redmine = inClient;
            dbCache = inDbCache;
        }


        [EventSubscription(EditLog.Events.Save, typeof(OnPublisher))]
        public void OnSaveEvent(object sender, EventArgs arg)
        {
            model.EditItem.Value.Hours = Math.Round(Convert.ToDecimal(model.Time.Value.Hours) + (Convert.ToDecimal(model.Time.Value.Minutes) / 60m), 2, MidpointRounding.AwayFromZero);
            redmine.UpdateLog(model.EditItem.Value);
        }

        [EventSubscription(WorkLog.Events.Edit, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<WorkLogItem> arg)
        {
            LoadData(arg.Data);
        }

        [EventSubscription(EditLog.Events.Load, typeof(OnPublisher))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
        }

        private void LoadData(WorkLogItem workLogItem)
        {
            model.EditItem.Update(workLogItem);
            model.Time.Update(new TimeSpan((int)workLogItem.Hours, (int)Math.Round((workLogItem.Hours % 1) * 60, MidpointRounding.AwayFromZero), 0));


            model.WorkActivities.Value.Clear();
            model.WorkActivities.Value.AddRange(dbCache.GetWorkActivityTypes());

            var item = model.WorkActivities.Value.Where(x => x.Id == model.EditItem.Value.IdActivity).FirstOrDefault();

            if (item != null)
                model.Activity.Invoke(item, ActionType.Set);

            model.WorkActivities.Update();
        }
    }
}
