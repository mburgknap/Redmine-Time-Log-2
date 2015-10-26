﻿using Appccelerate.EventBroker;
using RedmineLog.Common;
using System;
using System.Runtime.InteropServices;
using System.Timers;

namespace RedmineLog.UI
{
    internal class AppTimers
    {
        public const string WorkUpdate = "topic://Timer/Work/Update";
        public const string IdleUpdate = "topic://Timer/Idle/Update";
        public const string TimeUpdate = "topic://Timer/Time/Update";

        public enum ClockMode
        {
            Work,
            Idle
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        private static AppTimers instance = new AppTimers();

        private System.Timers.Timer workTimer;

        [EventPublication(WorkUpdate)]
        public event EventHandler<Args<int>> WorkUpdateEvent;

        [EventPublication(IdleUpdate)]
        public event EventHandler<Args<int>> IdleUpdateEvent;

        [EventPublication(TimeUpdate)]
        public event EventHandler TimeUpdateEvent;

        private System.Timers.Timer WorkTimer
        {
            get { return workTimer; }
            set
            {
                if (workTimer != null)
                    workTimer.Elapsed -= OnWorkElapsed;

                workTimer = value;
                if (workTimer != null)
                    workTimer.Elapsed += OnWorkElapsed;
            }
        }

        private static uint GetLastInputTime()
        {
            uint nIdleTime = 0;
            LASTINPUTINFO liiInfo = new LASTINPUTINFO();
            liiInfo.cbSize = Marshal.SizeOf(liiInfo);
            liiInfo.dwTime = 0;
            uint nEnvTicks = Convert.ToUInt32(Environment.TickCount);
            if (GetLastInputInfo(ref liiInfo))
            {
                uint nLastInputTick = liiInfo.dwTime;
                nIdleTime = nEnvTicks - nLastInputTick;

            }
            return ((nIdleTime > 0) ? (nIdleTime / 1000) : nIdleTime);
        }

        private void OnWorkElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            uint totalIdleTimeInSeconds = GetLastInputTime();

            if (totalIdleTimeInSeconds > 120)
                IdleUpdateEvent.Fire(this, 1);
            else
                WorkUpdateEvent.Fire(this, 1);

            TimeUpdateEvent.Fire(this, EventArgs.Empty);
        }

        internal static void Init(IEventBroker inGlobalEvent)
        {
            instance.WorkTimer = new System.Timers.Timer(1000);
            inGlobalEvent.Register(instance);
        }

        internal static void Start()
        {
            if (!instance.workTimer.Enabled)
                instance.WorkTimer.Start();
        }
    }
}