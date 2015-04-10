using System;
using System.Windows.Forms;

namespace RedmineLog
{

    public class AppConstants
    {
        public RedmineConfig Config = new RedmineConfig();

        public HistoryData History = new HistoryData();


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