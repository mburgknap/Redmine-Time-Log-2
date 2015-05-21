using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Common
{
    public interface ILogic<TView>
    {
        void Apply(string inCmd);
    }
}
