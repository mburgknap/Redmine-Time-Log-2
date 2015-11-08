using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class EditLog
    {
        public static class Events
        {
            public const string Save = "topic://EditLogTime/Save";
            public const string Load = "topic://EditLogTime/Load";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            DataProperty<WorkActivityList> WorkActivities { get; }
            DataProperty<WorkActivityType> Activity { get; }
            DataProperty<WorkLogItem> EditItem { get; }
            DataProperty<TimeSpan> Time { get; }
        }
    }
}
