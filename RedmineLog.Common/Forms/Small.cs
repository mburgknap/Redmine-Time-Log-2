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
            IModelSync Sync { get; }
            TimeSpan WorkTime { get; }
            TimeSpan IdleTime { get; }
            CommentData Comment { get; }
            RedmineIssueData IssueParentInfo { get; }
            RedmineIssueData IssueInfo { get; }
            string IssueUri { get; set; }
        }
    }
}
