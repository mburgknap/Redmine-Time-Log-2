using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Properties;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace RedmineLog
{
    //https://www.dropbox.com/s/y0zezwk6x51hcza/Version.cfg?dl=1
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            this.Initialize<Main.IView, frmMain>();
            CheckForIllegalCrossThreadCalls = false;
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();

            CheckVersion();


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
                        client.DownloadFile("https://www.dropbox.com/s/y0zezwk6x51hcza/Version.cfg?dl=1", filename);

                        var version = File.ReadAllText(filename);

                        if (!Assembly.GetExecutingAssembly().GetName().Version.Equals(new Version(version.Split(';')[0])))
                        {
                            if (MessageBox.Show("New version availible " + version.Split(';')[0]
                                                + Environment.NewLine
                                                + "Do you want download it ? ", "Information", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(version.Split(';')[1]);
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
            if (SystemInformation.VirtualScreen.Location.X < 0)
                this.Location = new Point(0 - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
            else
                this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }

        private void btnBugs_Click(object sender, EventArgs e)
        {
            cmIssuesKind.Show(btnIssueMode, new Point(0, 0));
        }

    }

    internal class MainView : Main.IView, IView<frmMain>
    {

        class ExContextMenu : ContextMenuStrip
        {
            private RedmineIssueData item;
            private Action<string, RedmineIssueData> data;
            public ExContextMenu()
            {
                Items.Add(new ToolStripMenuItem("Add Sub Issue", Resources.Add, (s, e) => { data("AddSubIssue", item); }));
            }

            public void Set(RedmineIssueData inItem, Action<string, object> inData)
            {
                item = inItem;
                data = inData;
            }
        }

        static ExContextMenu menu = new ExContextMenu();

        private Main.Actions currentMode;
        private frmMain Form;
        private Main.IModel model;
        [Inject]
        public MainView(Main.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            inGlobalEvent.Register(this);
            model.Sync.Bind(SyncTarget.View, this);
            AppTimers.Init(inGlobalEvent);
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

        [EventPublication(Main.Events.UpdateIssue, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> UpdateIssueEvent;

        [EventPublication(Main.Events.AddSubIssue, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<RedmineIssueData>> AddSubIssueEvent;

        private frmSmall smallForm;

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

        public void Init(frmMain inView)
        {
            Form = inView;
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
            Form.cbActivity.SelectedIndexChanged += OnActivityTypeChange;
            Form.cbResolveIssue.CheckedChanged += OnResolveIssueChange;
            Form.lnkSettings.Click += OnSettingClick;
            Form.lnkIssues.Click += OnRedmineIssuesLink;
            Form.lblIssue.MouseClick += OnRedmineIssueLink;
            Form.tbComment.Click += OnCommentClick;
            Form.tsmMyWork.Click += OnWorkLogClick;
            Form.btnRemoveItem.Visible = false;
            Form.btnSubmit.Visible = false;
            Form.btnSubmitAll.Visible = false;
            Form.Resize += OnResize;
            AppTimers.Start();
            Form.lblParentIssue.MouseClick += OnParentIssueMouseClick;
            Form.lblIssue.MouseClick += OnIssueMouseClick;
            Load();
        }

        void OnIssueMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menu.Set(model.IssueInfo, OnSpecialClick);
                menu.Show(Form.lblIssue, new Point(0, 0));
            }
        }

        void OnParentIssueMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menu.Set(model.IssueParentInfo, OnSpecialClick);
                menu.Show(Form.lblParentIssue, new Point(0, 0));
            }
        }
        private void OnSpecialClick(string action, object data)
        {
            if (action == "AddSubIssue" && data is RedmineIssueData)
            {
                new frmProcessing().Show(Form,
                        () =>
                        {
                            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                            UpdateIssueEvent.Fire(this, Form.tbIssue.Text);
                            AddSubIssueEvent.Fire(this, (RedmineIssueData)data);
                        });
            }
        }

        private void OnResolveIssueChange(object sender, EventArgs e)
        {
            model.Resolve = Form.cbResolveIssue.Checked;
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
            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
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
                           UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                           UpdateIssueEvent.Fire(this, Form.tbIssue.Text);
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


        [EventSubscription(AppTimers.IdleUpdate, typeof(OnPublisher))]
        public void OnIdleUpdateEvent(object sender, Args<int> arg)
        {
            model.IdleTime = model.IdleTime.Add(new TimeSpan(0, 0, arg.Data));
            model.Sync.Value(SyncTarget.View, "IdleTime");
        }

        [EventSubscription(AppTimers.WorkUpdate, typeof(OnPublisher))]
        public void OnWorkUpdateEvent(object sender, Args<int> arg)
        {
            model.WorkTime = model.WorkTime.Add(new TimeSpan(0, 0, arg.Data));
            model.Sync.Value(SyncTarget.View, "WorkTime");
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
                  ui.Visible = data.IssueInfo.Id > 0 || data.Comment != null;
              });
        }

        private void LoadComment(object sender, EventArgs e)
        {
            Form.cmComments.Items.Clear();

            var list = model.IssueComments.Where(x => !x.IsGlobal).ToList();

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

            list = model.IssueComments.Where(x => x.IsGlobal).ToList();

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

        private void OnActivityTypeChange(object sender, EventArgs e)
        {
            if (model.WorkActivities.Count > Form.cbActivity.SelectedIndex)
            {
                model.Activity = model.WorkActivities[Form.cbActivity.SelectedIndex];
                model.Sync.Value(SyncTarget.Source, "Activity");
            }
        }

        private void OnCommentChange()
        {
            Form.tbComment.Set(model.Comment,
              (ui, data) =>
              {
                  ui.Text = data != null ? data.Text : string.Empty;
                  ui.ReadOnly = data == null;
              });

            EnableSmallMode();
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
                    UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                    ExitEvent.Fire(this);
                }, () =>
                {
                    Form.Close();
                });
        }

        private void OnHideClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                       () =>
                       {
                           UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                           UpdateIssueEvent.Fire(this, Form.tbIssue.Text);
                       }, () =>
                       {
                           Form.WindowState = FormWindowState.Minimized;
                       });
        }

        private void OnIdleMode(object sender, EventArgs e)
        {
            Form.pManage.BackColor = Color.Azure;
            currentMode = Main.Actions.Idle;
        }

        private void OnIdleTimeChange()
        {
            Form.lblClockIndle.Set(model.IdleTime,
                (ui, data) =>
                {
                    ui.Text = data.ToString();
                });
        }

        private void OnIssueInfoChange()
        {
            Form.Set(model,
              (ui, data) =>
              {
                  ui.tbIssue.Text = data.IssueInfo.Id > 0 ? data.IssueInfo.Id.ToString() : "";

                  ui.lblProject.Text = data.IssueInfo.Project;
                  ui.lblTracker.Text = data.IssueInfo.Id > 0 ? "(" + data.IssueInfo.Tracker + ")" : "";
                  ui.lblIssue.Text = data.IssueInfo.Subject;

                  ui.btnRemoveItem.Visible = data.IssueInfo.Id > 0;
                  ui.btnSubmit.Visible = data.IssueInfo.Id > 0;
                  ui.btnSubmitAll.Visible = data.IssueInfo.Id > 0;
              });

            EnableSmallMode();
        }

        private void OnIssueParentInfoChange()
        {
            Form.lblParentIssue.Set(model.IssueParentInfo,
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
            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
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

        private void OnWorkActivitiesChange()
        {
            Form.cbActivity.Set(model,
             (ui, data) =>
             {
                 ui.Items.Clear();
                 ui.DataSource = data.WorkActivities;
                 ui.DisplayMember = "Name";
                 ui.ValueMember = "Id";

                 if (ui.Items.Count > 0)
                 {
                     ui.SelectedIndexChanged -= OnActivityTypeChange;
                     ui.SelectedItem = ui.Items[0];
                     ui.SelectedIndexChanged += OnActivityTypeChange;
                     data.Activity = data.WorkActivities[0];
                 }
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

        private void OnWorkTimeChange()
        {
            Form.lblClockActive.Set(model.WorkTime,
                (ui, data) =>
                {
                    ui.Text = data.ToString();
                });
        }

        private void OnResolveChange()
        {
            Form.cbResolveIssue.Set(model.Resolve,
                (ui, data) =>
                {
                    ui.Checked = data;
                });
        }

        private void SaveComment(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                new frmProcessing().Show(Form,
                    () =>
                    {
                        UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                    });
            }
        }

        private void SelectComment(object sender, ToolStripItemClickedEventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    var comment = e.ClickedItem.Tag as CommentData;
                    if (comment != null)
                    {

                        if (comment.IsGlobal && model.Issue.Id > 0)
                            AddCommentEvent.Fire(this, comment.Text);
                        else
                        {
                            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                            model.Comment = e.ClickedItem.Tag as CommentData;
                            model.Sync.Value(SyncTarget.View, "Comment");
                            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                        }
                    }
                });
        }
    }
}