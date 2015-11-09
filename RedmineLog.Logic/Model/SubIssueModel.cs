using RedmineLog.Common;
using RedmineLog.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    class SubIssueModel : DataProperty<SubIssueData>, SubIssue.IModel
    {
        public SubIssueModel()
        {
            Users = new DataProperty<UserDataList>();
            Trackers = new DataProperty<TrackerDataList>();
            Priorities = new DataProperty<PriorityDataList>();
        }

        public DataProperty<UserDataList> Users { get; private set; }

        public DataProperty<TrackerDataList> Trackers { get; private set; }

        public DataProperty<PriorityDataList> Priorities { get; private set; }

        public DataProperty<SubIssueData> SubIssueData
        {
            get { return this; }
        }

    }
}
