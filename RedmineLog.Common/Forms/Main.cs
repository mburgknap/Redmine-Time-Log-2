using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class Main
    {
        public enum Actions
        {
            Issue,
            Idle,
            All
        }

        public static class Events
        {
            public const string Load = "topic://Main/Load";
            public const string Hide = "topic://Main/Hide";
            public const string Exit = "topic://Main/Exit";
            public const string Play = "topic://Main/Play";
            public const string Stop = "topic://Main/Stop";
            public const string Reset = "topic://Main/Reset";
            public const string Submit = "topic://Main/Submit";
            public const string AddIssue = "topic://Main/AddIssue";
            public const string DelIssue = "topic://Main/DelIssue";
            public const string AddComment = "topic://Main/AddComment";
            public const string DelComment = "topic://Main/DelComment";
        }

        public interface IView
        {
            void Update(RedmineIssueData parent, RedmineIssueData issue);
        }

        public interface IModel
        {
            TimeSpan WorkTime { get; set; }
            TimeSpan IdleTime { get; set; }
            WorkActivityList WorkActivities { get; }
            CommentData Comment { get; set; }
            IssueData Issue { get; set; }
        }
    }
}
