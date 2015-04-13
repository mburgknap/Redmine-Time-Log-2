using System;
using System.Windows.Forms;

namespace RedmineLog
{

    public class AppConstants
    {
        public readonly RedmineConfig Config = new RedmineConfig();

        public readonly RedmineData History = new RedmineData();

        public readonly TimeLogData Work = new TimeLogData();

        public readonly RedmineIssues IssuesCache = new RedmineIssues();

    }

    internal static class App
    {

        static App()
        {
            Constants = new AppConstants();
        }

        public static AppConstants Constants { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}