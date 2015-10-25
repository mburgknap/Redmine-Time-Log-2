using Appccelerate.EventBroker;
using Ninject;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using RedmineLog.Logic.Common;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace RedmineLog.Logic
{
    internal class SettingFormLogic : ILogic<Settings.IView>
    {
        private IDbConfig dbConfig;
        private IDbRedmine dbRedmine;
        private IDbCache dbCache;
        private Settings.IModel model;
        private IRedmineClient redmine;
        private Settings.IView View;

        [Inject]
        public SettingFormLogic(Settings.IView inView, Settings.IModel inModel, IEventBroker inEvents, IRedmineClient inClient, IDbRedmine inDbRedmine, IDbConfig inDbConfig, IDbCache inDbCache)
        {
            View = inView;
            model = inModel;
            redmine = inClient;
            dbRedmine = inDbRedmine;
            dbConfig = inDbConfig;
            dbCache = inDbCache;
            model.Sync.Bind(SyncTarget.Source, this);
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
            model.Display = dbConfig.GetDisplay();

            model.Sync.Value(SyncTarget.View, "ApiKey");
            model.Sync.Value(SyncTarget.View, "Url");
            model.Sync.Value(SyncTarget.View, "Display");
        }

        [EventSubscription(Settings.Events.ReloadCache, typeof(Subscribe<Settings.IView>))]
        public void OnReloadCacheEvent(object sender, EventArgs arg)
        {
            dbCache.InitWorkActivities(redmine.GetWorkActivityTypes());

            var users = redmine.GetUsers().ToList();
            int myId = redmine.GetCurrentUser();
            var user = users.Where(x => x.Id == myId).First();
            user.IsDefault = true;
            dbCache.InitUsers(users);

            var trackers = redmine.GetTrackers();
            TrackerData tmpItem = null;

            var tmpTrackers = trackers.Where(x => x.Name.ToLower().StartsWith("zadanie")
                                            || x.Name.ToLower().Contains("błąd")).ToList();

            if (tmpTrackers.Count == 0)
            {
                tmpTrackers = trackers.ToList();
                tmpItem = trackers.FirstOrDefault();
            }
            else
            {
                tmpItem = tmpTrackers.Where(x => x.Name.ToLower().Contains("dev")).FirstOrDefault();
            }


            if (tmpItem != null)
                tmpItem.IsDefault = true;

            dbCache.InitTrackers(tmpTrackers);
            dbCache.InitPriorities(redmine.GetPriorites());
        }

        private void OnDisplayChange()
        {
            dbConfig.SetDisplay(model.Display);
        }
    }
}