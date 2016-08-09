using Ninject;
using NLog;
using RedmineLog.Common;
using RedmineLog.Logic.Data;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using RedmineLog.Utils;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace RedmineLog
{
    internal static class Program
    {
        public static IKernel Kernel { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            bool result;
            var mutex = new System.Threading.Mutex(true, "RedmineLog", out result);

            if (!result)
            {
                InitializeApp();
                return;
            }

            SetupVisual();

            do
            {
                StartApp();
                CleanApp();
            }
            while (Program.Restart);

            mutex.ReleaseMutex();
            GC.Collect();
            Environment.Exit(0);

        }

        private static void CleanApp()
        {
            Kernel.Get<IDatabase>().Dispose();
        }

        private static void SetupVisual()
        {
            Application.ThreadException += OnUnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        static void StartApp()
        {
            Restart = false;

            (Kernel = new StandardKernel())
                            .Load(new Bindings(),
                                  new UI.Bindings(),
                                  new Model.Bindings(),
                                  new Logic.Bindings(),
                                  new Logic.Data.Bindings());

            Init();

            Application.Run(new frmMain());

        }

        private static void InitializeApp()
        {
            try
            {
                MessageBox.Show("Another instance is already running.", "RedmineLog",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "RedmineLog", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private static void Init()
        {
            Kernel.Get<WebRedmine>();
            Kernel.Get<AppTime.IClock>().Init();
        }

        private static void OnUnhandledException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Program.Kernel.Get<Logger>().Error("Unhandled Exception ", e.Exception, "Unhandled Exception", "Error");
        }



        public static bool Restart { get; set; }
    }
}