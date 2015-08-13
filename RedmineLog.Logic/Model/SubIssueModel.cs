using RedmineLog.Common;
using RedmineLog.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    class SubIssueModel : SubIssueData, SubIssue.IModel
    {
        public SubIssueModel()
        {
            Sync = new ModelSync<SubIssue.IModel>();
            Users = new UserDataList();
            Trackers = new TrackerDataList();
            Priorities = new PriorityDataList();
        }

        public IModelSync Sync { get; private set; }

        public UserDataList Users { get; private set; }

        public TrackerDataList Trackers { get; private set; }

        public PriorityDataList Priorities { get; private set; }

        public SubIssueData ToSubIssueData()
        {
            return this;
        }
    }
}
