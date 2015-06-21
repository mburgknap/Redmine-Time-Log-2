using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
            this.Initialize<Main.IView, frmMain>();
            CheckForIllegalCrossThreadCalls = false;
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        private void OnMainLoad(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }

    }



    internal class MainView : Main.IView, IView<frmMain>
    {
        private Main.IModel model;

        private frmMain Form;

        [Inject]
        public MainView(Main.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            inGlobalEvent.Register(this);
            model.Sync.Bind(SyncTarget.View, this);
            AppTimers.Init(inGlobalEvent);
        }

        [EventPublication(Main.Events.Load, typeof(Publish<Main.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(Main.Events.Exit, typeof(Publish<Main.IView>))]
        public event EventHandler ExitEvent;

        [EventPublication(Main.Events.AddComment, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> AddCommentEvent;

        [EventPublication(Main.Events.UpdateComment, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> UpdateCommentEvent;

        [EventPublication(Main.Events.DelComment, typeof(Publish<Main.IView>))]
        public event EventHandler DelCommentEvent;

        [EventPublication(Main.Events.AddIssue, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> AddIssueEvent;

        [EventPublication(Main.Events.DelIssue, typeof(Publish<Main.IView>))]
        public event EventHandler DelIssueEvent;

        [EventPublication(Main.Events.Submit, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<Main.Actions>> SubmitEvent;

        [EventPublication(Main.Events.Reset, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<Main.Actions>> ResetEvent;

        [EventPublication(Main.Events.Link, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> GoLinkEvent;

        [EventSubscription(AppTimers.WorkUpdate, typeof(OnPublisher))]
        public void OnWorkUpdateEvent(object sender, Args<int> arg)
        {
            model.WorkTime = model.WorkTime.Add(new TimeSpan(0, 0, arg.Data));
            model.Sync.Value(SyncTarget.View, "WorkTime");
        }

        [EventSubscription(AppTimers.IdleUpdate, typeof(OnPublisher))]
        public void OnIdleUpdateEvent(object sender, Args<int> arg)
        {
            model.IdleTime = model.IdleTime.Add(new TimeSpan(0, 0, arg.Data));
            model.Sync.Value(SyncTarget.View, "IdleTime");
        }

        void OnWorkTimeChange()
        {
            SetText(Form.lblClockActive, model.WorkTime.ToString());
        }

        void OnIdleTimeChange()
        {
            SetText(Form.lblClockIndle, model.IdleTime.ToString());
        }

        public void SetText(Label inLabel, string inText)
        {
            if (inLabel.InvokeRequired)
            {
                inLabel.Invoke(new MethodInvoker(() => SetText(inLabel, inText)));
                return;
            }
            inLabel.Text = inText;
        }

        private Main.Actions currentMode;

        public void Init(frmMain inView)
        {
            Form = inView;
            Form.tbIssue.KeyDown += AddIssue;
            Form.btnRemoveItem.Click += DelIssue;
            Form.btnComments.Click += LoadComment;
            Form.btnNewComment.Click += AddComment;
            Form.btnRemoveComment.Click += DelComment;
            Form.tbComment.KeyDown += SaveComment;
            Form.cmComments.ItemClicked += SelectComment;
            Form.btnIssues.Click += OnSearchIssueClick;
            Form.btnSubmit.Click += OnSubmitClick;
            Form.btnSubmitAll.Click += OnSubmitAllClick;
            Form.btnResetIdle.Click += OnResetClockClick;
            Form.lHide.Click += OnHideClick;
            Form.lnkExit.Click += OnExitClick;
            Form.lblClockActive.Click += OnWorkMode;
            Form.lblClockIndle.Click += OnIdleMode;
            Form.cbActivity.SelectedIndexChanged += OnActivityTypeChange;
            Form.lnkSettings.Click += OnSettingClick;
            Form.lnkIssues.Click += OnRedmineIssuesLink;
            Form.lblIssue.Click += OnRedmineIssueLink;
            Form.tbComment.Click += OnCommentClick;
            Form.btnWorkTime.Click += OnWorkLogClick;

            OnWorkMode(this, EventArgs.Empty);

            LoadEvent.Fire(this);
        }

        private void OnWorkLogClick(object sender, EventArgs e)
        {
            Form.WindowState = FormWindowState.Minimized;

            var form = new frmWorkLog();

            form.FormClosed += (s, arg) =>
            {
                Form.WindowState = FormWindowState.Normal;
            };

            form.ShowDialog();
        }

        private void OnCommentClick(object sender, EventArgs e)
        {
            if (Form.tbComment.ReadOnly)
            {
                LoadComment(Form.btnComments, EventArgs.Empty);
            }
        }

        private void OnRedmineIssuesLink(object sender, EventArgs e)
        {
            GoLinkEvent.Fire(this, "Redmine");
        }

        private void OnRedmineIssueLink(object sender, EventArgs e)
        {
            GoLinkEvent.Fire(this, "Issue");
        }

        private void OnSettingClick(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
        }

        private void OnActivityTypeChange(object sender, EventArgs e)
        {
            if (model.WorkActivities.Count > Form.cbActivity.SelectedIndex)
                model.Activity = model.WorkActivities[Form.cbActivity.SelectedIndex];
        }


        private void OnIdleMode(object sender, EventArgs e)
        {
            Form.pManage.BackColor = Color.Azure;
            currentMode = Main.Actions.Idle;
        }

        private void OnWorkMode(object sender, EventArgs e)
        {
            Form.pManage.BackColor = Color.Wheat;
            currentMode = Main.Actions.Issue;
        }

        private void OnResetClockClick(object sender, EventArgs e)
        {
            ResetEvent.Fire(this, currentMode);
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
            ExitEvent.Fire(this);
            Form.Close();
        }

        private void OnHideClick(object sender, EventArgs e)
        {
            Form.WindowState = FormWindowState.Minimized;

            var form = new frmSmall();

            form.FormClosed += (s, arg) =>
            {
                Form.WindowState = FormWindowState.Normal;
            };

            form.ShowDialog();
        }

        private void OnSubmitAllClick(object sender, EventArgs e)
        {
            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
            SubmitEvent.Fire(this, Main.Actions.All);
        }

        private void OnSubmitClick(object sender, EventArgs e)
        {
            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
            SubmitEvent.Fire(this, currentMode);
        }

        private void SelectComment(object sender, ToolStripItemClickedEventArgs e)
        {
            var comment = e.ClickedItem.Tag as CommentData;

            if (comment.IsGlobal)
                AddCommentEvent.Fire(this, comment.Text);
            else
            {
                model.Comment = e.ClickedItem.Tag as CommentData;
                model.Sync.Value(SyncTarget.View, "Comment");
            }
        }

        private void SaveComment(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                UpdateCommentEvent.Fire(this, Form.tbComment.Text);
        }

        private void DelComment(object sender, EventArgs e)
        {
            DelCommentEvent.Fire(this);
        }

        private void AddComment(object sender, EventArgs e)
        {
            AddCommentEvent.Fire(this, Form.tbComment.Text);
        }

        private void DelIssue(object sender, EventArgs e)
        {
            DelIssueEvent.Fire(this);
        }

        private void AddIssue(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                AddIssueEvent.Fire(this, Form.tbIssue.Text);
        }

        private void OnSearchIssueClick(object sender, EventArgs e)
        {
            new frmSearch().ShowDialog();
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

        void OnCommentChange()
        {
            Form.tbComment.Text = model.Comment != null ? model.Comment.Text : string.Empty;
            Form.tbComment.ReadOnly = model.Comment == null;
        }
        void OnWorkActivitiesChange()
        {
            Form.cbActivity.Items.Clear();
            Form.cbActivity.DataSource = model.WorkActivities;
            Form.cbActivity.DisplayMember = "Name";
            Form.cbActivity.ValueMember = "Id";

            if (Form.cbActivity.Items.Count > 0)
            {
                Form.cbActivity.SelectedItem = Form.cbActivity.Items[0];
                model.Activity = model.WorkActivities[0];
            }
        }
        void OnIssueParentInfoChange()
        {
            if (model.IssueParentInfo != null)
            {
                Form.lblParentIssue.Text = model.IssueParentInfo.Subject + " :";
                Form.lblParentIssue.Visible = true;
            }
            else
                Form.lblParentIssue.Visible = false;
        }
        void OnIssueInfoChange()
        {
            Form.tbIssue.Text = model.IssueInfo.Id > 0 ? model.IssueInfo.Id.ToString() : "";

            Form.lblProject.Text = model.IssueInfo.Project;
            Form.lblTracker.Text = model.IssueInfo.Id > 0 ? "(" + model.IssueInfo.Tracker + ")" : "";
            Form.lblIssue.Text = model.IssueInfo.Subject;

            Form.btnRemoveItem.Visible = model.IssueInfo.Id > 0;
            Form.lHide.Visible = model.IssueInfo.Id > 0;
        }

        public void Info(string inMessage)
        {
            MessageBox.Show(inMessage);
        }

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
    }
}