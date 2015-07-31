using Appccelerate.EventBroker;
using Ninject;
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

        [Inject]
        public BugLogView(BugLog.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
            inGlobalEvent.Register(this);
        }


        [EventPublication(BugLog.Events.Load, typeof(Publish<BugLog.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(BugLog.Events.Select)]
        public event EventHandler<Args<BugLogItem>> SelectEvent;

        public void Init(frmBugLog inView)
        {
            Form = inView;
            Form.dataGrid.KeyDown += OnKeyDown;
            Form.dataGrid.CellClick += OnCellClick;

            Load();
        }

        private void OnBugsChange()
        {
            Form.dataGrid.Set(model,
              (ui, data) =>
              {
                  int row;
                  ui.Rows.Clear();
                  foreach (var item in data.Bugs)
                  {
                      row = ui.Rows.Add(new Object[] {
                        item.Id > 0 ? item.Id.ToString() : "",
                        item.Project,
                        item.Subject });

                      ui.Rows[row].Tag = item;

                      if (item.Id == 0)
                      {
                          ui.Rows[row].Cells[0].Style.BackColor = Color.Azure;
                          ui.Rows[row].Cells[1].Style.BackColor = Color.Azure;
                          ui.Rows[row].Cells[2].Style.BackColor = Color.Azure;
                      }

                  }

              });
        }


        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
                SelectBug(Form.dataGrid.Rows[e.RowIndex].Tag as BugLogItem);
        }

        private void SelectBug(BugLogItem item)
        {
            if (item.Id == 0)
                return;

            new frmProcessing().Show(Form,
               () =>
               {
                   if (item != null)
                       SelectEvent.Fire(this, item);
               }, () =>
               {
                   Form.Close();
               });
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Form.Close();

            if (Form.dataGrid.SelectedRows.Count == 0)
                return;

            if (e.KeyCode == Keys.Enter)
                SelectBug(Form.dataGrid.SelectedRows[0].Tag as BugLogItem);
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
