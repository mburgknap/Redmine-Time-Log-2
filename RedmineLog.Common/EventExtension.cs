using System;

namespace RedmineLog.Common
{
    public static class EventExtension
    {
        public static void Fire(this EventHandler inThis, object sender, EventArgs arg = null)
        {
            if (inThis != null)
                inThis(sender, arg ?? EventArgs.Empty);
        }
    }
}