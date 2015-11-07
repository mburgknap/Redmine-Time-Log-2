using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class Small
    {
        public static class Events
        {
            public const string Load = "topic://Small/Load";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            DataProperty<TimeSpan> WorkTime { get; }
            DataProperty<TimeSpan> IdleTime { get; }
            DataProperty<CommentData> Comment { get; }
            DataProperty<RedmineIssueData> IssueParentInfo { get; }
            DataProperty<RedmineIssueData> IssueInfo { get; }
            DataProperty<StringBuilder> IssueUri { get; }
        }
    }
}
