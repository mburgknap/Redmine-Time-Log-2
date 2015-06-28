using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSearch : Form
    {
        public frmSearch()
        {
            InitializeComponent();
            this.Initialize<Search.IView, frmSearch>();
        }

        private void OnSearchMouseLeave(object sender, EventArgs e)
        {
            //  this.Close();
        }

        private void OnSearchLoad(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }
    }

    internal class SearchView : Search.IView, IView<frmSearch>
    {
        private Search.IModel model;

        private frmSearch Form;

        [Inject]
        public SearchView(Search.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
            inGlobalEvent.Register(this);
        }

        [EventPublication(Search.Events.Load, typeof(Publish<Search.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(Search.Events.Select)]
        public event EventHandler<Args<WorkingIssue>> SelectEvent;

        public void Init(frmSearch inView)
        {
            Form = inView;
            Form.dataGridView1.KeyDown += OnKeyDown;
            Form.dataGridView1.CellClick += OnCellClick;

            Load();
        }

        public void Load()
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    LoadEvent.Fire(this);
                });
        }

        private void OnIssuesChange()
        {
            Form.dataGridView1.Set(model,
              (ui, data) =>
              {
                  int row;

                  ui.Rows.Clear();

                  foreach (var item in data.Issues.OrderByDescending(x =>
                  {
                      if (x.Issue.Id == 0) return int.MaxValue;
                      return x.Data.UsedCount;
                  }))
                  {
                      if (item.Data.Id > 0)
                          row = ui.Rows.Add(new Object[] {
                        item.Data.Id,
                        item.Issue.Project,
                        String.Format("{0}{1}",  item.Parent != null ? item.Parent.Subject + Environment.NewLine + "   ":"" , item.Issue.Subject )});
                      else
                          row = ui.Rows.Add(new Object[] { "", "", "" });

                      if (item.Data.Time.HasValue && item.Data.Time.Value > 0)
                          ui.Rows[row].Cells[0].Style.BackColor = Color.Red;
                      else
                          ui.Rows[row].Cells[0].Style.BackColor = Color.White;

                      ui.Rows[row].Tag = item;
                  }
              });
        }

        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
                SelectIssue(Form.dataGridView1.Rows[e.RowIndex].Tag as WorkingIssue);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Form.Close();

            if (Form.dataGridView1.SelectedRows.Count == 0)
                return;

            if (e.KeyCode == Keys.Enter)
                SelectIssue(Form.dataGridView1.SelectedRows[0].Tag as WorkingIssue);
        }

        private void SelectIssue(WorkingIssue item)
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
    }
}