﻿using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic;
using RedmineLog.Logic.Data;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSmall : Form
    {
        private bool isHide;
        public frmSmall()
        {
            InitializeComponent();
            this.Initialize<Small.IView, frmSmall>();
            isHide = false;
            cbAutoHide.Checked = true;
            Load += frmSmall_Load;
        }

        void frmSmall_Load(object sender, EventArgs e)
        {
            UnwarpForm();
            Load -= frmSmall_Load;
        }


        private void btnHide_Click(object sender, EventArgs e)
        {
            if (!isHide)
            {
                WrapForm();
            }
            else
            {
                UnwarpForm();
            }
        }
        private void UnwarpForm()
        {
            isHide = false;
            btnHide.Text = ">";
            this.SetupLocation(0, -150);

        }

        private void WrapForm()
        {
            isHide = true;
            this.Location = new Point(this.Location.X + flowLayoutPanel1.Width, this.Location.Y);
            btnHide.Text = "<";
        }

        private void frmSmall_MouseEnter(object sender, EventArgs e)
        {
            if (isHide)
            {
                UnwarpForm();
            }
        }

        private void btnHide_MouseEnter(object sender, EventArgs e)
        {
            if (isHide)
            {
                UnwarpForm();
            }
        }

        private void frmSmall_Deactivate(object sender, EventArgs e)
        {
            if (cbAutoHide.Checked && !isHide)
            {
                WrapForm();
            }
        }
    }

    internal class SmallView : Small.IView, IView<frmSmall>
    {
        private Small.IModel model;

        private frmSmall Form;

        [Inject]
        public SmallView(Small.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
            inGlobalEvent.Register(this);
        }


        [EventPublication(Small.Events.Load, typeof(Publish<Small.IView>))]
        public event EventHandler LoadEvent;

        [EventSubscription(AppTimers.WorkUpdate, typeof(OnPublisher))]
        public void OnWorkUpdateEvent(object sender, Args<int> arg)
        {
            model.Sync.Value(SyncTarget.View, "WorkTime");
        }

        [EventSubscription(AppTimers.IdleUpdate, typeof(OnPublisher))]
        public void OnIdleUpdateEvent(object sender, Args<int> arg)
        {
            model.Sync.Value(SyncTarget.View, "IdleTime");
        }


        public void Init(frmSmall inView)
        {
            Form = inView;

            Form.Click += OnFormClick;
            Form.lbProject.Click += OnFormClick;
            Form.lbParentIssue.Click += OnFormClick;
            Form.lblTracker.Click += OnFormClick;
            Form.lbWorkTime.Click += OnFormClick;
            Form.lbIdleTime.Click += OnFormClick;
            Form.lbComment.Click += OnFormClick;
            Form.flowLayoutPanel1.Click += OnFormClick;
            Form.lbIssue.Click += OnIssueClick;
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
        void OnWorkTimeChange()
        {
            Form.lbWorkTime.Set(model.WorkTime, (ui, data) =>
            {
                ui.Text = data.ToString();
            });
        }

        void OnIdleTimeChange()
        {
            Form.lbIdleTime.Set(model.IdleTime,
                (ui, data) =>
                {
                    ui.Text = data.ToString();
                });
        }
        void OnCommentChange()
        {
            Form.lbComment.Set(model,
                (ui, data) =>
                {
                    if (model.Comment != null)
                        ui.Text = "Comment : " + Environment.NewLine + " " + data.Comment.Text;
                });
        }

        void OnIssueParentInfoChange()
        {
            Form.lbParentIssue.Set(model.IssueParentInfo,
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
        void OnIssueInfoChange()
        {
            Form.Set(model,
              (ui, data) =>
              {
                  ui.lbComment.Visible = data.IssueInfo.Id == 0;

                  ui.lbIssue.Text = data.IssueInfo.Id > 0 ? data.IssueInfo.Id.ToString() : "";

                  ui.lbProject.Text = data.IssueInfo.Project;
                  ui.lblTracker.Text = data.IssueInfo.Id > 0 ? "(" + data.IssueInfo.Tracker + ")" : "";
                  ui.lbIssue.Text = data.IssueInfo.Subject;
              });
        }

        private void OnIssueClick(object sender, EventArgs e)
        {
            if (model.Comment != null) Form.toolTip1.Show(model.Comment.Text, Form.lbIssue);
        }

        private void OnFormClick(object sender, EventArgs e)
        {
            Form.Close();
        }
    }
}