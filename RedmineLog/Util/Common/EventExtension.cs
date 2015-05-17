using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Util.Common
{
    static class EventExtension
    {
        public static void Fire(this EventHandler inThis, object sender, EventArgs arg = null)
        {
            if (inThis != null)
                inThis(sender, arg ?? EventArgs.Empty);
        }
    }
}
