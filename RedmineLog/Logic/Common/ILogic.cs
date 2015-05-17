using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Common
{
    interface ILogic<TView>
    {
        void Init();

        void Apply(string inCmd);
    }
}
