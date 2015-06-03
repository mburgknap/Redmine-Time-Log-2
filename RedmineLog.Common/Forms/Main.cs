using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class Main
    {
        public static class Events
        {
            public const string Load = "topic://Main/Load";
        }

        public interface IView
        {
        }

        public interface IModel
        { 
        }
    }
}
