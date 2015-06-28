using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.UI;
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

            Load();
        }

        public void Load()
        {
            new frmProcessing().Show(Form,
               () =>
               {
                   LoadEvent.Fire(this);
               });
        }

        private void OnConnectClick(System.Object sender, System.EventArgs e)
        {
            model.Url = Form.tbRedmineURL.Text;
            model.ApiKey = Form.tbApiKey.Text;

            new frmProcessing().Show(Form,
              () =>
              {
                  ConnectEvent.Fire(this);

                  Form.Set(model,
                     (ui, data) =>
                     {
                         ui.DialogResult = System.Windows.Forms.DialogResult.OK;
                     });
              });
        }

        private void OnUrlChange()
        {
            Form.tbRedmineURL.Set(model.Url,
                       (ui, data) =>
                       {
                           ui.Text = data;
                       });
        }

        private void OnApiKeyChange()
        {
            Form.tbApiKey.Set(model.ApiKey,
                      (ui, data) =>
                      {
                          ui.Text = data;
                      });
        }
    }
}