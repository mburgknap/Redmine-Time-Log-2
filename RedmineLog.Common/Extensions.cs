using System;
using System.Reactive.Subjects;
using System.Text;

namespace RedmineLog.Common
{
    public static class StringBuilderExtension
    {

        public static void Init(this StringBuilder inThis, String inValue)
        {
            inThis.Clear();
            inThis.Append(inValue);
        }

    }

    public class Args<T> : EventArgs
    {
        public Args(T inValue)
        {
            Data = inValue;
        }
        public T Data { get; private set; }
    }

    public static class Extensions
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