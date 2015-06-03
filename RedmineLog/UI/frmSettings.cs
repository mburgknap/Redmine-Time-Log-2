using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.UI.Common;
using RedmineLog.UI.Views;
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

    internal class SettingsView : Settings.IView, IView<frmSettings>
    {
        private Settings.IModel Model;

        private frmSettings Form;

        [Inject]
        public SettingsView(Settings.IModel inModel, IEventBroker inGlobalEvent)
        {
            Model = inModel;
            inGlobalEvent.Register(this);
        }

        [EventPublication(Settings.Events.Connect, typeof(Publish<Settings.IView>))]
        public event EventHandler ConnectEvent;

        [EventPublication(Settings.Events.Load, typeof(Publish<Settings.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(Global.Events.Info, typeof(Publish<IAppView>))]
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
    }
}