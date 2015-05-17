using Ninject;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace RedmineLog
{

    public class AppContext
    {
        public readonly AppSettings Config = new AppSettings();

        public readonly LogData History = new LogData();

        public readonly TimeLogData Work = new TimeLogData();

        public readonly RedmineIssues IssuesCache = new RedmineIssues();

    }

    internal static class App
    {

        static App()
        {
            Context = new AppContext();
        }

        public static IKernel Kernel { get; private set; }

        public static AppContext Context { get; private set; }

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
                          new Logic.Bindings());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}