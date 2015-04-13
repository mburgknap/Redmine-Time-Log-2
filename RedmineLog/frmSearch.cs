using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSearch : Form
    {

        public Action<int> OnSelect;
        public frmSearch()
        {
            InitializeComponent();
        }

        private void OnSearchDeactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnSearchLoad(object sender, EventArgs e)
        {

            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);

            RedmineIssues.Item main = null;
            RedmineIssues.Item parent = null;

            int row;

            foreach (var item in App.Constants.History)
            {
                main = App.Constants.IssuesCache.GetIssue(item.Id);

                if (main != null && main.IdParent.HasValue)
                    parent = App.Constants.IssuesCache.GetIssue(main.IdParent.Value);
                else
                    parent = null;

                if (item.Id > 0)
                    row = dataGridView1.Rows.Add(new Object[] { item.Id, String.Format("{0}{1}{2}", GetSubject(parent), Environment.NewLine
                                                                                                  , "   " + GetSubject(main)) });
                else
                    row = dataGridView1.Rows.Add(new Object[] { "", "" });

                dataGridView1.Rows[row].Tag = item;

            }

        }

        private string GetSubject(RedmineIssues.Item main)
        {
            if (main != null)
                return main.Subject;
            return "";
        }

        private void OnSearchMouseLeave(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var item = dataGridView1.Rows[e.RowIndex].Tag as RedmineData.Issue;

            if (item != null && OnSelect != null)
                OnSelect(item.Id);

            this.Close();
        }
    }
}
