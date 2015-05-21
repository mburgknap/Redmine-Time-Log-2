using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Matchers;
using Ninject;
using Ninject.Modules;
using RedmineLog.Common;
using RedmineLog.Logic;
using RedmineLog.Logic.Common;
using RedmineLog.UI.Common;
using RedmineLog.UI.Views;
using RedmineLog.Utils;
using RedmineLog.Utils.Common;
using System;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            this.Initialize<Settings.IView, frmSettings>();
        }
    }


    class SettingsView : Settings.IView, IView<frmSettings>
    {
        Settings.IModel Model;

        frmSettings Form;

        [Inject]
        public SettingsView(Settings.IModel inModel, IEventBroker inGlobalEvent)
        {
            Model = inModel;
            //  inGlobalEvent.AddGlobalMatcher(new MyMatcher());
            inGlobalEvent.Register(this);
        }

        [EventPublication(Settings.Connect, typeof(Publish<Settings.IView>))]
        public event EventHandler ConnectEvent;

        [EventPublication(Settings.Load, typeof(Publish<Settings.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication("topic://Global/Info", typeof(Publish<IAppView>))]
        public event EventHandler InfoEvent;

        public void Init(frmSettings frmSettings)
        {
            Form = frmSettings;

            Form.tbRedmineURL.Text = Model.Url;
            Form.tbApiKey.Text = Model.ApiKey;

            Form.btnConnect.Click += this.OnConnectClick;
            Form.FormClosed += (s, e) =>
            {
                InfoEvent.Fire(this);
                Console.WriteLine("FormClosed");
            };

            LoadEvent.Fire(this);
        }


        private void OnConnectClick(System.Object sender, System.EventArgs e)
        {
            Model.Url = Form.tbRedmineURL.Text;
            Model.ApiKey = Form.tbApiKey.Text;

            ConnectEvent.Fire(this);

            Form.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void DescribeTo(System.IO.TextWriter writer)
        {
            throw new NotImplementedException();
        }

        public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}