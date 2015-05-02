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
            tbRedmineURL.Text = App.Context.Config.Url;
            tbApiKey.Text = App.Context.Config.ApiKey;
        }

        private void btnConnect_Click(System.Object sender, System.EventArgs e)
        {
            App.Context.Config.Url = tbRedmineURL.Text;
            App.Context.Config.ApiKey = tbApiKey.Text;
            App.Context.Config.Save();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}