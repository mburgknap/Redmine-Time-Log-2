using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using NLog;
using RedmineLog.Common;
using System;
using System.Runtime.InteropServices;
using System.Timers;

namespace RedmineLog.UI
{

    public class AppTimer : AppTime.IClock
    {

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
        
        private System.Timers.Timer workTimer;

        [EventPublication(AppTime.Events.WorkUpdate)]
        public event EventHandler<Args<int>> WorkUpdateEvent;

        [EventPublication(AppTime.Events.IdleUpdate)]
        public event EventHandler<Args<int>> IdleUpdateEvent;

        [EventPublication(AppTime.Events.TimeUpdate)]
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


        [Inject]
        public AppTimer()
        {
        }

        public void Start()
        {
            if (workTimer == null)
            {
                WorkTimer = new System.Timers.Timer(1000);
                WorkTimer.Start();
            }
        }
    }
}