using Ninject;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Common
{
    public class AppSettings : IAppSettings
    {
        private IDbConfig dbCache;

        [Inject]
        public AppSettings(IDbConfig inDbConfig)
        {
            dbCache = inDbConfig;
        }

        public DisplayData Display
        {
            get { return dbCache.GetDisplay(); }
        }
    }
}
