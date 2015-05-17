using Ninject;
using Ninject.Modules;
using RedmineLog.Logic.Common;
using RedmineLog.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    public interface ISettingsModel
    {
        string Url { get; set; }
        string ApiKey { get; set; }
        Action Connect { get; set; }
    }

    internal class SettingsLogic : ILogic<ISettingsView>
    {
        [Inject]
        public SettingsLogic(ISettingsView inView, ISettingsModel inModel)
        {
            View = inView;
            Model = inModel;
            Model.Connect = () =>
            {
                App.Context.Config.Save();
            };
        }

        private ISettingsView View;

        private ISettingsModel Model;

        public void Init()
        {
            System.Diagnostics.Debug.WriteLine("View " + View.GetType().Name);
            System.Diagnostics.Debug.WriteLine("Model " + Model.GetType().Name);
        }

        public void Apply(string inCmd)
        {

        }
    }
}
