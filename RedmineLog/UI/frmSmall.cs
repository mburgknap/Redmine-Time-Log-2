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
            MultipleScreenReversed = SystemInformation.VirtualScreen.Location.X < 0;
            this.Initialize<Small.IView, frmSmall>();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (MultipleScreenReversed)
            {
                this.Location = new Point(0 - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 150);
            }
            else
                this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 150);
        }

        public bool MultipleScreenReversed { get; protected set; }
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
            OriginalSize = Form.Size;
            Form.MouseLeave += OnFormMouseLeave;
            Form.Click += OnFormClick;
            Form.lbProject.Click += OnFormClick;
            Form.lbParentIssue.Click += OnFormClick;
            Form.lblTracker.Click += OnFormClick;
            Form.lbWorkTime.Click += OnFormClick;
            Form.lbIdleTime.Click += OnFormClick;
            Form.lbComment.Click += OnFormClick;
            Form.flowLayoutPanel1.Click += OnFormClick;
            Form.lbIssue.Click += OnIssueClick;

            Form.hideBtn.Click += OnButtonClick;
            Form.hideBtn.MouseMove += OnButtonMouseMove;
            
            Load();
            
            

        }
        public void Load()
        {
            new frmProcessing().Show(Form,
              () =>
              {
                  LoadEvent.Fire(this);
                  HideForm();
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
            if (!Hidden)
                Form.Close();
            else
                ResetForm(true);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            if (Hidden)
                ResetForm(true);
            else
                HideForm();
        }

        private void OnButtonMouseMove(object sender, EventArgs e)
        {
            if (Hidden)
                ResetForm(false);

        }
        private void OnFormMouseLeave(object sender, System.EventArgs e)
        {
            if (Hidden)
                HideForm();
        }

        private void HideForm()
        {
            Form.Size = new Size(Form.hideBtn.Width, Form.hideBtn.Height);

            if (Form.MultipleScreenReversed)
                Form.Location = new Point(0 - Form.hideBtn.Width, SystemInformation.VirtualScreen.Height - OriginalSize.Height - 150);
            else
                Form.Location = new Point(SystemInformation.VirtualScreen.Width - Form.hideBtn.Width, SystemInformation.VirtualScreen.Height - OriginalSize.Height - 150);

            Form.hideBtn.Image = Properties.Resources.resetBtn;
            Hidden = true;
        }

        private void ResetForm(bool isClicked)
        {
            Form.Size = new Size(OriginalSize.Width, OriginalSize.Height);

            if (Form.MultipleScreenReversed)
            {
                if (SystemInformation.VirtualScreen.Location.X < 0)
                    Form.Location = new Point(0 - Form.Width, SystemInformation.VirtualScreen.Height - Form.Height - 150);
                else
                    Form.Location = new Point(SystemInformation.VirtualScreen.Width - Form.Width, SystemInformation.VirtualScreen.Height - Form.Height - 150);
            }

            Form.hideBtn.Image = Properties.Resources.hideBtn;

            if (isClicked)
                Hidden = false;
            else
                Hidden = true;
        }
        public bool Hidden { get; set; }

        public Size OriginalSize { get; set; }

    }
}