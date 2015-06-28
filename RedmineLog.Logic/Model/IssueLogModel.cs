using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    internal class IssueLogModel : IssueLog.IModel
    {
        public IssueLogModel()
        {
            Issues = new WorkingIssueList();
            Sync = new ModelSync<IssueLog.IModel>();
        }
        public IModelSync Sync { get; private set; }


        public WorkingIssueList Issues { get; private set; }
    }
}
