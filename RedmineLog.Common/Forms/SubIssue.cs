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
            DataProperty<UserDataList> Users { get; }
            DataProperty<TrackerDataList> Trackers { get; }

            DataProperty<PriorityDataList> Priorities { get; }

            DataProperty<SubIssueData> SubIssueData { get; }

        }
    }
}
