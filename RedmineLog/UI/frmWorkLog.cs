using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RedmineLog.UI.Items;
using RedmineLog.Common.Forms;

namespace RedmineLog
{
    public partial class frmWorkLog : Form, ISetup
    {
        private IAppSettings settings;
        public frmWorkLog()
        {
            InitializeComponent();
            this.Initialize<WorkLog.IView, frmWorkLog>();
            cHeader.SetDescription();
        }

        private void OnWorkLogLoad(object sender, EventArgs e)
        {
            this.SetupLocation(settings.Display, 0, -50);
        }

        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
        }
    }

    internal class WorkLogView : WorkLog.IView, IView<frmWorkLog>
    {
        private WorkLog.IModel model;

        private frmWorkLog Form;

        private frmEditTimeLog editform;

        [Inject]
        public WorkLogView(WorkLog.IModel inModel)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
        }

        [EventPublication(WorkLog.Events.Load, typeof(Publish<WorkLog.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(WorkLog.Events.LoadMore, typeof(Publish<WorkLog.IView>))]
        public event EventHandler LoadMoreEvent;

        [EventPublication(WorkLog.Events.Select)]
        public event EventHandler<Args<WorkLogItem>> SelectEvent;

        [EventPublication(WorkLog.Events.Edit)]
        public event EventHandler<Args<WorkLogItem>> EditEvent;

        [EventPublication(SubIssue.Events.SetSubIssue)]
        public event EventHandler<Args<int>> SetSubIssueEvent;

        private frmSubIssue addIssueForm;

        public void Init(frmWorkLog inView)
        {
            Form = inView;
            Form.blLoadMore.Click += OnLoadMore;
            Form.blLoadMore.Focus();
            Form.FormClosing += OnCloseForm;
            KeyHelpers.BindKey(Form, OnKeyDown);
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


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Form.Close();
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
            var list = new List<Control>();

            WorkLogGroupItemView dayItem = null;

            foreach (var workItem in (from p in model.WorkLogs
                                      group p by p.Date.Date into g
                                      select new { Date = g.Key, Issues = g.ToList() }))
            {

                var dayTime = new TimeSpan(0);

                dayItem = new WorkLogGroupItemView();
                dayItem.Set(workItem.Date);
                list.Add(dayItem);
                KeyHelpers.BindKey(dayItem, OnKeyDown);

                foreach (var parentIssue in (from p in workItem.Issues
                                             group p by p.ProjectName into g
                                             orderby g.Key ascending
                                             select new { Project = g.Key, Issues = g.ToList() }))
                {

                    var projectTime = new TimeSpan(0);
                    var projectItem = new WorkLogGroupItemView();
                    projectItem.Set(parentIssue.Project);
                    list.Add(projectItem);
                    KeyHelpers.BindKey(projectItem, OnKeyDown);

                    foreach (var issue in (from i in parentIssue.Issues
                                           group i by i.ParentIssue into g
                                           orderby (String.IsNullOrWhiteSpace(g.Key) ? "aa" : g.Key) ascending
                                           select new { ParentIssue = g.Key, Issues = g.ToList() }))
                    {
                        WorkLogGroupItemView parentIssueItem = null;
                        var parentIssueTime = new TimeSpan(0);

                        if (!string.IsNullOrWhiteSpace(issue.ParentIssue))
                        {
                            parentIssueItem = new WorkLogGroupItemView();
                            parentIssueItem.Set(issue.ParentIssue);
                            list.Add(parentIssueItem);
                        }

                        foreach (var subIssue in issue.Issues.OrderBy(x => x.Id))
                        {
                            projectTime = projectTime.Add(new TimeSpan(0, (int)(subIssue.Hours * 60), 0));
                            parentIssueTime = parentIssueTime.Add(new TimeSpan(0, (int)(subIssue.Hours * 60), 0));

                            list.Add(new WorkLogItemView().Set(subIssue));
                            KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);
                            KeyHelpers.BindMouseClick(list[list.Count - 1], OnClick);
                            KeyHelpers.BindSpecialClick(list[list.Count - 1], OnSpecialClick);
                        }

                        if (parentIssueItem != null)
                            parentIssueItem.Update(parentIssueTime);
                    }

                    dayTime = dayTime.Add(projectTime);
                    projectItem.Update(projectTime);
                }

                dayItem.Update(dayTime);
            }

            Form.fpWokLogList.Set(model,
                       (ui, data) =>
                       {
                           ui.Controls.Clear();
                           ui.Controls.AddRange(list.ToArray());
                           ui.ScrollControlIntoView(dayItem);
                       });
        }

        private void OnSpecialClick(string action, object data)
        {
            if (action == "Select" && data is ICustomItem)
            {
                SelectIssue(((ICustomItem)data).Data as WorkLogItem);
                return;
            }

            if (action == "Edit" && data is Control && data is ICustomItem)
            {
                if (editform == null)
                {
                    editform = new frmEditTimeLog();
                    editform.FormClosed += (s, arg) =>
                    {
                        editform = null;
                    };
                    editform.Shown += (s, e) =>
                    {
                        EditEvent.Fire(this, ((ICustomItem)data).Data as WorkLogItem);
                    };
                    editform.Show();
                    return;
                }

                EditEvent.Fire(this, ((ICustomItem)data).Data as WorkLogItem);
                return;
            }

            if (action == "AddSubIssue" && data is ICustomItem)
            {
                addIssueForm = new frmSubIssue();
                SetSubIssueEvent.Fire(this, (((ICustomItem)data).Data as WorkLogItem).IdIssue);
                addIssueForm.ShowDialog();
                return;
            }
        }

        private void OnClick(object sender)
        {
            if (sender is ICustomItem)
            {
                SelectIssue(((ICustomItem)sender).Data as WorkLogItem);
                return;
            }

            if (sender is Control)
            {
                OnClick(((Control)sender).Parent);
                return;
            }
        }

    }
}