using Appccelerate.EventBroker;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.UI
{
    class AppTimers
    {

        public const string WorkUpdate = "topic://Timer/Work/Update";
        public const string IdleUpdate = "topic://Timer/Idle/Update";

        public enum ClockMode
        {
            Work,
            Idle
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);


        private static AppTimers instance = new AppTimers();

        private System.Timers.Timer workTimer;
        private System.Timers.Timer idleTimer;

        [EventPublication(WorkUpdate)]
        public event EventHandler<Args<int>> WorkUpdateEvent;

        [EventPublication(IdleUpdate)]
        public event EventHandler<Args<int>> IdleUpdateEvent;


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

        static uint GetLastInputTime()
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

            if (totalIdleTimeInSeconds > 1)
                IdleUpdateEvent.Fire(this, 1);
            else
                WorkUpdateEvent.Fire(this, 1);
        }

        internal static void Init(IEventBroker inGlobalEvent)
        {
            instance.WorkTimer = new System.Timers.Timer(1000);
            instance.WorkTimer.Start();
            inGlobalEvent.Register(instance);

        }
    }
}
