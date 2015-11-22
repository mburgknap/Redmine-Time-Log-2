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
using Ninject;
using Appccelerate.EventBroker;

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
                Items.Add(new ToolStripMenuItem("Add issue", Resources.Add, (s, e) => { data("AddSubIssue", item); }));
                Items.Add(new ToolStripMenuItem("Edit issue", Resources.Edit, (s, e) => { data("EditSubIssue", item); }));
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
            lblIssue.SetLinkMouseClick(() => { IssueShowEvent.Fire(this, ((WorkingIssue)Data).Issue.Id); }, this.OnMouseClick);
        }

        [EventPublication(WwwRedmineEvent.IssueShow)]
        public event EventHandler<Args<int>> IssueShowEvent;

        [EventPublication(WwwRedmineEvent.IssueEdit)]
        public event EventHandler<Args<int>> IssueEditEvent;


        internal void SetDescription()
        {
            lblIdIssue.Text = "#ID";
            lblIssue.Text = "Issue Subject";
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

            lblIdIssue.Text = issue.Issue.Id > 0 ? "#" + issue.Issue.Id.ToString() : "";
            lblIssue.Text = !string.IsNullOrWhiteSpace(issue.Issue.Subject) ? issue.Issue.Subject : "";
            lblTime.Text = issue.Data.GetWorkTime(new TimeSpan(0)).ToString();
            lblTime.Visible = issue.Data.GetWorkTime(new TimeSpan(0)).TotalMinutes > 1;
            lblActivityType.Text = issue.Issue.Tracker;

            return this;
        }

        public object Data { get; set; }

        public void Show(Action<string, object> inData)
        {
            menu.Set(this, (tag, obj) =>
            {
                if (tag == "EditSubIssue")
                { IssueEditEvent.Fire(this, ((WorkingIssue)Data).Issue.Id); }
                else
                    inData(tag, obj);
            });
            menu.Show(this, 0, 0);

        }

    }
}
