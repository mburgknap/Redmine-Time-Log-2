using Appccelerate.EventBroker;
using Ninject;
using Ninject.Modules;
using RedmineLog.Logic;
using RedmineLog.Logic.Common;
using RedmineLog.UI.Common;
using RedmineLog.UI.Views;
using RedmineLog.Util.Common;
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
            this.Initialize<ISettingsView, frmSettings>();
        }
    }


    class SettingsView : ISettingsView, IView<frmSettings>
    {
        ISettingsModel Model;

        frmSettings Form;

        [Inject]
        public SettingsView(ISettingsModel inModel, IEventBroker inEvent)
        {
            Model = inModel;
            inEvent.Register(this);
        }

        [EventPublication("topic://RedmineLog/Settings/Connect")]
        public event EventHandler ConnectEvent;

        [EventPublication("topic://RedmineLog/Settings/Init")]
        public event EventHandler InitEvent;

        public void Init(frmSettings frmSettings)
        {
            Form = frmSettings;

            Form.tbRedmineURL.Text = Model.Url;
            Form.tbApiKey.Text = Model.ApiKey;

            Form.btnConnect.Click += this.OnConnectClick;

            InitEvent.Fire(this);
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