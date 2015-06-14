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

        internal struct LastInput
        {
            public uint cSize;
            public uint dtime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LastInput rLI);


        private static AppTimers instance = new AppTimers();

        private System.Timers.Timer workTimer;
        private System.Timers.Timer idleTimer;

        [EventPublication(WorkUpdate)]
        public event EventHandler<Args<int>> WorkUpdateEvent;

        [EventPublication(IdleUpdate)]
        public event EventHandler<Args<int>> IdleUpdateEvent;
        private static bool isWorkClockEnabled;


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


        private void OnWorkElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            int sysupTime = Environment.TickCount;
            int lstTick = 0;
            int idlTick = 0;

            LastInput lInput = new LastInput();
            lInput.cSize = Convert.ToUInt32(Marshal.SizeOf(lInput));
            lInput.dtime = 0;

            if (GetLastInputInfo(ref lInput))
            {
                lstTick = Convert.ToInt32(lInput.dtime);
                idlTick = sysupTime - lstTick;
            }

            int totalIdleTimeInSeconds = idlTick / 1000;

            if (totalIdleTimeInSeconds > 1)
                IdleUpdateEvent.Fire(this, 1);
            else
                if (isWorkClockEnabled)
                    WorkUpdateEvent.Fire(this, 1);
        }

        internal static void StartWork()
        {
            isWorkClockEnabled = true;
        }

        internal static void StopWork()
        {
            isWorkClockEnabled = false;
        }

        internal static void Init(IEventBroker inGlobalEvent)
        {
            instance.WorkTimer = new System.Timers.Timer(1000);
            instance.WorkTimer.Start();
            inGlobalEvent.Register(instance);

        }
    }
}
