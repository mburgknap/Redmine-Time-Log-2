using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
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
    //https://www.dropbox.com/s/y0zezwk6x51hcza/Version.cfg?dl=1
    public partial class frmMain : Form, ISetup
    {
        private IAppSettings settings;
        public frmMain()
        {
            InitializeComponent();
            this.Initialize<Main.IView, frmMain>();
            CheckForIllegalCrossThreadCalls = false;
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            CheckVersion();
            cHeader.SetDescription();


        }

        private void CheckVersion()
        {
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    var filename = "Version.cfg";

                    using (var client = new WebClient())
                    {
                        File.Delete(filename);
                        client.DownloadFile("https://raw.githubusercontent.com/mburgknap/Redmine-Time-Log/master/RedmineLog/Version.config", filename);

                        var version = File.ReadAllText(filename);

                        if (Assembly.GetExecutingAssembly().GetName().Version.CompareTo(new Version(version.Split(';')[0])) == -1)
                        {
                            bool result = false;

                            if (Boolean.TryParse(version.Split(';')[0], out result))
                            {
                                if (result)
                                {
                                    MessageBox.Show("New version availible " + version.Split(';')[0], "Information", MessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                if (MessageBox.Show("New version availible " + version.Split(';')[0]
                                                   + Environment.NewLine
                                                   + "Do you want download it ?", "Information", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    System.Diagnostics.Process.Start(version.Split(';')[1]);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            })).Start();
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

        [Inject]
        public MainView(Main.IModel inModel)
        {
            model = inModel;
            Program.Kernel.Get<AppTime.IClock>().Start();

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
            Form.lblClockIndle.Set(obj,
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
                list.Add(new IssueItemView().Set(issue));
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

        [EventPublication(Main.Events.AddComment, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> AddCommentEvent;

        [EventPublication(Main.Events.AddIssue, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> AddIssueEvent;

        [EventPublication(Main.Events.DelComment, typeof(Publish<Main.IView>))]
        public event EventHandler DelCommentEvent;

        [EventPublication(Main.Events.DelIssue, typeof(Publish<Main.IView>))]
        public event EventHandler DelIssueEvent;

        [EventPublication(Main.Events.Exit, typeof(Publish<Main.IView>))]
        public event EventHandler ExitEvent;

        [EventPublication(Main.Events.Link, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> GoLinkEvent;

        [EventPublication(Main.Events.Load, typeof(Publish<Main.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(Main.Events.Reset, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<Main.Actions>> ResetEvent;

        [EventPublication(Main.Events.Submit, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<Main.Actions>> SubmitEvent;

        [EventPublication(Main.Events.UpdateComment, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> UpdateCommentEvent;

        [EventPublication(Main.Events.SelectComment)]
        public event EventHandler<Args<CommentData>> SelectCommentEvent;

        [EventPublication(Main.Events.UpdateIssue, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> UpdateIssueEvent;

        [EventPublication(SubIssue.Events.SetSubIssue)]
        public event EventHandler<Args<int>> SetSubIssueEvent;

        [EventPublication(IssueLog.Events.Select)]
        public event EventHandler<Args<WorkingIssue>> SelectEvent;

        [EventPublication(Main.Events.IssueResolve)]
        public event EventHandler<Args<WorkingIssue>> ResolveEvent;

        [EventPublication(Main.Events.Update)]
        public event EventHandler UpdateEvent;

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
                AppLogger.Log.Error("GoLink", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
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

            activityTypeChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbActivity, "SelectedIndexChanged"));
            resolveChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbResolveIssue, "CheckedChanged"));
            resolveChanged.Subscribe(OnNotifyResolve);
            commentChanged.Build(Observable.FromEventPattern<EventArgs>(Form.tbComment, "TextChanged"));
            commentChanged.Subscribe(OnNotifyComment);

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
            Form.lblClockActive.Click += OnWorkMode;
            Form.lblClockIndle.Click += OnIdleMode;
            Form.lnkSettings.Click += OnSettingClick;
            Form.lnkIssues.Click += OnRedmineIssuesLink;
            Form.lblIssue.MouseClick += OnRedmineIssueLink;
            Form.tbComment.Click += OnCommentClick;
            Form.tsmMyWork.Click += OnWorkLogClick;
            Form.btnRemoveItem.Visible = false;
            Form.btnSubmit.Visible = false;
            Form.btnSubmitAll.Visible = false;
            Form.Resize += OnResize;
            Form.lblParentIssue.MouseClick += OnParentIssueMouseClick;
            Form.lblIssue.MouseClick += OnIssueMouseClick;


            Load();
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

            new frmProcessing().Show(Form,
                () =>
                {
                    OnWorkMode(this, EventArgs.Empty);
                    LoadEvent.Fire(this);
                });
        }

        [EventSubscription(AppTime.Events.TimeUpdate, typeof(OnPublisher))]
        public void OnTimeUpdateEvent(object sender, EventArgs arg)
        {
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
                    UpdateEvent.Fire(this, EventArgs.Empty);
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

        private void OnIdleMode(object sender, EventArgs e)
        {
            Form.pManage.BackColor = Color.Azure;
            currentMode = Main.Actions.Idle;
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
                });
        }

        private void OnSubmitClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                    SubmitEvent.Fire(this, currentMode);
                });
        }

        private void OnWorkLogClick(object sender, EventArgs e)
        {
            var form = new frmWorkLog();

            form.ShowDialog();
        }

        private void OnWorkMode(object sender, EventArgs e)
        {
            Form.pManage.BackColor = Color.Wheat;
            currentMode = Main.Actions.Issue;
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
    }
}