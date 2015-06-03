using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic;
using RedmineLog.Logic.Data;
using RedmineLog.UI.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSmall : Form
    {
        private frmMain frmMain;

        public frmSmall()
        {
            InitializeComponent();
            this.Initialize<Small.IView, frmSmall>();
        }

        public frmSmall(frmMain frmMain)
            : this()
        {
            this.frmMain = frmMain;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
        }

        private void OnFormClick(object sender, EventArgs e)
        {
            frmMain.WindowState = FormWindowState.Normal;
        }

        internal void UpdateWorkTime(DateTime inTime)
        {
            SetText(lbWorkTime, inTime.ToLongTimeString());
        }

        internal void UpdateIdleTime(DateTime inTime)
        {
            SetText(lbIdleTime, inTime.ToLongTimeString());
        }

        private void OnlIssueLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (lMainIssue.Tag != null)
                    System.Diagnostics.Process.Start(lMainIssue.Tag.ToString());
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("OnlIssueLinkClicked", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        internal void SetMainIssue(RedmineIssues.Item inIssue)
        {
            if (inIssue != null)
            {
                SetText(lMainIssue, inIssue.Subject);
                SetText(lProject, inIssue.Project);
                lMainIssue.Tag = App.Context.Config.Url + "issues/" + inIssue.Id;
            }
            else
            {
                SetText(lMainIssue, "");
                SetText(lProject, "");
                lMainIssue.Tag = null;
            }
        }

        internal void SetParentIssue(RedmineIssues.Item inIssue)
        {
            if (inIssue != null)
                SetText(lbParentIssue, inIssue.Subject);
            else
                SetText(lbParentIssue, "");
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

    internal class SmallView : Small.IView, IView<frmSmall>
    {
        private Small.IModel Model;

        private frmSmall Form;

        [Inject]
        public SmallView(Small.IModel inModel, IEventBroker inGlobalEvent)
        {
            Model = inModel;
            inGlobalEvent.Register(this);
        }

        [EventPublication(Small.Events.Load, typeof(Publish<Small.IView>))]
        public event EventHandler LoadEvent;

        public void Init(frmSmall inView)
        {
            Form = inView;
            LoadEvent.Fire(this);
        }
    }
}