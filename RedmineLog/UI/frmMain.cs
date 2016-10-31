using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.Common.Interfaces;
using RedmineLog.Properties;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using RedmineLog.UI.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmMain : Form, ISetup
    {
        private IAppSettings settings;
        public frmMain()
        {
            InitializeComponent();
            this.Initialize<Main.IView, frmMain>();
            CheckForIllegalCrossThreadCalls = false;
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            // CheckVersion();
            cHeader.SetDescription();


        }

        private void CheckVersion()
        {
            Task.Run(() =>
            {

                try
                {
                    var filename = "version.txt";

                    using (var client = new WebClient())
                    {
                        File.Delete(filename);
                        client.DownloadFile("http://docs.google.com/uc?id=0BwoXOcOLMhp3VHVXQXlXdWU2TkE", filename);

                        var data = File.ReadAllText(filename).Split(';');

                        var onlineVersion = new Version(data[0]);
                        var url = data[1];

                        var appVersion = this.GetType().Assembly.GetName().Version;

                        if (appVersion.CompareTo(onlineVersion) < 0)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    if (MessageBox.Show("New version availible " + onlineVersion
                                               + Environment.NewLine
                                               + "Do you want download it ?", "RedmineLog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        Task.Run(() =>
                                        {
                                            try
                                            {
                                                var file = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RedmineLog.exe"));
                                                if (file.Exists)
                                                    file.Delete();

                                                client.DownloadFile(url, file.FullName);

                                                Process.Start(file.FullName);
                                            }
                                            catch (Exception ex)
                                            {
                                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                            }
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                }
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        private void OnMainLoad(object sender, EventArgs e)
        {
            this.SetupLocation(settings.Display, 0, -50);
        }

        private void btnBugs_Click(object sender, EventArgs e)
        {
            cmIssuesKind.Show(btnIssueMode, new Point(0, 0));
        }


        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmTemplateIssue.Show(btnTemplateIssue, new Point(0, 0));
        }

    }

    internal class MainView : Main.IView, IView<frmMain>
    {

        class ExContextMenuSubIssue : ContextMenuStrip
        {
            private RedmineIssueData item;
            private Action<string, RedmineIssueData> data;
            public ExContextMenuSubIssue()
            {
                Items.Add(new ToolStripMenuItem("Add subtask", Resources.Add, (s, e) => { data("AddSubIssue", item); }));
                Items.Add(new ToolStripMenuItem("Resolve", Resources.Resolve, (s, e) => { data("ResolveIssue", item); }));
            }

            public void Set(RedmineIssueData inItem, Action<string, object> inData)
            {
                item = inItem;
                data = inData;
            }
        }
        class ExContextMenuIssue : ContextMenuStrip
        {
            private RedmineIssueData item;
            private Action<string, RedmineIssueData> data;
            public ExContextMenuIssue()
            {
                Items.Add(new ToolStripMenuItem("Add subtask", Resources.Add, (s, e) => { data("AddSubIssue", item); }));
            }

            public void Set(RedmineIssueData inItem, Action<string, object> inData)
            {
                item = inItem;
                data = inData;
            }
        }

        static ExContextMenuSubIssue menuSubIssue = new ExContextMenuSubIssue();

        static ExContextMenuIssue menuIssue = new ExContextMenuIssue();

        private Main.Actions currentMode;
        private frmMain Form;
        private Main.IModel model;
        private IUpdater updater;

        [Inject]
        public MainView(Main.IModel inModel, IUpdater inUpdater)
        {
            model = inModel;
            updater = inUpdater;

            model.Activity.OnUpdate.Subscribe(OnUpdateActivity);
            model.Comment.OnUpdate.Subscribe(OnUpdateComment);
            model.IdleTime.OnUpdate.Subscribe(OnUpdateIdleTime);
            model.Issue.OnUpdate.Subscribe(OnUpdateIssue);
            model.IssueComments.OnUpdate.Subscribe(OnUpdateIssueComments);
            model.IssueInfo.OnUpdate.Subscribe(OnUpdateIssueInfo);
            model.IssueParentInfo.OnUpdate.Subscribe(OnUpdateIssueParentInfo);
            model.Resolve.OnUpdate.Subscribe(OnUpdateResolve);
            model.StartTime.OnUpdate.Subscribe(OnUpdateStartTime);
            model.WorkTime.OnUpdate.Subscribe(OnUpdateWorkTime);
            model.LastIssues.OnUpdate.Subscribe(OnUpdateLastIssues);
            model.WorkActivities.OnUpdate.Subscribe(OnUpdateWorkActivities);
            model.WorkReport.OnUpdate.Subscribe(OnUpdateWorkReport);
        }


        private void OnUpdateWorkReport(WorkReportData obj)
        {
            Form.Set(obj,
                  (ui, data) =>
                  {
                      int workBuffer = 1;

                      TimeSpan summary = new TimeSpan(0);

                      Action<LinkLabel, TimeSpan, bool, bool> action = (label, time, isFutureDay, isFreeDay) =>
                      {
                          summary = summary + time;

                          if (isFreeDay)
                          {
                              if (time.TotalMinutes > 0)
                                  label.BackColor = Color.Green;
                              else
                                  label.BackColor = isFreeDay ? Color.LightGray : SystemColors.ActiveCaption;
                          }
                          else
                              if (model.WorkReport.Value.ReportType == WorkReportType.LastWeek || !isFutureDay)
                              {
                                  if (time.Hours >= model.WorkReport.Value.MinimalHours)
                                      label.BackColor = Color.Green;
                                  else if (time.Hours >= model.WorkReport.Value.MinimalHours - workBuffer)
                                      label.BackColor = Color.Yellow;
                                  else
                                      label.BackColor = Color.Red;
                              }
                              else
                                  label.BackColor = isFreeDay ? Color.LightGray : SystemColors.ActiveCaption;

                          label.Text = time.ToWorkTime();
                      };

                      if (model.WorkReport.Value.ReportType == WorkReportType.Week)
                          ui.btnWorkReportMode.BackColor = Color.Green;
                      else
                          ui.btnWorkReportMode.BackColor = Color.Gray;

                      action(ui.llDay1, obj.Day1, DateTime.Now.DayOfWeek <= DayOfWeek.Monday, false);
                      action(ui.llDay2, obj.Day2, DateTime.Now.DayOfWeek <= DayOfWeek.Tuesday, false);
                      action(ui.llDay3, obj.Day3, DateTime.Now.DayOfWeek <= DayOfWeek.Wednesday, false);
                      action(ui.llDay4, obj.Day4, DateTime.Now.DayOfWeek <= DayOfWeek.Thursday, false);
                      action(ui.llDay5, obj.Day5, DateTime.Now.DayOfWeek <= DayOfWeek.Friday, false);
                      action(ui.llDay6, obj.Day6, false, true);
                      action(ui.llDay7, obj.Day7, false, true);

                      if (summary.WorkHours() >= 5 * obj.MinimalHours)
                          ui.llSummaryTime.BackColor = Color.Green;
                      else if (summary.WorkHours() >= (5 * obj.MinimalHours) - (2 * workBuffer))
                          ui.llSummaryTime.BackColor = Color.Yellow;
                      else
                          ui.llSummaryTime.BackColor = SystemColors.ActiveCaption;

                      ui.llSummaryTime.Text = summary.ToWorkTime();

                  });
        }

        private void OnUpdateActivity(WorkActivityType obj)
        {
            Form.cbActivity.Set(obj,
                (ui, data) =>
                {
                    activityTypeChanged.Dispose();
                    ui.SelectedItem = ui.Items[model.WorkActivities.Value.IndexOf(obj)];
                    activityTypeChanged.Subscribe(Observer.Create<EventPattern<EventArgs>>(OnNotifyActivity));
                });
        }

        private void OnNotifyActivity(EventPattern<EventArgs> obj)
        {
            if (model.WorkActivities.Value.Count > Form.cbActivity.SelectedIndex)
            {
                model.Activity.Notify(model.WorkActivities.Value[Form.cbActivity.SelectedIndex]);
            };
        }


        private void OnUpdateComment(CommentData obj)
        {
            Form.tbComment.Set(obj,
                 (ui, data) =>
                 {
                     ui.Text = data != null ? data.Text : string.Empty;
                     ui.ReadOnly = data == null;
                 });

            EnableSmallMode();
        }

        private void OnUpdateIdleTime(TimeSpan obj)
        {
            Form.lblClockIdle.Set(obj,
               (ui, data) =>
               {
                   ui.Text = data.ToString();
               });
        }

        private void OnUpdateIssue(IssueData obj)
        {
            System.Diagnostics.Debug.WriteLine("OnUpdateIssue Not Supported");
        }

        private void OnUpdateIssueComments(IssueCommentList obj)
        {
            System.Diagnostics.Debug.WriteLine("OnUpdateIssueComments Not Supported");
        }

        private void OnUpdateIssueInfo(RedmineIssueData obj)
        {
            Form.Set(obj,
                (ui, data) =>
                {
                    ui.tbIssue.Text = data.Id > 0 ? data.Id.ToString() : "";

                    ui.lblProject.Text = data.Project;
                    ui.lblTracker.Text = data.Id > 0 ? "(" + data.Tracker + ")" : "";
                    ui.lblIssue.Text = data.Subject;

                    ui.btnRemoveItem.Visible = data.Id > 0;
                    ui.btnSubmit.Visible = data.Id > 0;
                    ui.btnSubmitAll.Visible = data.Id > 0;
                });

            EnableSmallMode();
        }

        private void OnUpdateIssueParentInfo(RedmineIssueData obj)
        {
            Form.lblParentIssue.Set(obj,
              (ui, data) =>
              {
                  if (data != null)
                  {
                      ui.Text = data.Subject + " :";
                      ui.Visible = true;
                  }
                  else
                      ui.Visible = false;
              });
        }

        private void OnUpdateResolve(bool obj)
        {
            Form.cbResolveIssue.Set(obj,
                  (ui, data) =>
                  {
                      resolveChanged.Dispose();
                      ui.Checked = data;
                      resolveChanged.Subscribe(OnNotifyResolve);
                  });
        }

        private void OnNotifyResolve(EventPattern<EventArgs> obj)
        {
            model.Resolve.Notify(Form.cbResolveIssue.Checked);
        }

        private void OnUpdateStartTime(DateTime obj)
        {
            Form.lbClockTodayTime.Set(model.StartTime,
                  (ui, data) =>
                  {
                      ui.Text = (DateTime.Now - data.Value).ToString(@"hh\:mm\:ss");

                      if (string.IsNullOrWhiteSpace(Form.ttStartTime.ToolTipTitle))
                          Form.ttStartTime.SetToolTip(ui, data.Value.ToString(@"HH\:mm\:ss"));
                  });
        }

        private void OnUpdateWorkTime(TimeSpan obj)
        {
            Form.lblClockActive.Set(obj,
              (ui, data) =>
              {
                  ui.Text = data.ToString();
              });
        }

        private void OnUpdateWorkActivities(WorkActivityList value)
        {
            Form.cbActivity.Set(model,
               (ui, data) =>
               {
                   ui.BeginUpdate();

                   ui.Items.Clear();
                   ui.DataSource = data.WorkActivities.Value;
                   ui.DisplayMember = "Name";
                   ui.ValueMember = "Id";

                   if (ui.Items.Count > 0)
                   {
                       activityTypeChanged.Dispose();
                       ui.SelectedItem = ui.Items[0];
                       activityTypeChanged.Subscribe(Observer.Create<EventPattern<EventArgs>>(OnNotifyActivity));
                       data.Activity.Notify(data.WorkActivities.Value[0]);
                   }
                   ui.EndUpdate();
               });
        }

        private void OnUpdateLastIssues(WorkingIssueList value)
        {
            var list = new List<Control>();

            foreach (var issue in value)
            {
                list.Add(Program.Kernel.Get<IssueItemView>().Set(issue));
                KeyHelpers.BindMouseClick(list[list.Count - 1], OnClick);
                KeyHelpers.BindSpecialClick(list[list.Count - 1], OnSpecialClick2);
            }

            Form.fpIssueList.Set(model,
             (ui, data) =>
             {
                 ui.Controls.Clear();
                 ui.Controls.AddRange(list.ToArray());
             });
        }

        [EventPublication(Main.Events.AddComment)]
        public event EventHandler<Args<string>> AddCommentEvent;

        [EventPublication(Main.Events.AddIssue)]
        public event EventHandler<Args<string>> AddIssueEvent;

        [EventPublication(Main.Events.DelComment)]
        public event EventHandler DelCommentEvent;

        [EventPublication(Main.Events.DelIssue)]
        public event EventHandler DelIssueEvent;

        [EventPublication(Main.Events.Exit)]
        public event EventHandler ExitEvent;

        [EventPublication(Main.Events.Link)]
        public event EventHandler<Args<string>> GoLinkEvent;

        [EventPublication(Main.Events.Load)]
        public event EventHandler LoadEvent;

        [EventPublication(Main.Events.Reset)]
        public event EventHandler<Args<Main.Actions>> ResetEvent;

        [EventPublication(Main.Events.Submit)]
        public event EventHandler<Args<Main.Actions>> SubmitEvent;

        [EventPublication(Main.Events.UpdateComment)]
        public event EventHandler<Args<string>> UpdateCommentEvent;

        [EventPublication(Main.Events.SelectComment)]
        public event EventHandler<Args<CommentData>> SelectCommentEvent;

        [EventPublication(AppTime.Events.SetupClock)]
        public event EventHandler<Args<AppTime.ClockMode>> SetupClockEvent;

        [EventPublication(SubIssue.Events.SetSubIssue)]
        public event EventHandler<Args<int>> SetSubIssueEvent;

        [EventPublication(IssueLog.Events.Select)]
        public event EventHandler<Args<WorkingIssue>> SelectEvent;

        [EventPublication(Main.Events.IssueResolve)]
        public event EventHandler<Args<WorkingIssue>> ResolveEvent;

        [EventPublication(Main.Events.Update)]
        public event EventHandler UpdateEvent;

        [EventPublication(Main.Events.WorkReportSync)]
        public event EventHandler WorkReportSyncEvent;

        [EventPublication(Main.Events.WorkReportMode)]
        public event EventHandler WorkReportModeEvent;

        [EventPublication(IssueLog.Events.Delete)]
        public event EventHandler<Args<WorkingIssue>> DeleteEvent;



        private frmSmall smallForm;
        private frmSubIssue addIssueForm;


        public void GoLink(Uri inUri)
        {
            try
            {
                System.Diagnostics.Process.Start(inUri.ToString());
            }
            catch (Exception ex)
            {
                Program.Kernel.Get<ILog>().Error("GoLink", ex, "Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        public void Info(string inMessage)
        {
            MessageBox.Show(inMessage);
        }

        EventProperty<EventArgs> activityTypeChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> resolveChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> commentChanged = new EventProperty<EventArgs>();

        public void Init(frmMain inView)
        {
            Form = inView;
            StartClock();

            activityTypeChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbActivity, "SelectedIndexChanged"));

            resolveChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbResolveIssue, "CheckedChanged"));
            resolveChanged.Subscribe(OnNotifyResolve);
            commentChanged.Build(Observable.FromEventPattern<EventArgs>(Form.tbComment, "TextChanged"));
            commentChanged.Subscribe(OnNotifyComment);

            Observable.FromEventPattern<EventArgs>(Form.btnStopWork, "Click").Subscribe(OnActionSetupClock);
            Observable.FromEventPattern<EventArgs>(Form.lblClockIdle, "Click").Subscribe(OnActionIdleMode);
            Observable.FromEventPattern<EventArgs>(Form.lblClockActive, "Click").Subscribe(OnActionWorkMode);
            Observable.FromEventPattern<EventArgs>(Form.btnWorkReportSync, "Click").Subscribe(OnActionWorkReportSync);
            Observable.FromEventPattern<EventArgs>(Form.btnWorkReportMode, "Click").Subscribe(OnActionWorkReportMode);

            Observable.FromEventPattern<LinkLabelLinkClickedEventArgs>(Form.llDay1, "LinkClicked").Subscribe(obj => OnActionDayWork(DayOfWeek.Monday));
            Observable.FromEventPattern<LinkLabelLinkClickedEventArgs>(Form.llDay2, "LinkClicked").Subscribe(obj => OnActionDayWork(DayOfWeek.Tuesday));
            Observable.FromEventPattern<LinkLabelLinkClickedEventArgs>(Form.llDay3, "LinkClicked").Subscribe(obj => OnActionDayWork(DayOfWeek.Wednesday));
            Observable.FromEventPattern<LinkLabelLinkClickedEventArgs>(Form.llDay4, "LinkClicked").Subscribe(obj => OnActionDayWork(DayOfWeek.Thursday));
            Observable.FromEventPattern<LinkLabelLinkClickedEventArgs>(Form.llDay5, "LinkClicked").Subscribe(obj => OnActionDayWork(DayOfWeek.Friday));
            Observable.FromEventPattern<LinkLabelLinkClickedEventArgs>(Form.llDay6, "LinkClicked").Subscribe(obj => OnActionDayWork(DayOfWeek.Saturday));
            Observable.FromEventPattern<LinkLabelLinkClickedEventArgs>(Form.llDay7, "LinkClicked").Subscribe(obj => OnActionDayWork(DayOfWeek.Sunday));
            Observable.FromEventPattern<LinkLabelLinkClickedEventArgs>(Form.llSummaryTime, "LinkClicked").Subscribe(obj => OnActionDayWorkSummary());

            KeyHelpers.BindMouseClick(Form.cHeader, OnClick);
            Form.tsmMyBugs.Click += SearchBugClick;
            Form.tbIssue.KeyDown += SaveIssue;
            Form.btnRemoveItem.Click += DelIssue;
            Form.btnComments.Click += LoadComment;
            Form.btnNewComment.Click += AddComment;
            Form.btnRemoveComment.Click += DelComment;
            Form.tbComment.KeyDown += SaveComment;
            Form.cmComments.ItemClicked += SelectComment;
            Form.tsmMyIssues.Click += OnSearchIssueClick;
            Form.btnSubmit.Click += OnSubmitClick;
            Form.btnSubmitAll.Click += OnSubmitAllClick;
            Form.btnResetIdle.Click += OnResetClockClick;
            Form.lHide.Click += OnHideClick;
            Form.lnkExit.Click += OnExitClick;
            Form.lnkSettings.Click += OnSettingClick;
            Form.lnkIssues.Click += OnRedmineIssuesLink;
            Form.lblIssue.MouseClick += OnRedmineIssueLink;
            Form.tbComment.Click += OnCommentClick;
            Form.tsmMyWork.Click += OnWorkLogClick;
            Form.btnSearch.Click += OnIssueSearch;
            Form.btnRemoveItem.Visible = false;
            Form.btnSubmit.Visible = false;
            Form.btnSubmitAll.Visible = false;
            Form.Resize += OnResize;
            Form.lblParentIssue.MouseClick += OnParentIssueMouseClick;
            Form.lblIssue.MouseClick += OnIssueMouseClick;
            Form.lblVersion.Click += OnVersionClick;
            Load();
        }

        void OnVersionClick(object sender, EventArgs e)
        {
            updater.CheckVersion();

            var form = new frmAbout();
            form.ShowDialog();
        }

        private void OnIssueSearch(object sender, EventArgs e)
        {
            var form = new frmSearch();
            form.ShowDialog();
        }

        private void OnActionDayWorkSummary()
        {
            try
            {
                System.Diagnostics.Process.Start(model.WorkReport.Value.UriSummary(model.WorkReport.Value.ReportType));
            }
            catch (Exception ex)
            {
                Program.Kernel.Get<ILog>().Error("OnActionDayWork", ex, "Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void OnActionDayWork(DayOfWeek inDay)
        {
            try
            {
                System.Diagnostics.Process.Start(model.WorkReport.Value.Uri(inDay));
            }
            catch (Exception ex)
            {
                Program.Kernel.Get<ILog>().Error("OnActionDayWork", ex, "Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void OnActionWorkReportMode(EventPattern<EventArgs> obj)
        {
            new frmProcessing().Show(Form, Form.tableLayoutPanel1,
                       () =>
                       {
                           WorkReportModeEvent.Fire(this);
                       });
        }

        private void OnActionWorkReportSync(EventPattern<EventArgs> obj)
        {
            new frmProcessing().Show(Form, Form.tableLayoutPanel1,
                        () =>
                        {
                            WorkReportSyncEvent.Fire(this);
                        });
        }

        private void OnActionWorkMode(EventPattern<EventArgs> obj)
        {
            Form.pManage.BackColor = Color.Wheat;
            currentMode = Main.Actions.Issue;
        }

        private void OnActionIdleMode(EventPattern<EventArgs> obj)
        {
            Form.pManage.BackColor = Color.LightBlue;
            currentMode = Main.Actions.Idle;
        }

        private void StartClock()
        {
            Form.btnStopWork.Tag = AppTime.ClockMode.Standard;
            SetupClockEvent.Fire(this, AppTime.ClockMode.Standard);
            Program.Kernel.Get<AppTime.IClock>().Start();
        }

        private void OnActionSetupClock(EventPattern<EventArgs> obj)
        {
            Form.btnStopWork.Tag.Is<AppTime.ClockMode>(x =>
            {
                switch (x)
                {
                    case AppTime.ClockMode.Standard:
                        {
                            Form.btnStopWork.Tag = AppTime.ClockMode.AlwaysIdle;
                            Form.btnStopWork.Text = "Idle";
                            OnActionIdleMode(obj);
                            break;
                        }
                    case AppTime.ClockMode.AlwaysIdle:
                        {
                            Form.btnStopWork.Tag = AppTime.ClockMode.Standard;
                            Form.btnStopWork.Text = "Stop";
                            OnActionWorkMode(obj);
                            break;
                        }
                }

                SetupClockEvent.Fire(this, (AppTime.ClockMode)Form.btnStopWork.Tag);
            });

        }

        private void OnNotifyComment(EventPattern<EventArgs> obj)
        {
            if (model.Comment.Value != null)
            {
                model.Comment.Value.Text = Form.tbComment.Text;
                model.Comment.Notify();
            }
        }


        void OnIssueMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menuSubIssue.Set(model.IssueInfo.Value, OnSpecialClick);
                menuSubIssue.Show(Form.lblIssue, new Point(0, 0));
            }
        }

        void OnParentIssueMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menuIssue.Set(model.IssueParentInfo.Value, OnSpecialClick);
                menuIssue.Show(Form.lblParentIssue, new Point(0, 0));
            }
        }
        private void OnSpecialClick(string action, object data)
        {
            if (action == "AddSubIssue" && data is RedmineIssueData)
            {
                UpdateEvent.Fire(this, EventArgs.Empty);
                addIssueForm = new frmSubIssue();
                SetSubIssueEvent.Fire(this, ((RedmineIssueData)data).Id);
                addIssueForm.ShowDialog();

            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (Form.WindowState == FormWindowState.Minimized)
            {
                smallForm = new frmSmall();

                smallForm.FormClosed += (s, arg) =>
                {
                    Form.WindowState = FormWindowState.Normal;
                };

                smallForm.ShowDialog();
            }
            else
            {
                Form.WindowState = FormWindowState.Normal;

                if (smallForm != null)
                {
                    smallForm.Close();
                    smallForm = null;
                }
            }
        }



        private void SearchBugClick(object sender, EventArgs e)
        {
            UpdateEvent.Fire(this, EventArgs.Empty);
            model.LastIssues.Update();
            new frmBugLog().ShowDialog();
        }

        private void SaveIssue(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                new frmProcessing().Show(Form,
                     () =>
                     {
                         UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                         AddIssueEvent.Fire(this, Form.tbIssue.Text);
                     });
            }

            if (e.Control && e.KeyCode == Keys.S)
            {
                new frmProcessing().Show(Form,
                       () =>
                       {
                           UpdateEvent.Fire(this, EventArgs.Empty);
                           model.LastIssues.Update();
                       });
            }
        }

        public void Load()
        {
            updater.CheckVersion();
            new frmProcessing().Show(Form,
                () =>
                {
                    OnActionWorkMode(null);
                    LoadEvent.Fire(this);
                }, () =>
                {
                    OnActionWorkReportSync(null);
                });
        }

        [EventSubscription(AppTime.Events.TimeUpdate, typeof(OnPublisher))]
        public void OnTimeUpdateEvent(object sender, EventArgs arg)
        {
            model.StartTime.Update();
        }

        [EventSubscription(AppTime.Events.IdleUpdate, typeof(OnPublisher))]
        public void OnIdleUpdateEvent(object sender, Args<int> arg)
        {
            model.IdleTime.Notify(model.IdleTime.Value.Add(new TimeSpan(0, 0, arg.Data)));
            model.IdleTime.Update();
        }

        [EventSubscription(AppTime.Events.WorkUpdate, typeof(OnPublisher))]
        public void OnWorkUpdateEvent(object sender, Args<int> arg)
        {
            model.WorkTime.Notify(model.WorkTime.Value.Add(new TimeSpan(0, 0, arg.Data)));
            model.WorkTime.Update();
        }

        private void AddComment(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    AddCommentEvent.Fire(this, string.Empty);
                });
        }

        private void DelComment(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    DelCommentEvent.Fire(this);
                });
        }

        private void DelIssue(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    DelIssueEvent.Fire(this);
                });
        }

        private void EnableSmallMode()
        {
            Form.lHide.Set(model,
              (ui, data) =>
              {
                  ui.Visible = data.IssueInfo.Value.Id > 0 || data.Comment != null;
              });
        }

        private void LoadComment(object sender, EventArgs e)
        {
            Form.cmComments.Items.Clear();

            var list = model.IssueComments.Value.Where(x => !x.IsGlobal).ToList();

            if (list.Count > 0)
            {
                Form.cmComments.Items.Add(new ToolStripSeparator());
                var tmpStrip = new ToolStripStatusLabel("Issue comments");
                tmpStrip.Font = new Font(tmpStrip.Font, FontStyle.Bold);
                Form.cmComments.Items.Add(tmpStrip);
                Form.cmComments.Items.Add(new ToolStripSeparator());
            }

            foreach (var item in list)
            {
                var cmItem = Form.cmComments.Items.Add(item.Text);
                cmItem.Overflow = ToolStripItemOverflow.AsNeeded;
                cmItem.Tag = item;
            }

            list = model.IssueComments.Value.Where(x => x.IsGlobal).ToList();

            if (list.Count > 0)
            {
                Form.cmComments.Items.Add(new ToolStripSeparator());

                var tmpStrip = new ToolStripStatusLabel("Global comments");
                tmpStrip.Font = new Font(tmpStrip.Font, FontStyle.Bold);
                Form.cmComments.Items.Add(tmpStrip);
                Form.cmComments.Items.Add(new ToolStripSeparator());
            }

            foreach (var item in list)
            {
                var cmItem = Form.cmComments.Items.Add(item.Text);
                cmItem.Overflow = ToolStripItemOverflow.AsNeeded;
                cmItem.Tag = item;
            }
            if (Form.cmComments.Items.Count > 0)
                Form.cmComments.Show(Form.tbComment, 0, 0);

        }

        private void OnCommentClick(object sender, EventArgs e)
        {
            if (Form.tbComment.ReadOnly)
            {
                LoadComment(Form.btnComments, EventArgs.Empty);
            }
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    UpdateEvent.Fire(this);
                    ExitEvent.Fire(this);
                }, () =>
                {
                    Form.Close();
                });
        }

        private void OnHideClick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                UpdateEvent.Fire(this, EventArgs.Empty);
                model.LastIssues.Update();
            });

            Form.WindowState = FormWindowState.Minimized;
        }

        private void OnRedmineIssueLink(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                GoLinkEvent.Fire(this, "Issue");
        }

        private void OnRedmineIssuesLink(object sender, EventArgs e)
        {
            GoLinkEvent.Fire(this, "Redmine");
        }

        private void OnResetClockClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    ResetEvent.Fire(this, currentMode);
                });
        }

        private void OnSearchIssueClick(object sender, EventArgs e)
        {
            UpdateEvent.Fire(this, EventArgs.Empty);
            model.LastIssues.Update();
            new frmIssueLog().ShowDialog();
        }

        private void OnSettingClick(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
        }

        private void OnSubmitAllClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                    SubmitEvent.Fire(this, Main.Actions.All);
                }, () =>
                {
                    OnActionWorkReportSync(null);
                });
        }

        private void OnSubmitClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                    SubmitEvent.Fire(this, currentMode);
                }, () =>
                {
                    OnActionWorkReportSync(null);
                });
        }

        private void OnWorkLogClick(object sender, EventArgs e)
        {
            var form = new frmWorkLog();

            form.ShowDialog();
        }


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Form.Close();
                return;
            }
        }

        private void OnClick(object sender)
        {
            if (sender is ICustomItem)
            {
                UpdateEvent.Fire(this, EventArgs.Empty);

                var tmp = ((ICustomItem)sender).Data as WorkingIssue;

                if (tmp != null)
                    SelectIssue(tmp);
                else
                {
                    Form.tbIssue.Text = string.Empty;
                    new frmProcessing().Show(Form,
                       () =>
                       {
                           AddIssueEvent.Fire(this, string.Empty);
                       });

                }

                return;
            }

            if (sender is Control)
            {
                OnClick(((Control)sender).Parent);
                return;
            }
        }
        private void OnSpecialClick2(string action, object data)
        {
            if (action == "Select" && data is ICustomItem)
            {
                SelectIssue(((ICustomItem)data).Data as WorkingIssue);
                return;
            }

            if (action == "Resolve" && data is Control && data is ICustomItem)
            {
                Form.fpIssueList.Controls.Remove((Control)data);
                ResolveEvent.Fire(this, ((ICustomItem)data).Data as WorkingIssue);
                return;
            }

            if (action == "Delete" && data is Control && data is ICustomItem)
            {
                Form.fpIssueList.Controls.Remove((Control)data);
                DeleteEvent.Fire(this, ((ICustomItem)data).Data as WorkingIssue);
                return;
            }

            if (action == "AddSubIssue" && data is ICustomItem)
            {
                addIssueForm = new frmSubIssue();
                SetSubIssueEvent.Fire(this, (((ICustomItem)data).Data as WorkingIssue).Issue.Id);
                addIssueForm.ShowDialog();
                return;
            }

        }
        private void SelectIssue(WorkingIssue item)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    if (item != null)
                        SelectEvent.Fire(this, item);
                });
        }

        private void SaveComment(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                new frmProcessing().Show(Form,
                    () =>
                    {
                        UpdateEvent.Fire(this, EventArgs.Empty);
                        model.LastIssues.Update();
                    });
            }
        }

        private void SelectComment(object sender, ToolStripItemClickedEventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    UpdateCommentEvent.Fire(this, Form.tbComment.Text);

                    e.ClickedItem.Tag.Is<CommentData>(obj =>
                    {
                        if (obj.IsGlobal && !model.Issue.Value.IsGlobal())
                            AddCommentEvent.Fire(this, obj.Text);
                        else
                            SelectCommentEvent.Fire(this, obj);
                    });

                });
        }


        public void Restart()
        {
            Program.Restart = true;
            Form.Close();
        }
    }
}