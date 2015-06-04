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
            WorkActivities = new WorkActivityList();
        }

        public TimeSpan WorkTime { get; set; }

        public TimeSpan IdleTime { get; set; }

        public WorkActivityList WorkActivities { get; private set; }

        public CommentData Comment { get; set; }

        public IssueData Issue { get; set; }
    }
}
