﻿using DBreeze;
using DBreeze.DataTypes;
using DBreeze.Storage;
using Newtonsoft.Json;
using Ninject;
using NLog;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{

    internal class IssuesTable : IDbIssue
    {
        private IDatabase database;

        [Inject]
        public IssuesTable(IDatabase inDatabase)
        {
            database = inDatabase;
        }

        public void Init()
        {
            try
            {
                if (database.Get<IssuesTable, int, DbCustomSerializer<IssueData>>(0, null) == null)
                    database.Set<IssuesTable, int, DbCustomSerializer<IssueData>>(0, new IssueData() { Id = 0 });

                if (database.Get<IssuesTable, int, DbCustomSerializer<IssueData>>(-1, null) == null)
                    database.Set<IssuesTable, int, DbCustomSerializer<IssueData>>(-1, new IssueData() { Id = -1 });
            }
            catch (Exception ex)
            {
                ex.ToString();
                database.Set<IssuesTable, int, DbCustomSerializer<IssueData>>(0, new IssueData() { Id = 0 });
            }
        }

        public IssueData Get(int id)
        {
            var result = database.Get<IssuesTable, int, DbCustomSerializer<IssueData>>(id, null);

            if (result != null)
                return result.Get;

            return null;
        }

        public void Update(IssueData issueData)
        {
            database.Set<IssuesTable, int, DbCustomSerializer<IssueData>>(issueData.Id, issueData);
        }

        public void Delete(IssueData issueData)
        {
            database.Delete<IssuesTable, int>(issueData.Id);
        }

        public void Delete(BugLogItem bugData)
        {
            database.Delete<IssuesTable, int>(bugData.Id);
        }


        public IEnumerable<IssueData> GetList()
        {
            var result = new List<IssueData>();

            database.ForEach<IssuesTable, int, DbCustomSerializer<IssueData>>(item =>
            {
                result.Add(item.Get);
            });

            return result;
        }
    }

    internal class LastIssuesTable : IDbLastIssue
    {
        private IDatabase database;

        [Inject]
        public LastIssuesTable(IDatabase inDatabase)
        {
            database = inDatabase;
        }


        public void Init()
        {
            if (database.Get<LastIssuesTable, int, DbCustomSerializer<List<int>>>(1, null) == null)
                database.Set<LastIssuesTable, int, DbCustomSerializer<List<int>>>(1, new List<int>());
        }

        public void Update(List<int> inQueue)
        {
            database.Set<LastIssuesTable, int, DbCustomSerializer<List<int>>>(1, inQueue);
        }

        public List<int> GetList()
        {
            var result = database.Get<LastIssuesTable, int, DbCustomSerializer<List<int>>>(1, null);

            if (result != null)
                return result.Get;

            return new List<int>();
        }


        public void Delete(int inId)
        {
            var result = database.Get<LastIssuesTable, int, DbCustomSerializer<List<int>>>(1, null);

            if (result != null)
            {
                var tmp = result.Get;
                tmp.Remove(inId);
                database.Set<LastIssuesTable, int, DbCustomSerializer<List<int>>>(1, tmp);
            }
        }
    }

    internal class CommentsTable : IDbComment
    {
        private IDatabase database;

        [Inject]
        public CommentsTable(IDatabase inDatabase)
        {
            database = inDatabase;
        }

        public void Init()
        {

        }


        public IEnumerable<CommentData> GetList(IssueData inIssue)
        {
            return database.Get<CommentsTable, string, DbCustomSerializer<CommentData>>(inIssue.Comments.Select(x => x.ToString())).Select(x => x.Get).ToList();
        }


        public void Update(CommentData comment)
        {
            database.Set<CommentsTable, string, DbCustomSerializer<CommentData>>(comment.Id.ToString(), comment);
        }


        public void Delete(CommentData comment)
        {
            database.Delete<CommentsTable, string>(comment.Id);
        }

        public CommentData Get(IssueData inIssue)
        {
            var result = database.Get<CommentsTable, string, DbCustomSerializer<CommentData>>(inIssue.IdComment, null);

            if (result != null)
                return result.Get;

            return null;
        }
    }

    internal class CacheTable : IDbCache
    {
        private IDatabase database;

        [Inject]
        public CacheTable(IDatabase inDatabase)
        {
            database = inDatabase;
        }

        public void Init()
        {
        }


        public bool HasWorkActivities
        {
            get
            {
                var result = database.Get<CacheTable, String, DbCustomSerializer<List<WorkActivityType>>>(typeof(WorkActivityType).ToString(), null);

                return result != null;
            }
        }

        public IEnumerable<WorkActivityType> GetWorkActivityTypes()
        {
            var result = database.Get<CacheTable, String, DbCustomSerializer<List<WorkActivityType>>>(typeof(WorkActivityType).ToString(), null);

            if (result != null)
                return result.Get;

            return new List<WorkActivityType>();
        }

        public IEnumerable<UserData> GetUsers()
        {
            var result = database.Get<CacheTable, String, DbCustomSerializer<List<UserData>>>(typeof(UserData).ToString(), null);

            if (result != null)
                return result.Get;

            return new List<UserData>();
        }

        public IEnumerable<TrackerData> GetTrackers()
        {
            var result = database.Get<CacheTable, String, DbCustomSerializer<List<TrackerData>>>(typeof(TrackerData).ToString(), null);

            if (result != null)
                return result.Get;

            return new List<TrackerData>();
        }

        public IEnumerable<PriorityData> GetPriorities()
        {
            var result = database.Get<CacheTable, String, DbCustomSerializer<List<PriorityData>>>(typeof(PriorityData).ToString(), null);

            if (result != null)
                return result.Get;

            return new List<PriorityData>();
        }


        public void InitWorkActivities(IEnumerable<WorkActivityType> inData)
        {
            database.Set<CacheTable, String, DbCustomSerializer<List<WorkActivityType>>>(typeof(WorkActivityType).ToString(), new List<WorkActivityType>(inData));
        }


        public void InitUsers(IEnumerable<UserData> inData)
        {
            database.Set<CacheTable, String, DbCustomSerializer<List<UserData>>>(typeof(UserData).ToString(), new List<UserData>(inData));
        }

        public void InitTrackers(IEnumerable<TrackerData> inData)
        {
            database.Set<CacheTable, String, DbCustomSerializer<List<TrackerData>>>(typeof(TrackerData).ToString(), new List<TrackerData>(inData));
        }

        public void InitPriorities(IEnumerable<PriorityData> inData)
        {
            database.Set<CacheTable, String, DbCustomSerializer<List<PriorityData>>>(typeof(PriorityData).ToString(), new List<PriorityData>(inData));
        }
    }

    internal class RedmineIssuesTable : IDbRedmineIssue
    {
        private IDatabase database;

        [Inject]
        public RedmineIssuesTable(IDatabase inDatabase)
        {
            database = inDatabase;
        }

        public void Init()
        {
            database.Set<RedmineIssuesTable, int, DbCustomSerializer<RedmineIssueData>>(0, new RedmineIssueData() { Id = 0, Project = "", Subject = "" });
        }


        public RedmineIssueData Get(int id)
        {
            var result = database.Get<RedmineIssuesTable, int, DbCustomSerializer<RedmineIssueData>>(id, null);

            if (result != null)
                return result.Get;

            return null;
        }

        public void Update(RedmineIssueData issue)
        {
            database.Set<RedmineIssuesTable, int, DbCustomSerializer<RedmineIssueData>>(issue.Id, issue);
        }
    }

    internal class RedmineSetting : IDbRedmine
    {
        private IDatabase database;

        [Inject]
        public RedmineSetting(IDatabase inDatabase)
        {
            database = inDatabase;
        }

        public string GetUrl()
        {
            return database.Get<RedmineSetting, string, string>("Url", "");
        }

        public void SetUrl(string value)
        {
            database.Set<RedmineSetting, string, string>("Url", value);
        }

        public string GetApiKey()
        {
            return database.Get<RedmineSetting, string, string>("ApiKey", "");
        }

        public void SetApiKey(string value)
        {
            database.Set<RedmineSetting, string, string>("ApiKey", value);
        }
    }

    internal class AppConfig : IDbConfig
    {
        private IDatabase database;

        [Inject]
        public AppConfig(IDatabase inDatabase)
        {
            database = inDatabase;
        }

        public void Init()
        {
            if (database.Get<AppConfig, string, int>("WorkDayMinimalHours", -1) == -1)
                database.Set<AppConfig, string, int>("WorkDayMinimalHours", 8);
        }

        public int GetIdUser()
        {
            return database.Get<AppConfig, string, int>("IdUser", 0);
        }

        public void SetIdUser(int value)
        {
            database.Set<AppConfig, string, int>("IdUser", value);
        }


        public void SetDisplay(DisplayData value)
        {
            database.Set<AppConfig, string, DbCustomSerializer<DisplayData>>("DisplayData", value);
        }


        public DisplayData GetDisplay()
        {
            var result = database.Get<AppConfig, string, DbCustomSerializer<DisplayData>>("DisplayData", null);

            if (result != null)
                return result.Get;

            return null;
        }


        public DateTime? GetStartTime()
        {
            var result = database.Get<AppConfig, string, long>("StartTime", 0L);

            if (result != 0)
                return new DateTime(result);

            return (DateTime?)null;
        }

        public void SetStartTime(DateTime inDateTime)
        {
            database.Set<AppConfig, string, long>("StartTime", inDateTime.Ticks);
        }


        public int GetWorkDayMinimalHours()
        {
            return database.Get<AppConfig, string, int>("WorkDayMinimalHours", 0);
        }

        public void SetWorkDayMinimalHours(int value)
        {
            database.Set<AppConfig, string, int>("WorkDayMinimalHours", value);
        }
    }

    internal class Database : IDatabase
    {
        private Logger logger;
        private DBreezeEngine engine;

        [Inject]
        public Database(Logger inLogger)
        {
            logger = inLogger;

            try
            {
                DBreeze.Utils.CustomSerializator.Serializator = JsonConvert.SerializeObject;
                DBreeze.Utils.CustomSerializator.Deserializator = JsonConvert.DeserializeObject;

                var di = new DirectoryInfo(new Uri("App", UriKind.Relative).ToString());

                engine = new DBreezeEngine(new DBreezeConfiguration()
                {
                    DBreezeDataFolderName = Path.Combine(di.Parent.FullName, "Database"),
                    Storage = DBreezeConfiguration.eStorage.DISK,
                    Backup = new DBreeze.Storage.Backup()
                    {
                        BackupFolderName = Path.Combine(di.Parent.FullName, "Backup")
                    }
                });

                using (var tran = engine.GetTransaction())
                {
                    var item = tran.Select<string, bool>("DbSetting", "Init");

                    if (!item.Exists)
                    {
                        tran.Insert<string, bool>("DbSetting", "Init", true);
                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            { logger.Error("AppDatabase", ex); }
        }

        public TValue Get<Table, TKey, TValue>(TKey inKey, TValue inDefault)
        {
            try
            {
                using (var tran = engine.GetTransaction())
                {
                    var item = tran.Select<TKey, TValue>(typeof(Table).Name, inKey);

                    if (item.Exists)
                        return item.Value;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Get " + typeof(Table).Name
                          + " Key (" + typeof(TKey).Name + ") =" + inKey, ex);
            }

            return inDefault;
        }


        public void Set<Table, TKey, TValue>(TKey inKey, TValue inValue)
        {
            try
            {
                using (var tran = engine.GetTransaction())
                {
                    tran.Insert<TKey, TValue>(typeof(Table).Name, inKey, inValue);
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Set " + typeof(Table).Name
                          + " Key (" + typeof(TKey).Name + ") =" + inKey
                          + " Value (" + typeof(TValue).Name + ") =" + inValue, ex);
            }
        }

        public void Delete<Table, TKey>(TKey inKey)
        {
            try
            {
                using (var tran = engine.GetTransaction())
                {
                    tran.RemoveKey<TKey>(typeof(Table).Name, inKey);
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Delete " + typeof(Table).Name
                          + " Key (" + typeof(TKey).Name + ") =" + inKey, ex);
            }
        }


        public IEnumerable<TValue> Get<Table, TKey, TValue>(IEnumerable<TKey> inKeys)
        {
            try
            {
                using (var tran = engine.GetTransaction())
                {
                    var result = new List<TValue>();

                    foreach (var key in inKeys)
                    {
                        var item = tran.Select<TKey, TValue>(typeof(Table).Name, key);

                        if (item.Exists)
                            result.Add(item.Value);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Get " + typeof(Table).Name, ex);
            }

            return new List<TValue>();
        }


        public void ForEach<Table, TKey, TValue>(Action<TValue> OnValue)
        {
            try
            {
                using (var tran = engine.GetTransaction())
                {
                    foreach (var item in tran.SelectForward<TKey, TValue>(typeof(Table).Name))
                    {
                        if (item.Exists)
                            OnValue(item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ForEach " + typeof(Table).Name, ex);
            }
        }
    }
}
