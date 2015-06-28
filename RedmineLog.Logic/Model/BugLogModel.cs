using RedmineLog.Common;
using RedmineLog.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    class BugLogModel : BugLog.IModel
    {

        public BugLogModel()
        {
            Bugs = new BugLogList();
            Sync = new ModelSync<BugLog.IModel>();
        }

        public IModelSync Sync { get; private set; }

        public BugLogList Bugs { get; private set; }
    }
}
