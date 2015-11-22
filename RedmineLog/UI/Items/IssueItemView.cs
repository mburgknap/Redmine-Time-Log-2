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
using RedmineLog.Common;
using Ninject;
using Appccelerate.EventBroker;

namespace RedmineLog.UI.Items
{
    public partial class IssueItemView : UserControl, ICustomItem, ISpecialAction
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

        public IssueItemView()
        {
            InitializeComponent();
            lkIssue.SetLinkMouseClick(() => { IssueShowEvent.Fire(this, ((WorkingIssue)Data).Issue.Id); }, this.OnMouseClick);
        }

        [EventPublication(WwwRedmineEvent.IssueShow)]
        public event EventHandler<Args<int>> IssueShowEvent;

        [EventPublication(WwwRedmineEvent.IssueEdit)]
        public event EventHandler<Args<int>> IssueEditEvent;

        internal void SetDescription()
        {
            lbProject.Text = "Project";
            lbParentIssue.Text = "Parent issue";
            lkIssue.Text = "Issue subject";
            lbTime.Text = "Time";
            lbTracker.Text = "Activity type";
            lbIssueId.Text = "#Id";
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
            lbParentIssue.Text = issue.Parent != null ? issue.Parent.Subject + " :" : "";
            lkIssue.Text = !string.IsNullOrWhiteSpace(issue.Issue.Subject) ? issue.Issue.Subject : "";
            lbTime.Text = issue.Data.GetWorkTime(new TimeSpan(0)).ToString();
            lbTime.Visible = issue.Data.GetWorkTime(new TimeSpan(0)).TotalMinutes > 1;
            lbTracker.Text = issue.Issue.Tracker;
            lbProject.Text = issue.Issue.Project;
            lbComment.Text = issue.Comment;

            return this;
        }
    }
}
