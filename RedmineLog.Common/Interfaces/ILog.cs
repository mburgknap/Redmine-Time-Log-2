using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public interface ILog
    {
        void Error(String inMessage, Exception ex);
        void Error(String inTag, Exception ex, String inMessage, string inTitle);
    }
}
