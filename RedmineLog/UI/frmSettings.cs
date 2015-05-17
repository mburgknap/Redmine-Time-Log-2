using Ninject;
using Ninject.Modules;
using RedmineLog.Logic;
using RedmineLog.Logic.Common;
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
            ((SettingsView)App.Kernel.Get<ISettingsView>()).Init(this);
        }
    }


    class SettingsView : ISettingsView, IView<frmSettings>
    {
        ISettingsModel Model;

        frmSettings Form;

        [Inject]
        public SettingsView(ISettingsModel inModel)
        {
            Model = inModel;
        }

        public void Init(frmSettings frmSettings)
        {
            Form = frmSettings;

            Form.tbRedmineURL.Text = Model.Url;
            Form.tbApiKey.Text = Model.ApiKey;

            Form.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            App.Kernel.Get<ILogic<ISettingsView>>().Init();
        }


        private void btnConnect_Click(System.Object sender, System.EventArgs e)
        {
            Model.Url = Form.tbRedmineURL.Text;
            Model.ApiKey = Form.tbApiKey.Text;
            Model.Connect();

            Form.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}