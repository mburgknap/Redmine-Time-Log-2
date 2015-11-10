using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class IssueLog
    {
        public static class Events
        {
            public const string Load = "topic://IssueLog/Load";
            public const string Select = "topic://IssueLog/Select";
            public const string Delete = "topic://IssueLog/Delete";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            DataProperty<WorkingIssueList> Issues { get; }
        }
    }
}
