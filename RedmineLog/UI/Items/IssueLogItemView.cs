using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedmineLog.Common;
using RedmineLog.UI.Common;
using RedmineLog.Properties;

namespace RedmineLog.UI.Items
{



    public partial class IssueLogItemView : UserControl, ICustomItem, ISpecialAction
    {

        class ExContextMenu : ContextMenuStrip
        {
            private ICustomItem item;
            private Action<string, object> data;
            public ExContextMenu()
            {
                Items.Add(new ToolStripMenuItem("Select", Resources.Select, (s, e) => { data("Select", item); }));
                Items.Add(new ToolStripMenuItem("Resolve", Resources.Resolve, (s, e) => { data("Resolve", item); }));
                Items.Add(new ToolStripMenuItem("Delete", Resources.Remove, (s, e) => { data("Delete", item); }));
            }

            public void Set(ICustomItem inItem, Action<string, object> inData)
            {
                item = inItem;
                data = inData;
            }
        }

        static ExContextMenu menu = new ExContextMenu();

        public IssueLogItemView()
        {
            InitializeComponent();
            lblIssue.SetLinkMouseClick(IssueLinkGo, this.OnMouseClick);
        }

        void IssueLinkGo()
        {
            try
            {
                System.Diagnostics.Process.Start(((WorkingIssue)Data).IssueUri);
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("GoLink", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }


        internal void SetDescription()
        {
            lblIdIssue.Text = "#ID";
            lblIssue.Text = "Issue Subject";
            lblParentIssue.Text = "Main Subject";
            lblTime.Text = "Time";
            lblActivityType.Text = "Activity type";
        }

        private void lblIssue_DragEnter(object sender, DragEventArgs e)
        {
            this.BackColor = Color.Azure;
        }

        private void lblIssue_DragLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Wheat;
        }

        internal Control Set(WorkingIssue issue)
        {
            Data = issue;

            lblIdIssue.Text = issue.Issue.Id > 0 ? issue.Issue.Id.ToString() : "";
            lblIssue.Text = !string.IsNullOrWhiteSpace(issue.Issue.Subject) ? issue.Issue.Subject : " Blank issue ";
            lblParentIssue.Text = issue.Parent != null ? issue.Parent.Subject : "None";
            lblTime.Text = issue.Data.GetWorkTime(new TimeSpan(0)).ToString();
            lblTime.Visible = issue.Data.GetWorkTime(new TimeSpan(0)).TotalMinutes > 1;
            lblActivityType.Text = issue.Issue.Tracker;

            return this;
        }

        public object Data { get; set; }

        public void Show(Action<string, object> inData)
        {
            menu.Set(this, inData);
            menu.Show(lblParentIssue, new Point(0, 0));
        }
    }
}
