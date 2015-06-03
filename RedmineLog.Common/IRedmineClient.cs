using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public interface IRedmineClient
    {
        int GetCurrentUser();
    }
}
