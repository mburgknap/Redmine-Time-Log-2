using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    internal class SearchModel : Search.IModel
    {
        public SearchModel()
        {
            Issues = new WorkingIssueList();
            Sync = new ModelSync<Search.IModel>();
        }
        public IModelSync Sync { get; private set; }


        public WorkingIssueList Issues { get; private set; }
    }
}
