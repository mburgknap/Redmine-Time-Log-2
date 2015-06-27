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
    internal class SettingFormLogic : ILogic<Settings.IView>
    {
        private IDbConfig dbConfig;
        private IDbRedmine dbRedmine;
        private Settings.IModel model;
        private IRedmineClient redmine;
        private Settings.IView View;
        [Inject]
        public SettingFormLogic(Settings.IView inView, Settings.IModel inModel, IEventBroker inEvents, IRedmineClient inClient, IDbRedmine inDbRedmine, IDbConfig inDbConfig)
        {
            View = inView;
            model = inModel;
            redmine = inClient;
            dbRedmine = inDbRedmine;
            dbConfig = inDbConfig;
            inEvents.Register(this);
        }

        [EventSubscription(Settings.Events.Connect, typeof(Subscribe<Settings.IView>))]
        public void OnConnectEvent(object sender, EventArgs arg)
        {
            dbRedmine.SetApiKey(model.ApiKey);
            dbRedmine.SetUrl(model.Url);
            dbConfig.SetIdUser(redmine.GetCurrentUser());
        }

        [EventSubscription(Settings.Events.Load, typeof(Subscribe<Settings.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.ApiKey = dbRedmine.GetApiKey();
            model.Url = dbRedmine.GetUrl();

            model.Sync.Value(SyncTarget.View, "ApiKey");
            model.Sync.Value(SyncTarget.View, "Url");
        }
    }
}