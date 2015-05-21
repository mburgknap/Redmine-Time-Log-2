using RedmineLog.Common;
using RedmineLog.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    public class App
    {
        static App()
        {
            Context = new AppContext();
        }

        public static AppContext Context { get; private set; }
    }
}
