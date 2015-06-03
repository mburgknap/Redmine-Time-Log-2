using Appccelerate.EventBroker;
using Ninject;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using RedmineLog.Logic.Common;
using System;
using System.Collections.Specialized;

namespace RedmineLog.Logic
{
    internal class SettingsLogic : ILogic<Settings.IView>
    {
        [Inject]
        public SettingsLogic(Settings.IView inView, Settings.IModel inModel, IEventBroker inEvents, IRedmineClient inClient)
        {
            View = inView;
            Model = inModel;
            redmine = inClient;
            inEvents.Register(this);
            inEvents.Register(new Test());
        }

        private Settings.IView View;

        private Settings.IModel Model;

        private IRedmineClient redmine;

        private class Test
        {
            [EventSubscription(Settings.Events.Load, typeof(Subscribe<Settings.IView>))]
            public void OnLoadEvent(object sender, EventArgs arg)
            {
                Console.WriteLine("OnLoadEvent " + this.GetType().Name);
            }

            [EventSubscription(Settings.Events.Connect, typeof(Subscribe<Settings.IView>))]
            public void OnConnectEvent(object sender, EventArgs arg)
            {
                Console.WriteLine("OnConnectEvent " + this.GetType().Name);
            }
        }

        public void Apply(string inCmd)
        {
        }

        [EventSubscription(Settings.Events.Load, typeof(Subscribe<Settings.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            System.Diagnostics.Debug.WriteLine("View " + View.GetType().Name);
            System.Diagnostics.Debug.WriteLine("Model " + Model.GetType().Name);
        }

        [EventSubscription(Settings.Events.Connect, typeof(Subscribe<Settings.IView>))]
        public void OnConnectEvent(object sender, EventArgs arg)
        {
            Model.IdUser = redmine.GetCurrentUser();
            App.Context.Config.Save();
        }

        [EventSubscription(Global.Events.Info, typeof(Subscribe<Global.IView>))]
        public void OnInfoEvent(object sender, EventArgs arg)
        {
            var t = "wewew";
            var t1 = t.ToString();
            Console.WriteLine("OnInfoEvent");
        }
    }
}