﻿using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using Ninject.Modules;
using Redmine.Net.Api;
using RedmineLog.Logic.Common;
using RedmineLog.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    public interface ISettingsModel
    {
        string Url { get; set; }
        string ApiKey { get; set; }
        int IdUser { get; set; }
    }

    internal class SettingsLogic : ILogic<ISettingsView>
    {
        [Inject]
        public SettingsLogic(ISettingsView inView, ISettingsModel inModel, IEventBroker inEvent)
        {
            View = inView;
            Model = inModel;
            inEvent.Register(this);
        }

        private ISettingsView View;

        private ISettingsModel Model;

        public void Apply(string inCmd)
        {

        }

        [EventSubscription("topic://RedmineLog/Settings/Init", typeof(OnPublisher))]
        public void OnInitEvent(object sender, EventArgs arg)
        {
            System.Diagnostics.Debug.WriteLine("View " + View.GetType().Name);
            System.Diagnostics.Debug.WriteLine("Model " + Model.GetType().Name);
        }

        [EventSubscription("topic://RedmineLog/Settings/Connect", typeof(OnPublisher))]
        public void OnConnectEvent(object sender, EventArgs arg)
        {
            var parameters = new NameValueCollection { };
            var manager = new RedmineManager(Model.Url, Model.ApiKey);
            var user = manager.GetCurrentUser(parameters);
            Model.IdUser = user.Id;
            App.Context.Config.Save();
        }
    }
}
