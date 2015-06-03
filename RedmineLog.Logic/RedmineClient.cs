using Ninject;
using Redmine.Net.Api;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{

    internal class RedmineClient : IRedmineClient
    {
        private IDbRedmine dbRedmine;

        [Inject]
        public RedmineClient(IDbRedmine inDbRedmine)
        {
            dbRedmine = inDbRedmine;
        }

        public int GetCurrentUser()
        {
            var parameters = new NameValueCollection { };
            var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());
            var user = manager.GetCurrentUser(parameters);

            return user.Id;
        }
    }
}
