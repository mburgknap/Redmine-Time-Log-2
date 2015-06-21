using Appccelerate.EventBroker;
using Ninject;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using RedmineLog.Logic;
using RedmineLog.Logic.Data;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmWorkLog : Form
    {
        public frmWorkLog()
        {
            InitializeComponent();
            this.Initialize<WorkLog.IView, frmWorkLog>();
        }

        private void OnWorkLogLoad(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }

    }

    internal class WorkLogView : WorkLog.IView, IView<frmWorkLog>
    {
        private WorkLog.IModel model;

        private frmWorkLog Form;

        [Inject]
        public WorkLogView(WorkLog.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
            inGlobalEvent.Register(this);
        }

        [EventPublication(WorkLog.Events.Load, typeof(Publish<WorkLog.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(WorkLog.Events.LoadMore, typeof(Publish<WorkLog.IView>))]
        public event EventHandler LoadMoreEvent;

        [EventPublication(WorkLog.Events.Select)]
        public event EventHandler<Args<WorkLogItem>> SelectEvent;

        public void Init(frmWorkLog inView)
        {
            Form = inView;
            Form.blLoadMore.Click += OnLoadMore;
            Form.dataGridView1.KeyDown += OnGridViewKeyDown;
            Form.dataGridView1.CellClick += OnGridCellClick;
            LoadEvent.Fire(this);
        }

        private void OnGridCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var timeEntry = Form.dataGridView1.Rows[e.RowIndex].Tag as WorkLogItem;

            if (timeEntry != null)
            {
                if (e.ColumnIndex != 1)
                    SelectIssue(timeEntry);
                else if (e.ColumnIndex == 1)
                {
                    var form = new frmEditTimeLog();
                    form.ShowDialog();
                }
            }
        }

        private void OnGridViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Form.Close();

            if (e.KeyCode == Keys.Enter && Form.dataGridView1.SelectedRows.Count > 0)
                SelectIssue(Form.dataGridView1.SelectedRows[0].Tag as WorkLogItem);
        }

        private void SelectIssue(WorkLogItem item)
        {
            if (item != null)
                SelectEvent.Fire(this, item);

            Form.Close();
        }

        private void OnLoadMore(object sender, EventArgs e)
        {
            LoadMoreEvent.Fire(this);
        }

        void OnWorkLogsChange()
        {
            Form.dataGridView1.Rows.Clear();

            int headrow = -1;
            int row = 0;

            var workTime = new TimeSpan();

            foreach (var item in model.WorkLogs)
            {
                if (item.Id < 0)
                {
                    UpdateWorkTime(headrow, workTime);

                    workTime = new TimeSpan();
                    headrow = Form.dataGridView1.Rows.Add(new Object[] { ToDayInfo(item.Date), "", "", item.Date.ToShortDateString() });
                    Form.dataGridView1.Rows[headrow].Cells[2].Style.BackColor = Color.LightBlue;
                    Form.dataGridView1.Rows[headrow].Cells[0].Style.BackColor = Color.LightBlue;
                    Form.dataGridView1.Rows[headrow].Cells[3].Style.BackColor = Color.LightBlue;
                    Form.dataGridView1.Rows[headrow].Cells[1].Style.BackColor = Color.Yellow;
                }
                else
                {
                    var time = new TimeSpan(0, (int)(item.Hours * 60), 0);
                    workTime = workTime.Add(time);

                    row = Form.dataGridView1.Rows.Add(new Object[] {
                            item.IdIssue,
                            time.ToString(@"hh\:mm"),
                            item.ProjectName ,
                            "(" + item.ActivityName + ")" + Environment.NewLine + item.Comment });

                    Form.dataGridView1.Rows[row].Tag = item;
                }
            }

            UpdateWorkTime(headrow, workTime);

            Form.dataGridView1.FirstDisplayedScrollingRowIndex = headrow;

            Form.dataGridView1.Focus();
        }

        private void UpdateWorkTime(int headrow, TimeSpan workTime)
        {
            if (headrow >= 0)
            {
                Form.dataGridView1.Rows[headrow].Cells[1].Value = workTime.ToString(@"hh\:mm");

                if (workTime.TotalHours < 8)
                    Form.dataGridView1.Rows[headrow].Cells[1].Style.BackColor = Color.Red;
            }
        }

        private object ToDayInfo(DateTime inDate)
        {
            if (DateTime.Today.Equals(inDate.Date))
                return "Today";

            return inDate.DayOfWeek.ToString();
        }
    }
}