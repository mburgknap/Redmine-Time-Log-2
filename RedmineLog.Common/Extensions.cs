﻿using System;
using System.Reactive.Subjects;
using System.Text;

namespace RedmineLog.Common
{

    public static class TimeSpanExtension
    {
        public static String ToWorkTime(this TimeSpan inThis)
        {
            return ((inThis.Days * 24) + inThis.Hours) + ":" + inThis.Minutes.ToString("00");
        }

        public static int WorkHours(this TimeSpan inThis)
        {
            return ((inThis.Days * 24) + inThis.Hours);
        }
    }

    public static class CastExtension
    {
        public static void Is<T>(this Object inThis, Action<T> inDo)
        {
            if (inThis is T && inDo != null) inDo((T)inThis);
        }
    }

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