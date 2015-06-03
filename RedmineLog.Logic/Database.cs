using DBreeze;
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
            return database.Get<string, string>(typeof(RedmineSetting), "Url", "");
        }

        public void SetUrl(string value)
        {
            database.Set<string, string>(typeof(RedmineSetting), "Url", value);
        }

        public string GetApiKey()
        {
            return database.Get<string, string>(typeof(RedmineSetting), "ApiKey", "");
        }

        public void SetApiKey(string value)
        {
            database.Set<string, string>(typeof(RedmineSetting), "ApiKey", value);
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
            return database.Get<string, int>(typeof(AppConfig), "IdUser", 0);
        }

        public void SetIdUser(int value)
        {
            database.Set<string, int>(typeof(AppConfig), "IdUser", value);
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
                engine = new DBreezeEngine(new Uri("Database", UriKind.Relative).ToString());

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

        public TValue Get<TKey, TValue>(Type inTable, TKey inKey, TValue inDefault)
        {
            try
            {
                using (var tran = engine.GetTransaction())
                {
                    var item = tran.Select<TKey, TValue>(inTable.Name, inKey);

                    if (item.Exists)
                        return item.Value;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Get " + inTable.Name
                          + " Key (" + typeof(TKey).Name + ") =" + inKey, ex);
            }

            return inDefault;
        }


        public void Set<TKey, TValue>(Type inTable, TKey inKey, TValue inValue)
        {
            try
            {
                using (var tran = engine.GetTransaction())
                {
                    tran.Insert<TKey, TValue>(inTable.Name, inKey, inValue);
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Set " + inTable.Name
                          + " Key (" + typeof(TKey).Name + ") =" + inKey
                          + " Value (" + typeof(TValue).Name + ") =" + inValue, ex);
            }
        }
    }
}
