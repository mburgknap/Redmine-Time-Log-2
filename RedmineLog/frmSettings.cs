using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            tbRedmineURL.Text = App.Constants.Config.Url;
            tbApiKey.Text = App.Constants.Config.ApiKey;
        }

        private void btnConnect_Click(System.Object sender, System.EventArgs e)
        {
            App.Constants.Config.Url = tbRedmineURL.Text;
            App.Constants.Config.ApiKey = tbApiKey.Text;
            App.Constants.Config.Save();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}