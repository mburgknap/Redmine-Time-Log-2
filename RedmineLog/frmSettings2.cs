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
    public partial class frmSettings2 : Form
    {
        private bool _close = false;

        public frmSettings2()
        {
            FormClosing += frmSettings_FormClosing;
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            txtRedmineURL.Text = Settings.Default.RedmineURL;
            txtApiKey.Text = Settings.Default.ApiKey;
        }

        private void btnConnect_Click(System.Object sender, System.EventArgs e)
        {
            Settings.Default.SaveSettings = checkBox1.Checked.ToString();
            Settings.Default.RedmineURL = txtRedmineURL.Text;
            Settings.Default.ApiKey = txtApiKey.Text;
            Settings.Default.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            _close = true;
        }

        private void btnClose_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
