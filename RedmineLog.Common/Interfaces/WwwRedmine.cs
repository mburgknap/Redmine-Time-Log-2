using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class WwwRedmineEvent
    {
        public const string IssueAdd = "topic://WwwRedmine/Issue/Add";
        public const string IssueShow = "topic://WwwRedmine/Issue/Show";
        public const string IssueEdit = "topic://WwwRedmine/Issue/Edit";
    }

}
