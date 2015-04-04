using RedmineLog.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            tbRedmineURL.Text = Settings.Default.RedmineURL;
            tbApiKey.Text = Settings.Default.ApiKey;
        }

        private void btnClose_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _close = true;
        }

        private void btnConnect_Click(System.Object sender, System.EventArgs e)
        {
            Settings.Default.PersistentSettings = cbSaveSettings.Checked.ToString();
            Settings.Default.RedmineURL = tbRedmineURL.Text;
            Settings.Default.ApiKey = tbApiKey.Text;
            Settings.Default.Save();

            AppLogger.Log.Info("Settings saved");
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
