using System;

namespace RedmineLog.Common
{

    public class Args<T> : EventArgs
    {
        public Args(T inValue)
        {
            Data = inValue;
        }
        public T Data { get; private set; }
    }

    public static class EventExtension
    {
        public static void Fire(this EventHandler inThis, object sender, EventArgs arg = null)
        {
            if (inThis != null)
                inThis(sender, arg ?? EventArgs.Empty);
        }
        public static void Fire<T>(this EventHandler<Args<T>> inThis, object sender, T arg)
        {
            if (inThis != null)
                inThis(sender, new Args<T>(arg));
        }
    }
}