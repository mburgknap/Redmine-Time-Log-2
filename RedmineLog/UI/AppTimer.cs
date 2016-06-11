using Appccelerate.EventBroker;
using Ninject;
using NLog;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace RedmineLog.UI
{

    interface IScheduleTimer
    {
        Action Elapsed { get; set; }

        void Start();
    }

    class ThreadScheduler : IScheduleTimer
    {
        private Thread thread;
        private int interval;

        public ThreadScheduler(int inInterval)
        {
            this.interval = inInterval;
            thread = new Thread(new ThreadStart(Execute));
        }

        public void Execute()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(interval);

                    if (Elapsed != null) Elapsed();
                }
            }
            catch (Exception ex)
            {

            }

        }

        public Action Elapsed { get; set; }


        public void Start()
        {
            thread.Start();
        }
    }

    class TimerScheduler : IScheduleTimer
    {
        private System.Timers.Timer timer;
        private int interval;

        public TimerScheduler(int inInterval)
        {
            this.interval = inInterval;
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += (s, e) =>
            {
                if (Elapsed != null) Elapsed();
            };
        }

        public Action Elapsed { get; set; }

        public void Start()
        {
            timer.Enabled = true;
            timer.Start();
        }
    }


    public class AppTimer : AppTime.IClock
    {
        private static Dictionary<TimerType, IScheduleTimer> timers = new Dictionary<TimerType, IScheduleTimer>();

        static AppTimer()
        {
            timers.Add(TimerType.System, new TimerScheduler(1000));
            timers.Add(TimerType.Thread, new ThreadScheduler(1000));
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

        private IScheduleTimer workTimer;

        [EventPublication(AppTime.Events.WorkUpdate)]
        public event EventHandler<Args<int>> WorkUpdateEvent;

        [EventPublication(AppTime.Events.IdleUpdate)]
        public event EventHandler<Args<int>> IdleUpdateEvent;

        [EventPublication(AppTime.Events.TimeUpdate)]
        public event EventHandler TimeUpdateEvent;

        private AppTime.ClockMode clockMode;
        private IDbConfig dbConfig;

        private IScheduleTimer WorkTimer
        {
            get { return workTimer; }
            set
            {
                if (workTimer != null)
                    workTimer.Elapsed = null;

                workTimer = value;
                if (workTimer != null)
                    workTimer.Elapsed = OnWorkElapsed;
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

        private void OnWorkElapsed()
        {
            if (clockMode == AppTime.ClockMode.Standard)
            {
                uint totalIdleTimeInSeconds = GetLastInputTime();

                System.Diagnostics.Debug.Write("time " + totalIdleTimeInSeconds);

                if (totalIdleTimeInSeconds > 120)
                    IdleUpdateEvent.Fire(this, 1);
                else
                    WorkUpdateEvent.Fire(this, 1);
            }
            else if (clockMode == AppTime.ClockMode.AlwaysIdle)
            {
                IdleUpdateEvent.Fire(this, 1);
            }

            TimeUpdateEvent.Fire(this, EventArgs.Empty);
        }

        [EventSubscription(AppTime.Events.SetupClock, typeof(OnPublisher))]
        public void OnSelectCommentEvent(object sender, Args<AppTime.ClockMode> arg)
        {
            clockMode = arg.Data;
        }

        [Inject]
        public AppTimer(IDbConfig inDbConfig)
        {
            dbConfig = inDbConfig;
        }

        public void Start()
        {
            if (workTimer == null)
            {
                WorkTimer = timers[dbConfig.GetTimer()];
                WorkTimer.Start();
            }
        }
    }
}