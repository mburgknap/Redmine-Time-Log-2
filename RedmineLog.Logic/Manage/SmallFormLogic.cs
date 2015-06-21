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
    internal class SmallFormLogic : ILogic<Small.IView>
    {
        private Small.IView view;
        private Small.IModel model;
        [Inject]
        public SmallFormLogic(Small.IView inView, Small.IModel inModel, IEventBroker inEvents)
        {
            view = inView;
            model = inModel;
            inEvents.Register(this);
        }


        [EventSubscription(Small.Events.Load, typeof(Subscribe<Small.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.Sync.Value(SyncTarget.View, "IssueInfo");
            model.Sync.Value(SyncTarget.View, "IssueParentInfo");
        }
    }
}
