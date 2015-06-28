using Appccelerate.EventBroker;
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
    internal class BugLogFormLogic : ILogic<BugLog.IView>
    {
        private BugLog.IView view;
        private BugLog.IModel model;
        private IDbRedmineIssue dbRedmineIssue;
        
        [Inject]
        public BugLogFormLogic(BugLog.IView inView, BugLog.IModel inModel, IEventBroker inEvents, IDbRedmineIssue inDbRedmineIssue)
        {
            view = inView;
            model = inModel;
            model.Sync.Bind(SyncTarget.Source, this);
            dbRedmineIssue = inDbRedmineIssue;
            inEvents.Register(this);
        }

        [EventSubscription(BugLog.Events.Load, typeof(Subscribe<BugLog.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        { }
    }
}
