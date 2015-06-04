using Ninject.Modules;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Data
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabase>().To<Database>().InSingletonScope();
            Bind<IDbRedmine>().To<RedmineSetting>().InSingletonScope();
            Bind<IDbConfig>().To<AppConfig>().InSingletonScope();
            Bind<IDbIssue>().To<IssuesTable>().InSingletonScope();
            Bind<IDbComment>().To<CommentsTable>().InSingletonScope();
            Bind<IDbRedmineIssue>().To<RedmineIssuesTable>().InSingletonScope();
        }
    }
}
