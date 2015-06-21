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
            WorkActivities = new WorkActivityList();
            Sync = new ModelSync<EditLog.IModel>();
        }
        public IModelSync Sync { get; private set; }
        public WorkActivityList WorkActivities { get; private set; }
        public WorkActivityType Activity { get; set; }
        public WorkLogItem EditItem { get; set; }
        public TimeSpan Time { get; set; }
    }
}
