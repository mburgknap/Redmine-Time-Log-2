using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Properties;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

namespace RedmineLog
{
    public partial class frmMain : Form
    {
        public enum SubmitMode
        {
            Work,
            Idle
        }

        private frmSmall small;
        private SubmitMode submitMode = SubmitMode.Work;
        private ClockMode clockMode = ClockMode.Stop;
        private bool close = false;

        private DateTime workTime;
        private DateTime idleTime;

        private System.Timers.Timer idleTimer;
        private LogData.Issue.Comment issueComment;
        private LogData.Issue issueData;
        private RedmineIssues.Item mainIssue;
        private RedmineIssues.Item parentIssue;
        private System.Timers.Timer workTimer;
        private frmSearch search;

        public frmMain()
        {
            InitializeComponent();
            FormClosing += OnFormClosing;
            CheckForIllegalCrossThreadCalls = false;
            WorkTimer = new System.Timers.Timer(1000);
            IdleTimer = new System.Timers.Timer(1000);
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            OnClockActiveClick(lblClockActive, null);
            ManageHide();

        }

        public enum ClockMode
        {
            Play,
            Pause,
            Stop
        }

        private System.Timers.Timer IdleTimer
        {
            get { return idleTimer; }
            set
            {
                if (idleTimer != null)
                {
                    idleTimer.Elapsed -= OnIdleCheckElapsed;
                }
                idleTimer = value;
                if (idleTimer != null)
                {
                    idleTimer.Elapsed += OnIdleCheckElapsed;
                }
            }
        }

        private System.Timers.Timer WorkTimer
        {
            get { return workTimer; }
            set
            {
                if (workTimer != null)
                {
                    workTimer.Elapsed -= OnWorkTimeElapsed;
                }
                workTimer = value;
                if (workTimer != null)
                {
                    workTimer.Elapsed += OnWorkTimeElapsed;
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LastInput rLI);

        private void OnRemoveItem(object sender, EventArgs e)
        {
            var issue = new LogData.Issue(tbIssue.Text);

            if (issue.IsValid
                && App.Context.History.Contains(issue))
            {
                App.Context.History.Remove(issue);
                App.Context.History.Save();

                tbComment.Text = "";
                issueComment = null;
                tbComment.ReadOnly = true;

                tbIssue.Text = "";
                issueData = App.Context.History.GetIssue(-1);
                LoadIssue();
            }
        }

        private void LoadAppData()
        {
            try
            {
                var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);

                var parameters = new NameValueCollection { };

                var responseList = manager.GetObjectList<TimeEntryActivity>(parameters);

                cbActivity.Items.Clear();
                cbActivity.DataSource = responseList;
                cbActivity.DisplayMember = "Name";
                cbActivity.ValueMember = "Id";

                if (cbActivity.Items.Count > 0)
                    cbActivity.SelectedItem = cbActivity.Items[0];

                App.Context.History.Load();
                App.Context.IssuesCache.Load();
                App.Context.Work.Load();

                tbIssue.Text = "";
                issueData = App.Context.History.GetIssue(-1);
                LoadIssue();
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("LoadAppData", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
                close = true;
                Application.Exit();
            }
        }

        private void LoadIssue()
        {
            try
            {
                bool cacheChanged = false;

                tbComment.Text = "";
                tbComment.ReadOnly = true;

                int newIssueId = 0;

                if (!int.TryParse(tbIssue.Text, out newIssueId))
                {
                    mainIssue = null;
                    parentIssue = null;
                    lblIssue.Text = "";
                    lblIssue.Tag = null;
                    lblIssue.Visible = false;
                    lblParentIssue.Text = "";
                    lblParentIssue.Visible = false;
                    btnRemoveItem.Visible = false;
                    issueData = null;
                    issueComment = null;
                    tbComment.Text = "";
                    tbComment.ReadOnly = true;
                    ManageHide();
                    return;
                }

                if (mainIssue != null && mainIssue.Id != newIssueId && clockMode == ClockMode.Play)
                {
                    SaveIssueWorkTime(mainIssue);
                }

                issueComment = null;
                tbComment.Text = "";
                tbComment.ReadOnly = true;

                mainIssue = App.Context.IssuesCache.GetIssue(newIssueId);

                if (mainIssue == null)
                {
                    var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);
                    var parameters = new NameValueCollection { };
                    mainIssue = new RedmineIssues.Item(manager.GetObject<Issue>(newIssueId.ToString(), parameters));

                    if (mainIssue != null)
                    {
                        App.Context.IssuesCache.Add(mainIssue);
                        cacheChanged = true;
                    }


                }
                parentIssue = null;


                if (mainIssue == null)
                {
                    lblIssue.Text = "";
                    lblIssue.Visible = false;
                    btnRemoveItem.Visible = false;
                }
                else
                {
                    lblIssue.Text = mainIssue.Subject;
                    lblIssue.Tag = App.Context.Config.Url + "issues/" + newIssueId;
                    lblIssue.Visible = true;

                    btnRemoveItem.Visible = true;

                    var tmp = App.Context.History.GetIssue(newIssueId);

                    if (tmp == null)
                    {
                        tmp = new LogData.Issue(newIssueId);
                        App.Context.History.Add(tmp);
                        App.Context.History.Save();
                    }

                    issueData = tmp;

                }


                if (mainIssue != null
                    && mainIssue.IdParent.HasValue)
                {

                    parentIssue = App.Context.IssuesCache.GetIssue(mainIssue.IdParent.Value);

                    if (parentIssue == null)
                    {
                        var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);
                        var parameters = new NameValueCollection { };
                        parentIssue = new RedmineIssues.Item(manager.GetObject<Issue>(mainIssue.IdParent.Value.ToString(), parameters));

                        if (parentIssue != null)
                        {
                            App.Context.IssuesCache.Add(parentIssue);
                            cacheChanged = true;
                        }
                    }

                    lblParentIssue.Text = parentIssue.Subject;
                    lblParentIssue.Visible = true;
                }

                if (parentIssue == null)
                {
                    lblParentIssue.Text = "";
                    lblParentIssue.Visible = false;
                }


                if (cacheChanged)
                    App.Context.IssuesCache.Save();

                SetupIssueWorkTime(mainIssue);


                ManageHide();
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("LoadIssue", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void SetupIssueWorkTime(RedmineIssues.Item mainIssue)
        {
            var tmpWork = App.Context.Work.Get(mainIssue.Id);

            if (tmpWork != null)
            {
                workTime = tmpWork.WorkTime;
                LoadComment(tmpWork.IdComment);
            }
            else
            {
                App.Context.Work.Add(new TimeLogData.TaskTime(mainIssue.Id,
                    issueComment != null ? issueComment.Id : (Guid?)null,
                    workTime));
                App.Context.Work.Save();
            }

            if (clockMode == ClockMode.Stop)
                SetClockMode(ClockMode.Play);
        }

        private void LoadComment(Guid? inIdComment)
        {
            if (issueData != null && inIdComment.HasValue)
            {
                issueComment = issueData.Comments.Where(x => x.Id == inIdComment.Value).FirstOrDefault();
            }

            if (issueComment != null)
            {
                tbComment.Text = issueComment.Text;
                tbComment.ReadOnly = false;
            }
            else
            {
                tbComment.Text = "";
                tbComment.ReadOnly = true;
            }
        }


        private void SaveIssueWorkTime(RedmineIssues.Item mainIssue)
        {
            var tmp = App.Context.Work.Get(mainIssue.Id);

            if (tmp == null)
                App.Context.Work.Add(new TimeLogData.TaskTime(mainIssue.Id,
                   issueComment != null ? issueComment.Id : (Guid?)null,
                   workTime));
            else
            {
                tmp.WorkTime = workTime;
                tmp.IdComment = issueComment != null ? issueComment.Id : (Guid?)null;
            }

            SetClockMode(ClockMode.Stop);

            App.Context.Work.Save();
        }

        private void SetClockMode(ClockMode inClockMode)
        {
            var tmpTime = workTime;
            switch (inClockMode)
            {
                case ClockMode.Pause:
                    clockMode = ClockMode.Pause;
                    WorkTimer.Stop();
                    btnClock.Text = "Play";
                    break;
                case ClockMode.Play:
                    clockMode = ClockMode.Play;
                    WorkTimer.Start();
                    btnClock.Text = "Pause";
                    ManageHide();
                    break;
                case ClockMode.Stop:
                    clockMode = ClockMode.Stop;
                    WorkTimer.Stop();
                    workTime = DateTime.MinValue;
                    btnClock.Text = "Play";
                    ManageHide();
                    break;
            }
            AppLogger.Log.Info("Clock: " + clockMode + " Time: " + tmpTime.ToLongTimeString());
        }


        private void OnExitClick(System.Object sender, System.EventArgs e)
        {
            SaveIssueWorkTime(mainIssue);
            close = true;
            Application.Exit();
        }

        private void OnFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (close == false)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void OnFormLoad(object sender, System.EventArgs e)
        {
            try
            {
                var screen = Screen.FromPoint(this.Location);
                this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);


                if (!App.Context.Config.Load())
                {
                    var objSettings = new frmSettings();

                    if (objSettings.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                }

                lblIssue.Text = "";
                lblIssue.Tag = null;
                lblIssue.Visible = false;
                lblParentIssue.Text = "";
                lblParentIssue.Visible = false;
                btnRemoveItem.Visible = false;

                LoadAppData();
                IdleTimer.Start();
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("frmMain_Load", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
                Application.Exit();
            }
        }

        private void OnIconClick(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();
        }

        private void OnIdleCheckElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int sysupTime = Environment.TickCount;
            int lstTick = 0;
            int idlTick = 0;

            LastInput lInput = new LastInput();
            lInput.cSize = Convert.ToUInt32(Marshal.SizeOf(lInput));
            lInput.dtime = 0;

            if (GetLastInputInfo(ref lInput))
            {
                lstTick = Convert.ToInt32(lInput.dtime);
                idlTick = sysupTime - lstTick;
            }

            int totalIdleTimeInSeconds = idlTick / 1000;
            if (totalIdleTimeInSeconds > App.Context.Work.IdleSeconds)
            {
                if (WorkTimer.Enabled == true)
                {
                    WorkTimer.Enabled = false;
                }

                idleTime = idleTime.AddSeconds(1);
            }
            else
            {
                if (WorkTimer.Enabled == false & clockMode == ClockMode.Play)
                {
                    WorkTimer.Enabled = true;
                }
            }

            if (!WorkTimer.Enabled)
            {
                SetText(lblClockIndle, idleTime.ToLongTimeString());

                if (small != null)
                    small.UpdateIdleTime(idleTime);
            }
        }

        private void OnIssueLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lblIssue.Tag.ToString());
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("btnSend_Click", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void OnRedmineIssuesClick(System.Object sender, System.EventArgs e)
        {
            try
            {
                Process.Start(App.Context.Config.Url + "issues");
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("btnSend_Click", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void OnSubmitClick(System.Object sender, System.EventArgs e)
        {
            try
            {
                int n;
                bool isNumeric = int.TryParse(tbIssue.Text, out n);

                if (isNumeric == false)
                {
                    MessageBox.Show("Invalid Issue ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(tbComment.Text.Trim()))
                {
                    MessageBox.Show("No comment entered", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                DateTime time = submitMode == SubmitMode.Work ? workTime : idleTime;

                decimal hours = (decimal)(time.Hour * 60 + time.Minute) / 60m;

                var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);

                var response = manager.CreateObject<TimeEntry>(new TimeEntry()
                {
                    Issue = new IdentifiableName() { Id = mainIssue.Id },
                    Activity = new IdentifiableName() { Id = ((TimeEntryActivity)cbActivity.SelectedItem).Id },
                    Comments = tbComment.Text,
                    Hours = decimal.Round(hours, 2),
                    SpentOn = DateTime.Now
                });


                if (submitMode == SubmitMode.Work)
                {
                    SetClockMode(ClockMode.Stop);
                    SetText(lblClockActive, workTime.ToLongTimeString());

                    App.Context.Work.RemoveId(mainIssue.Id);
                }
                else
                {
                    idleTime = DateTime.MinValue;
                    SetText(lblClockIndle, idleTime.ToLongTimeString());
                }
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("btnSend_Click", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void OnAcceptIssue(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadIssue();
            }
        }

        private void ManageHide()
        {
            if (clockMode == ClockMode.Play && mainIssue != null && mainIssue.Id > 0)
                lHide.Visible = true;
            else
                lHide.Visible = false;
        }

        private void OnSettingsClick(System.Object sender, System.EventArgs e)
        {
            new frmSettings().ShowDialog();
        }

        private void OnStopClick(System.Object sender, System.EventArgs e)
        {
            SetClockMode(ClockMode.Stop);
        }

        private void OnWorkTimeElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (clockMode == ClockMode.Play)
            {
                workTime = workTime.AddSeconds(1);
                SetText(lblClockActive, workTime.ToLongTimeString());

                if (small != null)
                    small.UpdateWorkTime(workTime);
            }
            else if (clockMode == ClockMode.Stop)
            {
                SetText(lblClockActive, DateTime.MinValue.ToLongTimeString());
            }
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


        internal struct LastInput
        {
            public uint cSize;
            public uint dtime;
        }

        private void OnClockActiveClick(object sender, EventArgs e)
        {
            btnResetIdle.Visible = false;
            btnClock.Visible = true;
            btnStop.Visible = true;

            pManage.BackColor = Color.Wheat;
            submitMode = SubmitMode.Work;
        }

        private void OnClockIdleClick(object sender, EventArgs e)
        {
            btnResetIdle.Visible = true;
            btnClock.Visible = false;
            btnStop.Visible = false;

            pManage.BackColor = Color.Azure;
            submitMode = SubmitMode.Idle;
        }

        private void OnResetIdleClick(object sender, EventArgs e)
        {
            AppLogger.Log.Info("Reset Idle Clock: " + clockMode + " Time: " + idleTime.ToLongTimeString());
            idleTime = DateTime.MinValue;
            SetText(lblClockIndle, idleTime.ToLongTimeString());
        }

        private void OnNewCommentClick(object sender, EventArgs e)
        {
            issueComment = new LogData.Issue.Comment() { Id = Guid.NewGuid() };
            tbComment.Text = "";
            tbComment.ReadOnly = false;
        }

        private void OnRemoveCommentClick(object sender, EventArgs e)
        {
            var tmpIssue = App.Context.History.GetIssue(issueData);

            if (tmpIssue != null && issueComment != null)
            {
                tmpIssue.Comments.Remove(issueComment);
                App.Context.History.Save();
            }

            issueComment = null;
            tbComment.Text = "";
            tbComment.ReadOnly = true;
        }

        private void OnSelectComment(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag != null)
            {
                issueComment = e.ClickedItem.Tag as LogData.Issue.Comment;
                LoadComment(issueComment != null ? issueComment.Id : (Guid?)null);

                var tmp = App.Context.Work.Get(issueData.Id);

                if (tmp != null)
                {
                    tmp.IdComment = issueComment.Id;
                    App.Context.Work.Save();
                }

            }
        }


        private void OnCommentKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys.LButton | Keys.MButton | Keys.Back))
            {
                AcceptComment();
                e.Handled = true;
            }
        }

        private void AcceptComment()
        {
            var tmpIssue = App.Context.History.GetIssue(issueData);

            if (tmpIssue != null)
            {
                var tmpComment = tmpIssue.Comments.Where(x => x.Equals(issueComment)).FirstOrDefault();

                if (tmpComment != null)
                {
                    tmpComment.Text = tbComment.Text;
                    App.Context.History.Save();
                }
                else
                {
                    tmpComment = issueComment;

                    if (tmpComment != null)
                    {
                        if (tmpIssue.Id == -1)
                            tmpComment.IsGlobal = true;
                        else
                        {
                            if (tmpComment.IsGlobal)
                            {
                                tmpComment = new LogData.Issue.Comment()
                                    {
                                        Id = Guid.NewGuid(),
                                    };

                                issueComment = tmpComment;
                            }
                        }

                        tmpComment.Text = tbComment.Text;
                        tmpIssue.Comments.Add(tmpComment);
                        App.Context.History.Save();
                    }
                }
            }
        }

        private void OnCommentClick(object sender, EventArgs e)
        {
            if (issueComment != null)
                return;

            ShowComments();
        }

        private void ShowComments()
        {
            var tmp = App.Context.History.GetIssue(issueData);

            cmComments.Items.Clear();

            if (tmp != null && tmp.Id > 0)
            {
                if (tmp.Comments.Count > 0)
                {
                    cmComments.Items.Add(new ToolStripSeparator());
                    var tmpStrip = new ToolStripStatusLabel("Issue comments");
                    tmpStrip.Font = new Font(tmpStrip.Font, FontStyle.Bold);
                    cmComments.Items.Add(tmpStrip);
                    cmComments.Items.Add(new ToolStripSeparator());
                }

                foreach (var item in tmp.Comments)
                {
                    var cmItem = cmComments.Items.Add(item.Text);
                    cmItem.Overflow = ToolStripItemOverflow.AsNeeded;
                    cmItem.Tag = item;
                }
            }

            tmp = App.Context.History.GetIssue(-1);

            if (tmp.Comments.Count > 0)
            {
                cmComments.Items.Add(new ToolStripSeparator());

                var tmpStrip = new ToolStripStatusLabel("Global comments");
                tmpStrip.Font = new Font(tmpStrip.Font, FontStyle.Bold);
                cmComments.Items.Add(tmpStrip);
                cmComments.Items.Add(new ToolStripSeparator());
            }

            foreach (var item in tmp.Comments)
            {
                var cmItem = cmComments.Items.Add(item.Text);
                cmItem.Overflow = ToolStripItemOverflow.AsNeeded;
                cmItem.Tag = item;
            }

            if (cmComments.Items.Count > 0)
                cmComments.Show(tbComment, 0, 0);
        }

        private void OnCommentLostFocus(object sender, EventArgs e)
        {

            if (issueComment != null && !issueComment.Text.Equals(tbComment.Text))
            {
                AcceptComment();
            }
        }


        private void OnFormResize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                if (small != null)
                {
                    small.Close();
                    small = null;
                }
                System.Diagnostics.Debug.WriteLine(WindowState.ToString());
            }
            else if (WindowState == FormWindowState.Minimized)
            {
                small = new frmSmall(this);
                small.SetMainIssue(mainIssue);
                small.SetParentIssue(parentIssue);
                small.UpdateWorkTime(workTime);
                small.UpdateIdleTime(idleTime);
                small.ShowDialog();
                System.Diagnostics.Debug.WriteLine(WindowState.ToString());
            }
        }

        private void OnHideMouseEnter(object sender, EventArgs e)
        {
            if (clockMode == ClockMode.Play)
                WindowState = FormWindowState.Minimized;
        }


        private void OnSearchIssueClick(object sender, EventArgs e)
        {
            search = new frmSearch();
            search.OnSelect = (int id) =>
            {
                if (id < 0)
                    tbIssue.Text = "";
                else
                    tbIssue.Text = id.ToString();


                LoadIssue();

            };
            search.Show();

        }

        private void OnIssueKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys.LButton | Keys.MButton | Keys.Back))
                LoadIssue();
        }

        private void OnCommentShowClick(object sender, EventArgs e)
        {
            ShowComments();
        }

        private void OnIssueMouseClick(object sender, MouseEventArgs e)
        {
            tbIssue.SelectAll();
            tbIssue.Focus();
        }

        private void OnClockClick(object sender, EventArgs e)
        {
            if (clockMode == ClockMode.Play)
            { SetClockMode(ClockMode.Pause); }

            if (clockMode == ClockMode.Pause
                || clockMode == ClockMode.Stop)
            { SetClockMode(ClockMode.Play); }
        }
    }
}