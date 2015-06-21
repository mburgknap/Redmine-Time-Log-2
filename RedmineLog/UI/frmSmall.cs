using Appccelerate.EventBroker;
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
        public frmSmall()
        {
            InitializeComponent();
            this.Initialize<Small.IView, frmSmall>();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 150);
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
            Form.flowLayoutPanel1.Click += OnFormClick;
            Form.lbIssue.Click += OnIssueClick;

            LoadEvent.Fire(this);
        }

        void OnWorkTimeChange()
        {
            SetText(Form.lbWorkTime, model.WorkTime.ToString());
        }

        void OnIdleTimeChange()
        {
            SetText(Form.lbIdleTime, model.IdleTime.ToString());
        }

        void OnIssueParentInfoChange()
        {
            if (model.IssueParentInfo != null)
            {
                Form.lbParentIssue.Text = model.IssueParentInfo.Subject + " :";
                Form.lbParentIssue.Visible = true;
            }
            else
                Form.lbParentIssue.Visible = false;
        }
        void OnIssueInfoChange()
        {
            Form.lbIssue.Text = model.IssueInfo.Id > 0 ? model.IssueInfo.Id.ToString() : "";

            Form.lbProject.Text = model.IssueInfo.Project;
            Form.lblTracker.Text = model.IssueInfo.Id > 0 ? "(" + model.IssueInfo.Tracker + ")" : "";
            Form.lbIssue.Text = model.IssueInfo.Subject;

        }

        private void OnIssueClick(object sender, EventArgs e)
        {
            if (model.Comment != null) Form.toolTip1.Show(model.Comment.Text, Form.lbIssue);
        }

        private void OnFormClick(object sender, EventArgs e)
        {
            Form.Close();
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
    }
}