using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public interface IRedmineClient
    {
        int GetCurrentUser();
        IEnumerable<WorkActivityType> GetWorkActivityTypes();

        RedmineIssueData GetIssue(int idIssue);

        bool AddWorkTime(WorkTimeData workData);

        Uri IssueListUrl();

        void Resolve(IssueData issueData);

        void Resolve(WorkingIssue workingIssue);

        void Resolve(BugLogItem bugLogItem);

        IEnumerable<WorkLogItem> GetWorkLogs(int idUser, DateTime workData);

        void UpdateLog(WorkLogItem workLogItem);

        IEnumerable<BugLogItem> GetUserBugs(int idUser);

        int AddSubIssue(SubIssueData inIssueData);

        IEnumerable<UserData> GetUsers();

        IEnumerable<TrackerData> GetTrackers();

        IEnumerable<PriorityData> GetPriorites();
        
        string IssueUrl(int iniDIssue);
    }
}
