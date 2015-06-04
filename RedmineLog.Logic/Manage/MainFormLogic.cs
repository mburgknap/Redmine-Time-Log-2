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
    internal class MainFormLogic : ILogic<Main.IView>
    {
        private Main.IView view;
        private Main.IModel model;
        private IRedmineClient redmine;

        [Inject]
        public MainFormLogic(Main.IView inView, Main.IModel inModel, IEventBroker inEvents, IRedmineClient inClient)
        {
            view = inView;
            model = inModel;
            redmine = inClient;
            inEvents.Register(this);
        }

        [EventSubscription(Main.Events.Load, typeof(Subscribe<Main.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
        }
    }
}
