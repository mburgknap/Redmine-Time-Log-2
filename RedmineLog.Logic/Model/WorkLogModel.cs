using RedmineLog.Common;
using RedmineLog.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    internal class WorkLogModel : WorkLog.IModel
    {
        public WorkLogModel()
        {
            WorkLogs = new WorkLogList();
            Sync = new ModelSync<WorkLog.IModel>();
        }

        public IModelSync Sync { get; private set; }

        public WorkLogList WorkLogs { get; private set; }

        public DateTime LoadedTime { get; set; }
    }
}
