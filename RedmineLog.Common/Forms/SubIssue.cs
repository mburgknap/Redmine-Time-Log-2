using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common.Forms
{
    public static class SubIssue
    {
        public static class Events
        {
            public const string Load = "topic://SubIssue/Load";
            public const string SetSubIssue = "topic://SubIssue/SetSubIssue";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            IModelSync Sync { get; }
            int ParentId { get; set; }

            String Subject { get; set; }
            String Description { get; set; }

            UserDataList Users { get; }
            UserData User { get; set; }

            TrackerDataList Trackers { get; }
            TrackerData Tracker { get; set; }

            PriorityDataList Priorities { get; }
            PriorityData Priority { get; set; }

            SubIssueData ToSubIssueData();

        }
    }
}
