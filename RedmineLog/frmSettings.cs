using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSettings : Form
    {
        private bool _close = false;

        public frmSettings()
        {
            FormClosing += frmSettings_FormClosing;
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            tbRedmineURL.Text = App.Constants.Config.Url;
            tbApiKey.Text = App.Constants.Config.ApiKey;
        }

        private void btnClose_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _close = true;
        }

        private void btnConnect_Click(System.Object sender, System.EventArgs e)
        {
            App.Constants.Config.Url = tbRedmineURL.Text;
            App.Constants.Config.ApiKey = tbApiKey.Text;
            App.Constants.Config.Save();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            _close = true;
        }

        private void frmSettings_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (_close == false)
            {
                e.Cancel = true;
            }
        }
    }
}