using Appccelerate.EventBroker;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI
{
    public partial class frmBugLog : Form
    {
        public frmBugLog()
        {
            InitializeComponent();
            this.Initialize<BugLog.IView, frmBugLog>();
        }
        private void OnBugLogLoad(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }
    }

    internal class BugLogView : BugLog.IView, IView<frmBugLog>
    {
        private BugLog.IModel model;

        private frmBugLog Form;


        [EventPublication(BugLog.Events.Load, typeof(Publish<BugLog.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(BugLog.Events.Select)]
        public event EventHandler<Args<BugLogItem>> SelectEvent;

        public void Init(frmBugLog inView)
        {
            Form = inView;
            Form.dataGridView1.KeyDown += OnKeyDown;
            Form.dataGridView1.CellClick += OnCellClick;

            Load();
        }

        private void OnBugsChange()
        {
            Form.dataGridView1.Set(model,
              (ui, data) =>
              {
                  int row;

                  ui.Rows.Clear();

                  foreach (var item in data.Bugs)
                  {
                      row = ui.Rows.Add(new Object[] {
                        item.Id,
                        //item.Project,
                        item.Subject });

                      ui.Rows[row].Tag = item;
                  }
              });
        }


        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
                SelectBug(Form.dataGridView1.Rows[e.RowIndex].Tag as BugLogItem);
        }

        private void SelectBug(BugLogItem item)
        {
            new frmProcessing().Show(Form,
               () =>
               {
                   if (item != null)
                       SelectEvent.Fire(this, item);

                   Form.Set(model,
                       (ui, data) =>
                       {
                           Form.Close();
                       });
               });
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Form.Close();

            if (Form.dataGridView1.SelectedRows.Count == 0)
                return;

            if (e.KeyCode == Keys.Enter)
                SelectBug(Form.dataGridView1.SelectedRows[0].Tag as BugLogItem);
        }

        public void Load()
        {
            new frmProcessing().Show(Form,
                  () =>
                  {
                      LoadEvent.Fire(this);
                  });
        }
    }
}
