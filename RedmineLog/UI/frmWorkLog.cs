﻿using Appccelerate.EventBroker;
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
        public Action<int, string> OnSelect;
        private Point appLocation;
        private DateTime backDate;

        public frmWorkLog()
        {
            InitializeComponent();
            this.Initialize<WorkLog.IView, frmWorkLog>();
        }

        private void OnWorkLogLoad(object sender, EventArgs e)
        {
            this.Location = new Point(appLocation.X - 100, appLocation.Y);

            LoadWorkinIssue(DateTime.Now, true);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                LoadWorkinIssue(backDate = DateTime.Now.AddDays(-3), false);
            else
                LoadWorkinIssue(backDate = DateTime.Now.AddDays(-1), false);

            if (dataGridView1.RowCount > 0)
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
        }

        private void LoadWorkinIssue(DateTime inSpendOn, bool inClear)
        {
            try
            {
                var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);

                var parameters = new NameValueCollection { };
                parameters.Add("user_id", App.Context.Config.IdUser.ToString());
                parameters.Add("spent_on", inSpendOn.ToString("yyyy-MM-dd"));

                int headrow = 0;
                int row = 0;

                var results = from p in manager.GetObjectList<TimeEntry>(parameters)
                              orderby p.SpentOn descending
                              group p by p.SpentOn.Value.Date into g
                              select new { Date = g.Key, Entities = g.ToList() };

                if (inClear)
                    dataGridView1.Rows.Clear();

                foreach (var listItem in results)
                {
                    headrow = dataGridView1.Rows.Add(new Object[] { ToDayInfo(listItem.Date), "", "", listItem.Date.ToShortDateString() });
                    dataGridView1.Rows[headrow].Cells[2].Style.BackColor = Color.LightBlue;
                    dataGridView1.Rows[headrow].Cells[0].Style.BackColor = Color.LightBlue;
                    dataGridView1.Rows[headrow].Cells[3].Style.BackColor = Color.LightBlue;
                    dataGridView1.Rows[headrow].Cells[1].Style.BackColor = Color.Yellow;

                    var workTime = new TimeSpan();

                    foreach (var item in listItem.Entities)
                    {
                        var time = new TimeSpan(0, (int)(item.Hours * 60), 0);
                        workTime = workTime.Add(time);

                        row = dataGridView1.Rows.Add(new Object[] {
                            item.Issue.Id,
                            time.ToString(@"hh\:mm"),
                            item.Project.Name ,
                            "(" + item.Activity.Name + ")" + Environment.NewLine + item.Comments });

                        dataGridView1.Rows[row].Tag = item;
                    }

                    dataGridView1.Rows[headrow].Cells[1].Value = workTime.ToString(@"hh\:mm");

                    if (workTime.TotalHours < 8)
                        dataGridView1.Rows[headrow].Cells[1].Style.BackColor = Color.Red;
                }

                dataGridView1.FirstDisplayedScrollingRowIndex = headrow;

                dataGridView1.Focus();
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("OnWorkLogLoad", ex);
            }
        }

        private object ToDayInfo(DateTime inDate)
        {
            if (DateTime.Today.Equals(inDate.Date))
                return "Today";

            return inDate.DayOfWeek.ToString();
        }

        private string GetSubject(RedmineIssues.Item main)
        {
            if (main != null)
                return main.Subject;
            return "";
        }

        internal void Init(Point point)
        {
            appLocation = point;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var timeEntry = dataGridView1.Rows[e.RowIndex].Tag as TimeEntry;

            if (timeEntry != null)
            {
                if (e.ColumnIndex != 1)
                    SelectIssue(timeEntry);
                else if (e.ColumnIndex == 1)
                {
                    var form = new frmEditTimeLog();
                    form.Location = new Point(this.Location.X - form.Width, this.Location.Y);
                    form.Init(timeEntry);
                    form.OnChange = () =>
                    {
                        LoadWorkinIssue(DateTime.Now, true);
                    };
                    form.ShowDialog();
                }
            }
        }

        private void SelectIssue(TimeEntry item)
        {
            if (item != null && OnSelect != null)
            {
                var data = App.Context.History
                                      .Where(x => x.Id == item.Issue.Id)
                                      .FirstOrDefault();
                if (data != null)
                {
                    data.UsedCount += 1;
                    App.Context.History.Save();
                }
                OnSelect(item.Issue.Id, item.Comments);
            }
            this.Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.Enter && dataGridView1.SelectedRows.Count > 0)
                SelectIssue(dataGridView1.SelectedRows[0].Tag as TimeEntry);
        }

        private void blLoadMore_Click(object sender, EventArgs e)
        {
            backDate = backDate.AddDays(-1);

            if (backDate.DayOfWeek == DayOfWeek.Sunday)
                backDate = backDate.AddDays(-2);
            if (backDate.DayOfWeek == DayOfWeek.Saturday)
                backDate = backDate.AddDays(-1);

            LoadWorkinIssue(backDate, false);
        }
    }

    internal class WorkLogView : WorkLog.IView, IView<frmWorkLog>
    {
        private WorkLog.IModel Model;

        private frmWorkLog Form;

        [Inject]
        public WorkLogView(WorkLog.IModel inModel, IEventBroker inGlobalEvent)
        {
            Model = inModel;
            inGlobalEvent.Register(this);
        }

        [EventPublication(WorkLog.Events.Load, typeof(Publish<WorkLog.IView>))]
        public event EventHandler LoadEvent;

        public void Init(frmWorkLog inView)
        {
            Form = inView;
            LoadEvent.Fire(this);
        }
    }
}