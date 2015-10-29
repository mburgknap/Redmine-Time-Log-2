using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public interface IDbIssue
    {
        void Init();

        IssueData Get(int id);

        void Update(IssueData issueData);

        void Delete(IssueData issueData);

        void Delete(BugLogItem bugData);

        IEnumerable<IssueData> GetList();
    }

    public interface IDbLastIssue
    {
        void Init();

        void Update(List<int> inQueue);
        List<int> GetList();

        void Delete(int inId);
    }

    public interface IDbComment
    {

        void Init();

        IEnumerable<CommentData> GetList(IssueData inIssue);

        void Update(CommentData comment);

        void Delete(CommentData comment);
    }

    public interface IDbCache
    {
        void Init();

        bool HasWorkActivities { get; }

        IEnumerable<WorkActivityType> GetWorkActivityTypes();

        void InitWorkActivities(IEnumerable<WorkActivityType> inData);

        void InitUsers(IEnumerable<UserData> inData);

        void InitTrackers(IEnumerable<TrackerData> inData);

        void InitPriorities(IEnumerable<PriorityData> inData);

        IEnumerable<PriorityData> GetPriorities();

        IEnumerable<UserData> GetUsers();

        IEnumerable<TrackerData> GetTrackers();
    }


    public interface IDbRedmineIssue
    {

        void Init();

        RedmineIssueData Get(int id);

        void Update(RedmineIssueData issue);
    }


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

        void SetDisplay(DisplayData value);

        DisplayData GetDisplay();

        DateTime? GetStartTime();

        void SetStartTime(DateTime inDateTime);
    }

    public interface IDatabase
    {
        TValue Get<Table, TKey, TValue>(TKey inKey, TValue inDefault);

        IEnumerable<TValue> Get<Table, TKey, TValue>(IEnumerable<TKey> inKey);

        void ForEach<Table, TKey, TValue>(Action<TValue> OnValue);

        void Set<Table, TKey, TValue>(TKey inKey, TValue inValue);

        void Delete<Table, TKey>(TKey inKey);
    }
}
