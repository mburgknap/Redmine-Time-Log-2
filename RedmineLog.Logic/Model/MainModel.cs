using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{

    internal class MainModel : Main.IModel
    {
        [Inject]
        public MainModel()
        {
            Resolve = new DataProperty<bool>();
            WorkTime = new DataProperty<TimeSpan>();
            IdleTime = new DataProperty<TimeSpan>();
            StartTime = new DataProperty<DateTime>();
            IssueComments = new DataProperty<IssueCommentList>();
            Activity = new DataProperty<WorkActivityType>();
            WorkActivities = new DataProperty<WorkActivityList>();
            LastIssues = new DataProperty<WorkingIssueList>();
            Comment = new DataProperty<CommentData>();
            Comment.Update(null);
            Issue = new DataProperty<IssueData>();
            IssueParentInfo = new DataProperty<RedmineIssueData>();
            IssueInfo = new DataProperty<RedmineIssueData>();
            WorkReport = new DataProperty<WorkReportData>();
        }

        public DataProperty<bool> Resolve { get; private set; }

        public DataProperty<TimeSpan> WorkTime { get; private set; }

        public DataProperty<TimeSpan> IdleTime { get; private set; }

        public DataProperty<DateTime> StartTime { get; private set; }

        public DataProperty<WorkActivityList> WorkActivities { get; private set; }

        public DataProperty<WorkActivityType> Activity { get; private set; }

        public DataProperty<CommentData> Comment { get; private set; }

        public DataProperty<IssueData> Issue { get; private set; }

        public DataProperty<RedmineIssueData> IssueParentInfo { get; private set; }

        public DataProperty<RedmineIssueData> IssueInfo { get; private set; }

        public DataProperty<IssueCommentList> IssueComments { get; private set; }

        public DataProperty<WorkingIssueList> LastIssues { get; private set; }
        public DataProperty<WorkReportData> WorkReport { get; private set; }

    }


}
