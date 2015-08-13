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
    public partial class BugLogItemView : UserControl, ICustomItem, ISpecialAction
    {

        class ExContextMenu : ContextMenuStrip
        {
            private ICustomItem item;
            private Action<string, object> data;
            public ExContextMenu()
            {
                Items.Add(new ToolStripMenuItem("Select", Resources.Select, (s, e) => { data("Select", item); }));
                Items.Add(new ToolStripMenuItem("Resolve", Resources.Resolve, (s, e) => { data("Resolve", item); }));
            }

            public void Set(ICustomItem inItem, Action<string, object> inData)
            {
                item = inItem;
                data = inData;
            }
        }

        static ExContextMenu menu = new ExContextMenu();

        public BugLogItemView()
        {
            InitializeComponent();
        }

        internal void SetDescription()
        {
            lblIdIssue.Text = "#ID";
            lblIssue.Text = "Issue Subject";
            lblPriority.Text = "Priority";
        }

        private void lblIssue_DragEnter(object sender, DragEventArgs e)
        {
            this.BackColor = Color.Azure;
        }

        private void lblIssue_DragLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Wheat;
        }

        internal Control Set(BugLogItem bug)
        {
            Data = bug;

            lblIdIssue.Text = bug.Id > 0 ? bug.Id.ToString() : "";
            lblIssue.Text = !string.IsNullOrWhiteSpace(bug.Subject) ? bug.Subject : " Blank issue ";
            lblPriority.Text = bug.Priority;

            return this;
        }

        public object Data { get; set; }

        public void Show(Action<string, object> inData)
        {
            menu.Set(this, inData);
            menu.Show(lblIssue, new Point(0, 0));
        }

    }
}
