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
        public MainModel()
        {
            IssueComments = new IssueCommentList();
            WorkActivities = new WorkActivityList();
            Sync = new ModelSync<Main.IModel>();
        }

        public bool Resolve { get; set; }

        public TimeSpan WorkTime { get; set; }

        public TimeSpan IdleTime { get; set; }

        public WorkActivityList WorkActivities { get; private set; }

        public CommentData Comment { get; set; }

        public IssueData Issue { get; set; }

        public IModelSync Sync { get; private set; }

        public RedmineIssueData IssueParentInfo { get; set; }

        public RedmineIssueData IssueInfo { get; set; }

        public IssueCommentList IssueComments { get; private set; }

        public WorkActivityType Activity { get; set; }
    }
}
