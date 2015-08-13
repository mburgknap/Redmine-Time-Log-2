using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common.Forms
{
    public static class BugLog
    {
        public static class Events
        {
            public const string Load = "topic://BugLog/Load";
            public const string Select = "topic://BugLog/Select";
            public const string Resolve = "topic://BugLog/Resolve";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            IModelSync Sync { get; }
            BugLogList Bugs { get; }
        }
    }
}
