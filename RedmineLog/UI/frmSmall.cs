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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSmall : Form, ISetup
    {
        private bool isHide;
        private IAppSettings settings;
        private int flowPanelWidth;
        private Timer timer;

        public frmSmall()
        {
            InitializeComponent();
            this.Initialize<Small.IView, frmSmall>();
            isHide = false;
            cbAutoHide.Checked = true;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += ShowHideForm;
            Load += OnFormLoad;
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            UnwrapForm();
            WrapForm();
            Load -= OnFormLoad;
        }


        private void OnHideClick(object sender, EventArgs e)
        {
            if (!isHide)
            {
                WrapForm();
            }
            else
            {
                UnwrapForm();
            }

            timer.Stop();
        }

        private void ShowHideForm(object sender, EventArgs e)
        {
            if (!isHide)
            {
                WrapForm();
            }
            else
            {
                UnwrapForm();
            }

            timer.Stop();
        }

        private void UnwrapForm()
        {
            isHide = false;
            btnHide.Text = ">";
            this.Width += flowPanelWidth;
            this.SetupLocation(settings.Display, 0, -150);

        }

        private void WrapForm()
        {
            timer.Stop();
            isHide = true;
            flowPanelWidth = flowLayoutPanel1.Width;
            this.Location = new Point(this.Location.X + flowLayoutPanel1.Width, this.Location.Y);
            this.Width -= flowPanelWidth;
            btnHide.Text = "<";
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            timer.Stop();

            if (isHide)
            {
                UnwrapForm();
            }
        }

        private void OnFormDeactivate(object sender, EventArgs e)
        {
            timer.Stop();
            if (!isHide)
            {
                WrapForm();
            }
        }



        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            if (cbAutoHide.Checked && !isHide)
            {
                timer.Start();
            }
        }

    }

    internal class SmallView : Small.IView, IView<frmSmall>
    {
        private Small.IModel model;

        private frmSmall Form;

        [Inject]
        public SmallView(Small.IModel inModel)
        {
            model = inModel;
            model.IdleTime.OnUpdate.Subscribe(OnUpdateIdleTime);
            model.IssueInfo.OnUpdate.Subscribe(OnUpdateIssueInfo);
            model.IssueParentInfo.OnUpdate.Subscribe(OnUpdateIssueParentInfo);
            model.Comment.OnUpdate.Subscribe(OnUpdateComment);
            model.WorkTime.OnUpdate.Subscribe(OnUpdateWorkTime);
        }

        private void OnUpdateComment(CommentData obj)
        {
            Form.lbComment.Set(obj,
                  (ui, data) =>
                  {
                      if (data != null)
                          ui.Text = "Comment : " + Environment.NewLine + " " + data.Text;
                  });
        }

        private void OnUpdateWorkTime(TimeSpan obj)
        {
            Form.lbWorkTime.Set(obj, (ui, data) =>
            {
                ui.Text = data.ToString();
            });
        }

        private void OnUpdateIssueParentInfo(RedmineIssueData obj)
        {
            Form.lbParentIssue.Set(obj,
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

        private void OnUpdateIssueInfo(RedmineIssueData obj)
        {
            Form.Set(obj,
               (ui, data) =>
               {
                   ui.lbComment.Visible = data.IsGlobal();

                   ui.lbIssue.Text = data.Id > 0 ? "#" + data.Id.ToString() : "";

                   ui.lbProject.Text = data.Project;
                   ui.lblTracker.Text = data.Id > 0 ? "(" + data.Tracker + ")" : "";
                   ui.lbIssue.Text = data.Subject;
               });
        }

        private void OnUpdateIdleTime(TimeSpan obj)
        {
            Form.lbIdleTime.Set(obj,
                  (ui, data) =>
                  {
                      ui.Text = data.ToString();
                  });
        }


        [EventPublication(Small.Events.Load, typeof(Publish<Small.IView>))]
        public event EventHandler LoadEvent;

        [EventSubscription(AppTime.Events.WorkUpdate, typeof(OnPublisher))]
        public void OnWorkUpdateEvent(object sender, Args<int> arg)
        {
            model.WorkTime.Update();
        }

        [EventSubscription(AppTime.Events.IdleUpdate, typeof(OnPublisher))]
        public void OnIdleUpdateEvent(object sender, Args<int> arg)
        {
            model.IdleTime.Update();
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
            Task.Run(() =>
            {
                LoadEvent.Fire(this);
            });
        }


        private void OnIssueClick(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(model.IssueUri.Value.ToString());
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("GoLink", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }


        private void OnFormClick(object sender, EventArgs e)
        {
            Form.Close();
        }
    }
}