using RedmineLog.Logic.Data;

namespace RedmineLog.Logic
{
    public class App
    {
        static App()
        {
            Context = new AppContext();
        }

        public static AppContext Context { get; private set; }
    }
}