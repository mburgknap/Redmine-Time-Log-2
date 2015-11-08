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
            WorkLogs = new DataProperty<WorkLogList>();
            LoadedTime = new DataProperty<DateTime>();
        }

        public DataProperty<WorkLogList> WorkLogs { get; private set; }

        public DataProperty<DateTime> LoadedTime { get; private set; }
    }
}
