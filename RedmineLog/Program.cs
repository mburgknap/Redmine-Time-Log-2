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

            (Kernel = new StandardKernel())
                       .Load(new Bindings(),
                             new UI.Bindings(),
                             new Model.Bindings(),
                             new Logic.Bindings(),
                             new Logic.Data.Bindings());

            Init();

            Application.ThreadException += OnUnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new frmMain());

            GC.KeepAlive(mutex);

        }

        private static void InitializeApp()
        {
            try
            {
                var appStart = new AppStart();

                if (appStart.IsFirstStart())
                {
                    NotifyBox.Show("Initialize application", "RedmineLog");

                    if (appStart.FindDataToImport())
                    {

                        var res = MessageBox.Show("Import data from old Redmine Log ?", "Question",
                                         MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                        if (res == DialogResult.OK) appStart.ImportData();
                    }

                }


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
        }

        private static void OnUnhandledException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Program.Kernel.Get<Logger>().Error("Unhandled Exception ", e.Exception, "Unhandled Exception", "Error");
        }


    }
}