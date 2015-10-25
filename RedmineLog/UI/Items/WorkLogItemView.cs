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
            lbIssue.SetLinkMouseClick(IssueLinkGo, this.OnMouseClick);
        }

        void IssueLinkGo()
        {
            try
            {
                System.Diagnostics.Process.Start(((WorkLogItem)Data).IssueUri);
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("GoLink", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        internal void SetDescription()
        {
            lbParentIssue.Text = "Parent issue";
            lbIssue.Text = "Issue";
            lblComment.Text = "Comment";
            lblTime.Text = "Time";
            lblActivityType.Text = "Activity type";
        }

        public object Data { get; set; }

        public void Show(Action<string, object> inData)
        {
            menu.Set(this, inData);
            menu.Show(lblComment, new Point(0, 0));
        }

        internal Control Set(WorkLogItem item)
        {
            Data = item;

            lbIssue.Text = item.Issue;
            lbParentIssue.Text = item.ParentIssue;
            lblComment.Text = item.Comment;
            lblActivityType.Text = item.ActivityName;

            var time = new TimeSpan(0, (int)(item.Hours * 60), 0);

            lblTime.Text = time.ToString(@"hh\:mm");

            return this;
        }

    }
}
