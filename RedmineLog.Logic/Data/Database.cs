using DBreeze;
using DBreeze.DataTypes;
using Newtonsoft.Json;
using Ninject;
using NLog;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
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
            if (database.Get<IssuesTable, int, DbMJSON<IssueData>>(0, null) == null)
                database.Set<IssuesTable, int, DbMJSON<IssueData>>(0, new IssueData() { Id = 0 });
        }

        public IssueData Get(int id)
        {
            var result = database.Get<IssuesTable, int, DbMJSON<IssueData>>(id, null);

            if (result != null)
                return result.Get;

            return null;
        }

        public void Update(IssueData issueData)
        {
            database.Set<IssuesTable, int, DbMJSON<IssueData>>(issueData.Id, issueData);
        }


        public void Delete(IssueData issueData)
        {
            database.Delete<IssuesTable, int>(issueData.Id);
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
            return database.Get<CommentsTable, string, DbMJSON<CommentData>>(inIssue.Comments.Select(x => x.ToString())).Select(x => x.Get).ToList();
        }


        public void Update(CommentData comment)
        {
            database.Set<CommentsTable, string, DbMJSON<CommentData>>(comment.Id.ToString(), comment);
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
            database.Set<RedmineIssuesTable, int, DbMJSON<RedmineIssueData>>(0, new RedmineIssueData() { Id = 0, Project = "", Subject = "" });
        }


        public RedmineIssueData Get(int id)
        {
            var result = database.Get<RedmineIssuesTable, int, DbMJSON<RedmineIssueData>>(id, null);

            if (result != null)
                return result.Get;

            return null;
        }

        public void Update(RedmineIssueData issue)
        {
            database.Set<RedmineIssuesTable, int, DbMJSON<RedmineIssueData>>(issue.Id, issue);
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

        public int GetIdUser()
        {
            return database.Get<AppConfig, string, int>("IdUser", 0);
        }

        public void SetIdUser(int value)
        {
            database.Set<AppConfig, string, int>("IdUser", value);
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

                engine = new DBreezeEngine(new Uri("Database", UriKind.Relative).ToString());

                using (var tran = engine.GetTransaction())
                {
                    var item = tran.Select<string, bool>("DbSetting", "Init");

                    if (!item.Exists)
                    {
                        tran.Insert<string, bool>("DbSetting", "Init", true);
                        tran.Commit();
                    }

                    tran.Insert<string, bool>("DbSetting", "Init", true);
                    tran.Commit();
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
    }
}
