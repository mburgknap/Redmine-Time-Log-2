using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class WorkLog
    {
        public static class Events
        {
            public const string Load = "topic://WorkLog/Load";
            public const string LoadMore = "topic://WorkLog/LoadMore";
            public const string Select = "topic://WorkLog/Select";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            IModelSync Sync { get; }
            DateTime LoadedTime { get; set; }
            WorkLogList WorkLogs { get; }
        }
    }
}
