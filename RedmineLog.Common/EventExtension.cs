using System;

namespace RedmineLog.Common
{

    public class Args<T>: EventArgs
    {
        T Data { get; set; }
    }

    public static class EventExtension
    {
        public static void Fire(this EventHandler inThis, object sender, EventArgs arg = null)
        {
            if (inThis != null)
                inThis(sender, arg ?? EventArgs.Empty);
        }
    }
}