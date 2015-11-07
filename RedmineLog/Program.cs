using Ninject;
using NLog;
using RedmineLog.Common;
using RedmineLog.Logic.Data;
using RedmineLog.UI;
using System;
using System.Windows.Forms;

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
            (Kernel = new StandardKernel())
                       .Load(new Bindings(),
                             new UI.Bindings(),
                             new Model.Bindings(),
                             new Logic.Bindings(),
                             new Logic.Data.Bindings());

            Application.ThreadException += OnUnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new frmMain());
        }

        private static void OnUnhandledException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Program.Kernel.Get<Logger>().Error("Unhandled Exception ", e.Exception);
        }
    }
}