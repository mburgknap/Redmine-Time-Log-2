using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Appccelerate.EventBroker.Matchers.Scope;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{

    public enum WorkReportType
    {
        Week,
        LastWeek
    }

    public class WorkReportData
    {
        public DateTime WeekStart { get; set; }
        public TimeSpan Day1 { get; set; }
        public TimeSpan Day2 { get; set; }
        public TimeSpan Day3 { get; set; }
        public TimeSpan Day4 { get; set; }
        public TimeSpan Day5 { get; set; }
        public TimeSpan Day6 { get; set; }
        public TimeSpan Day7 { get; set; }
        public int MinimalHours { get; set; }
        public WorkReportType ReportType { get; set; }
        public Func<DayOfWeek, string> Uri { get; set; }
        public Func<WorkReportType, string> UriSummary { get; set; }
    }

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
            public const string Link = "topic://Main/Link";
            public const string Load = "topic://Main/Load";
            public const string Exit = "topic://Main/Exit";
            public const string Reset = "topic://Main/Reset";
            public const string Submit = "topic://Main/Submit";
            public const string Update = "topic://Main/Update";
            public const string AddIssue = "topic://Main/AddIssue";
            public const string DelIssue = "topic://Main/DelIssue";
            public const string AddComment = "topic://Main/AddComment";
            public const string SelectComment = "topic://Main/SelectComment";
            public const string UpdateComment = "topic://Main/UpdateComment";
            public const string DelComment = "topic://Main/DelComment";
            public const string AddSubIssue = "topic://Main/AddSubIssue";
            public const string IssueResolve = "topic://Main/IssueResolve";
            public const string WorkReportSync = "topic://Main/WorkReportSync";
            public const string WorkReportMode = "topic://Main/WorkReportMode";
        }

        public interface IView
        {
            void Info(string inMessage);

            void GoLink(Uri inUri);

            void Restart();
        }

        public interface IModel
        {
            DataProperty<bool> Resolve { get; }
            DataProperty<TimeSpan> WorkTime { get; }
            DataProperty<TimeSpan> IdleTime { get; }
            DataProperty<DateTime> StartTime { get; }
            DataProperty<WorkActivityList> WorkActivities { get; }
            DataProperty<WorkActivityType> Activity { get; }
            DataProperty<CommentData> Comment { get; }
            DataProperty<IssueData> Issue { get; }
            DataProperty<IssueCommentList> IssueComments { get; }
            DataProperty<RedmineIssueData> IssueParentInfo { get; }
            DataProperty<RedmineIssueData> IssueInfo { get; }
            DataProperty<WorkingIssueList> LastIssues { get; }
            DataProperty<WorkReportData> WorkReport { get; }
        }
    }
}
