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
    public partial class WorkLogItemView : UserControl, ICustomItem, ISpecialAction
    {

        class ExContextMenu : ContextMenuStrip
        {
            private ICustomItem item;
            private Action<string, object> data;
            public ExContextMenu()
            {
                Items.Add(new ToolStripMenuItem("Select", Resources.Select, (s, e) => { data("Select", item); }));
                Items.Add(new ToolStripMenuItem("Edit", Resources.Edit, (s, e) => { data("Edit", item); }));
                Items.Add(new ToolStripMenuItem("Add Issue", Resources.Add, (s, e) => { data("AddSubIssue", item); }));
                Items.Add(new ToolStripMenuItem("Edit issue", Resources.Edit, (s, e) => { data("EditSubIssue", item); }));
            }

            public void Set(ICustomItem inItem, Action<string, object> inData)
            {
                item = inItem;
                data = inData;
            }
        }

        static ExContextMenu menu = new ExContextMenu();

        public WorkLogItemView()
        {
            InitializeComponent();
            lbIssue.SetLinkMouseClick(() => { IssueShowEvent.Fire(this, ((WorkLogItem)Data).Id); }, this.OnMouseClick);
        }

        [EventPublication(WwwRedmineEvent.IssueShow)]
        public event EventHandler<Args<int>> IssueShowEvent;

        [EventPublication(WwwRedmineEvent.IssueEdit)]
        public event EventHandler<Args<int>> IssueEditEvent;


        internal void SetDescription()
        {
            lbIssue.Text = "Issue";
            lblComment.Text = "Comment";
            lblTime.Text = "Time";
            lblActivityType.Text = "Activity type";
        }

        public object Data { get; set; }

        public void Show(Action<string, object> inData)
        {
            menu.Set(this, (tag, obj) =>
            {
                if (tag == "EditSubIssue")
                { IssueEditEvent.Fire(this, ((WorkLogItem)Data).Id); }
                else
                    inData(tag, obj);
            });
            menu.Show(lblComment, new Point(0, 0));
        }

        internal Control Set(WorkLogItem item)
        {
            Data = item;

            lbIssue.Text = item.Issue;
            lblComment.Text = item.Comment;
            lblActivityType.Text = item.ActivityName;

            var time = new TimeSpan(0, (int)Math.Round(item.Hours * 60, MidpointRounding.AwayFromZero), 0);

            lblTime.Text = time.ToString(@"hh\:mm");

            return this;
        }

    }
}
