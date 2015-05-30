using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Data
{
    public class AppContext
    {
        public readonly AppSettings Config = new AppSettings();

        public readonly ActivityData Activity = new ActivityData();

        public readonly LogData History = new LogData();

        public readonly TimeLogData Work = new TimeLogData();

        public readonly RedmineIssues IssuesCache = new RedmineIssues();

    }
}
