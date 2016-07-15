using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedmineLog.UI.Common;
using RedmineLog.Properties;
using Appccelerate.EventBroker;
using RedmineLog.Common;

namespace RedmineLog.UI.Items
{
    public partial class SearchItemView : UserControl, ICustomItem, ISpecialAction
    {
        public SearchItemView()
        {
            InitializeComponent();
            llIssue.SetLinkMouseClick(() => { IssueShowEvent.Fire(this, ((WorkingIssue)Data).Issue.Id); }, this.OnMouseClick);
        }

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

        [EventPublication(WwwRedmineEvent.IssueShow)]
        public event EventHandler<Args<int>> IssueShowEvent;

        [EventPublication(WwwRedmineEvent.IssueEdit)]
        public event EventHandler<Args<int>> IssueEditEvent;

        internal void SetDescription()
        {
            lblProject.Text = "Project";
            lblParentIssue.Text = "Parent issue";
            llIssue.Text = "Issue subject";
            lbIssueType.Text = "Activity type";
            lbIssueId.Text = "#id";
            lblUser.Text = "User";
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

        internal Control Set(WorkingIssue issue)
        {
            Data = issue;

            lbIssueId.Text = issue.Issue.Id > 0 ? "#" + issue.Issue.Id.ToString() : "";
            lblParentIssue.Text = issue.Parent != null ? issue.Parent.Subject + " :" : "";
            llIssue.Text = !string.IsNullOrWhiteSpace(issue.Issue.Subject) ? issue.Issue.Subject : "";
            lbIssueType.Text = issue.Issue.Tracker;
            lblProject.Text = issue.Issue.Project;
            lblUser.Text = issue.Issue.User;

            return this;
        }
    }
}
