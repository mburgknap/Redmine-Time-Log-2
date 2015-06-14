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

        Uri IssueUrl(IssueData issueData);
    }
}
