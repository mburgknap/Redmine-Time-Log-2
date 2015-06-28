﻿using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using RedmineLog.Common;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
            Form.btnBugs.Click += SearchBugClick;
            Form.tbIssue.KeyDown += SaveIssue;
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
            Form.btnRemoveItem.Visible = false;
            Form.btnSubmit.Visible = false;
            Form.btnSubmitAll.Visible = false;
            Load();
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
            Form.WindowState = FormWindowState.Minimized;

            var form = new frmSmall();

            form.FormClosed += (s, arg) =>
            {
                Form.WindowState = FormWindowState.Normal;
            };

            form.ShowDialog();
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

        private void OnRedmineIssueLink(object sender, EventArgs e)
        {
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

            form.Load += (s, arg) =>
            {
                Form.WindowState = FormWindowState.Minimized;
            };

            form.FormClosed += (s, arg) =>
            {
                Form.WindowState = FormWindowState.Normal;
            };

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

                    if (comment.IsGlobal && model.Issue.Id > 0)
                        AddCommentEvent.Fire(this, comment.Text);
                    else
                    {
                        UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                        model.Comment = e.ClickedItem.Tag as CommentData;
                        model.Sync.Value(SyncTarget.View, "Comment");
                        UpdateCommentEvent.Fire(this, Form.tbComment.Text);
                    }
                });
        }
    }
}