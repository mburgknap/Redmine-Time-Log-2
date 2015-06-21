using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.UI.Common;
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
        private Settings.IModel model;

        private frmSettings Form;

        [Inject]
        public SettingsView(Settings.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
            inGlobalEvent.Register(this);
        }

        [EventPublication(Settings.Events.Connect, typeof(Publish<Settings.IView>))]
        public event EventHandler ConnectEvent;

        [EventPublication(Settings.Events.Load, typeof(Publish<Settings.IView>))]
        public event EventHandler LoadEvent;

        public void Init(frmSettings frmSettings)
        {
            Form = frmSettings;

            Form.btnConnect.Click += OnConnectClick;

            LoadEvent.Fire(this);
        }

        private void OnConnectClick(System.Object sender, System.EventArgs e)
        {
            model.Url = Form.tbRedmineURL.Text;
            model.ApiKey = Form.tbApiKey.Text;

            ConnectEvent.Fire(this);

            Form.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void OnUrlChange()
        {
            Form.tbRedmineURL.Text = model.Url;
        }

        private void OnApiKeyChange()
        {
            Form.tbApiKey.Text = model.ApiKey;
        }
    }
}