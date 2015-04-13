using Redmine.Net.Api.Types;
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
    public partial class frmSmall : Form
    {
        private frmMain frmMain;

        public frmSmall()
        {
            InitializeComponent();
        }

        public frmSmall(frmMain frmMain)
            : this()
        {
            this.frmMain = frmMain;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);

        }

        private void OnFormClick(object sender, EventArgs e)
        {
            frmMain.WindowState = FormWindowState.Normal;
        }

        internal void UpdateWorkTime(DateTime inTime)
        {
            lbWorkTime.Text = inTime.ToLongTimeString();
        }

        internal void UpdateIdleTime(DateTime inTime)
        {
            lbIdleTime.Text = inTime.ToLongTimeString();
        }

        private void OnlIssueLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (lMainIssue.Tag != null)
                    System.Diagnostics.Process.Start(lMainIssue.Tag.ToString());
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("OnlIssueLinkClicked", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }



        internal void SetMainIssue(RedmineIssues.Item inIssue)
        {
            if (inIssue != null)
            {
                lMainIssue.Text = inIssue.Subject;
                lMainIssue.Tag = App.Constants.Config.Url + "issues/" + inIssue.Id;
            }
            else
            {
                lMainIssue.Text = "";
                lMainIssue.Tag = null;
            }
        }

        internal void SetParentIssue(RedmineIssues.Item inIssue)
        {
            if (inIssue != null)
                lbParentIssue.Text = inIssue.Subject;
            else
                lbParentIssue.Text = "";
        }

    }
}
