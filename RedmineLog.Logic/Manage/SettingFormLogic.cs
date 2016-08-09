using Appccelerate.EventBroker;
using Ninject;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using RedmineLog.Logic.Common;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace RedmineLog.Logic
{
    internal class SettingFormLogic : ILogic<Settings.IView>
    {
        private IDatabase db;
        private IDbConfig dbConfig;
        private IDbRedmine dbRedmine;
        private IDbCache dbCache;
        private ILog log;
        private Settings.IModel model;
        private IRedmineClient redmine;
        private Settings.IView view;

        [Inject]
        public SettingFormLogic(Settings.IView inView, Settings.IModel inModel, IRedmineClient inClient, IDbRedmine inDbRedmine, IDbConfig inDbConfig, IDbCache inDbCache, IDatabase inDatabase, ILog inLog)
        {
            view = inView;
            model = inModel;
            redmine = inClient;
            dbRedmine = inDbRedmine;
            dbConfig = inDbConfig;
            dbCache = inDbCache;
            db = inDatabase;
            log = inLog;

            model.Display.OnNotify.Subscribe(OnNotifyDisplay);
            model.Timer.OnNotify.Subscribe(OnNotifyTimer);
        }

        private void OnNotifyDisplay(DisplayData obj)
        {
            dbConfig.SetDisplay(obj);
        }

        private void OnNotifyTimer(TimerType obj)
        {
            dbConfig.SetTimer(obj);
        }

        [EventSubscription(Settings.Events.ImportDb, typeof(OnPublisher))]
        public void OnImportDbEvent(object sender, Args<String> arg)
        {
            try
            {
                db.Import(arg.Data);
            }
            catch (Exception ex)
            {
                log.Error("OnImportDbEvent", ex);
            }
        }

        [EventSubscription(Settings.Events.Save, typeof(OnPublisher))]
        public void OnSaveEvent(object sender, EventArgs arg)
        {
            dbRedmine.SetApiKey(model.ApiKey.Value.ToString());
            dbRedmine.SetUrl(model.Url.Value.ToString());
            dbConfig.SetIdUser(redmine.GetCurrentUser().Id);
            dbConfig.SetWorkDayMinimalHours(model.WorkDayHours.Value);
            dbConfig.SetTimer(model.Timer.Value);
        }

        [EventSubscription(Settings.Events.Load, typeof(OnPublisher))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.ApiKey.Update(dbRedmine.GetApiKey());
            model.Url.Update(dbRedmine.GetUrl());
            model.Display.Update(dbConfig.GetDisplay());
            model.WorkDayHours.Update(dbConfig.GetWorkDayMinimalHours());
            model.Timer.Update(dbConfig.GetTimer());
            model.DbPath.Update(db.GetDbPath());
        }

        [EventSubscription(Settings.Events.ReloadCache, typeof(OnPublisher))]
        public void OnReloadCacheEvent(object sender, EventArgs arg)
        {
            dbCache.InitWorkActivities(redmine.GetWorkActivityTypes());

            var users = redmine.GetUsers().ToList();
            var myId = redmine.GetCurrentUser();
            var user = users.Where(x => x.Id == myId.Id).FirstOrDefault();

            if (user == null)
            {
                user = myId;
                users.Add(myId);
            }

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
            dbCache.InitProjects(redmine.GetProjects());
        }
    }
}