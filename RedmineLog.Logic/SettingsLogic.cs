using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using Ninject.Modules;
using Redmine.Net.Api;
using RedmineLog.Common;
using RedmineLog.Logic.Common;
using RedmineLog.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    internal class SettingsLogic : ILogic<Settings.IView>
    {
        [Inject]
        public SettingsLogic(Settings.IView inView, Settings.IModel inModel, IEventBroker inEvents)
        {
            View = inView;
            Model = inModel;
            inEvents.Register(this);
            inEvents.Register(new Test());
        }

        private Settings.IView View;

        private Settings.IModel Model;

        class Test
        {
            [EventSubscription(Settings.Load, typeof(Subscribe<Settings.IView>))]
            public void OnLoadEvent(object sender, EventArgs arg)
            {
                Console.WriteLine("OnLoadEvent " + this.GetType().Name);
            }

            [EventSubscription(Settings.Connect, typeof(Subscribe<Settings.IView>))]
            public void OnConnectEvent(object sender, EventArgs arg)
            {
                Console.WriteLine("OnConnectEvent " + this.GetType().Name);
            }
        }


        public void Apply(string inCmd)
        {

        }

        [EventSubscription("topic://Load", typeof(Subscribe<Settings.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            System.Diagnostics.Debug.WriteLine("View " + View.GetType().Name);
            System.Diagnostics.Debug.WriteLine("Model " + Model.GetType().Name);
        }

        [EventSubscription("topic://Connect", typeof(Subscribe<Settings.IView>))]
        public void OnConnectEvent(object sender, EventArgs arg)
        {
            var parameters = new NameValueCollection { };
            var manager = new RedmineManager(Model.Url, Model.ApiKey);
            var user = manager.GetCurrentUser(parameters);
            Model.IdUser = user.Id;
            App.Context.Config.Save();
        }

        [EventSubscription("topic://Info", typeof(Subscribe<RedmineLog.Common.Global.IView>))]
        public void OnInfoEvent(object sender, EventArgs arg)
        {
            var t = "wewew";
            var t1 = t.ToString();
            Console.WriteLine("OnInfoEvent");
        }
    }
}
