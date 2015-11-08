using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    internal class EditLogModel : EditLog.IModel
    {
        public EditLogModel()
        {
            WorkActivities = new DataProperty<WorkActivityList>();
            Activity = new DataProperty<WorkActivityType>();
            EditItem = new DataProperty<WorkLogItem>();
            Time = new DataProperty<TimeSpan>();
        }

        public DataProperty<WorkActivityList> WorkActivities { get; private set; }
        public DataProperty<WorkActivityType> Activity { get; private set; }
        public DataProperty<WorkLogItem> EditItem { get; private set; }
        public DataProperty<TimeSpan> Time { get; private set; }
    }
}
