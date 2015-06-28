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
        }

        public interface IView
        {
            void Load();
        }

        public interface IModel
        {
            IModelSync Sync { get; }
            WorkActivityList WorkActivities { get; }
            WorkActivityType Activity { get; set; }
            WorkLogItem EditItem { get; set; }
            TimeSpan Time { get; set; }
        }
    }
}
