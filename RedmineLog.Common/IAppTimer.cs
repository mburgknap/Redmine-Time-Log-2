using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class AppTime
    {
        public static class Events
        {
            public const string WorkUpdate = "topic://Timer/Work/Update";
            public const string IdleUpdate = "topic://Timer/Idle/Update";
            public const string TimeUpdate = "topic://Timer/Time/Update";
        }

        public interface IClock
        {
            void Start();
        }
    }

   
}
