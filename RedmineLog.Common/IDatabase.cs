using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public interface IDbRedmine
    {
        string GetUrl();

        void SetUrl(string value);

        string GetApiKey();

        void SetApiKey(string value);
    }

    public interface IDbConfig
    {
        int GetIdUser();

        void SetIdUser(int value);
    }

    public interface IDatabase
    {
        TValue Get<TKey, TValue>(Type inTable, TKey inKey, TValue inDefault);

        void Set<TKey, TValue>(Type inTable, TKey inKey, TValue inDefault);
    }
}
