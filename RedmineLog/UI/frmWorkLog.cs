using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Drawing;
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
            if (SystemInformation.VirtualScreen.Location.X < 0)
                this.Location = new Point(0 - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
            else
                this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }
    }

    internal class WorkLogView : WorkLog.IView, IView<frmWorkLog>
    {
        private WorkLog.IModel model;

        private frmWorkLog Form;

        private frmEditTimeLog editform;

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

        [EventPublication(WorkLog.Events.Edit)]
        public event EventHandler<Args<WorkLogItem>> EditEvent;

        public void Init(frmWorkLog inView)
        {
            Form = inView;
            Form.blLoadMore.Click += OnLoadMore;
            Form.blLoadMore.KeyDown += OnGridViewKeyDown;
            Form.blLoadMore.Focus();
            Form.dataGridView1.KeyDown += OnGridViewKeyDown;
            Form.dataGridView1.CellClick += OnGridCellClick;
            Form.FormClosing += OnCloseForm;
            Form.KeyDown += Form_KeyDown;
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

        void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Form.Close();
        }

        private void OnCloseForm(object sender, FormClosingEventArgs e)
        {
            if (editform != null)
                editform.Close();
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
                    if (editform == null)
                    {
                        editform = new frmEditTimeLog();
                        editform.FormClosed += (s, arg) =>
                        {
                            editform = null;
                        };
                        editform.Show();
                    }

                    EditEvent.Fire(this, timeEntry);
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

        private void OnLoadMore(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    LoadMoreEvent.Fire(this);
                });
        }


        private void OnWorkLogsChange()
        {
            Form.dataGridView1.Set(model,
                       (ui, data) =>
                       {
                           ui.Rows.Clear();

                           int headrow = -1;
                           int row = 0;

                           var workTime = new TimeSpan();


                           foreach (var item in data.WorkLogs)
                           {
                               if (item.Id < 0)
                               {
                                   UpdateWorkTime(ui, headrow, workTime);

                                   workTime = new TimeSpan();
                                   headrow = ui.Rows.Add(new Object[] { ToDayInfo(item.Date), "", "", item.Date.ToShortDateString() });
                                   ui.Rows[headrow].Cells[2].Style.BackColor = Color.LightBlue;
                                   ui.Rows[headrow].Cells[0].Style.BackColor = Color.LightBlue;
                                   ui.Rows[headrow].Cells[3].Style.BackColor = Color.LightBlue;
                                   ui.Rows[headrow].Cells[1].Style.BackColor = Color.Yellow;
                               }
                               else
                               {
                                   var time = new TimeSpan(0, (int)(item.Hours * 60), 0);
                                   workTime = workTime.Add(time);

                                   row = ui.Rows.Add(new Object[] {
                                                        item.IdIssue,
                                                        time.ToString(@"hh\:mm"),
                                                        item.ProjectName ,
                                                        "(" + item.ActivityName + ")" + Environment.NewLine + item.Comment });

                                   ui.Rows[row].Tag = item;
                               }
                           }

                           UpdateWorkTime(ui, headrow, workTime);

                           ui.FirstDisplayedScrollingRowIndex = headrow;

                           ui.Focus();

                       });

        }

        private void UpdateWorkTime(DataGridView ui, int headrow, TimeSpan workTime)
        {
            if (headrow >= 0)
            {
                ui.Rows[headrow].Cells[1].Value = workTime.ToString(@"hh\:mm");

                if (workTime.TotalHours < 8)
                    ui.Rows[headrow].Cells[1].Style.BackColor = Color.Red;
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