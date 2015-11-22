using Ninject;
using NLog;
using RedmineLog.Common;
using RedmineLog.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog
{
    class AppLog : ILog
    {
        private static Logger Log;

        [Inject]
        public AppLog()
        {
            Log = LogManager.GetCurrentClassLogger();
        }

        public void Error(String inMessage, Exception ex)
        {
            Log.Error(inMessage, ex);
        }


        public void Error(string inTag, Exception ex, string inMessage, string inTitle)
        {
            Log.Error(inTag, ex);
            NotifyBox.Show(inMessage, inTitle);
        }
    }
}
