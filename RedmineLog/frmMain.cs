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

        private DateTime startTime;
        private DateTime idleTime;

        private System.Timers.Timer idleCheck;
        private Issue mainIssue;
        private Issue parentIssue;
        private System.Timers.Timer workTimer;

        public frmMain()
        {
            FormClosing += OnFormClosing;
            CheckForIllegalCrossThreadCalls = false;
            WorkTimer = new System.Timers.Timer(1000);
            IdleChecker = new System.Timers.Timer(1000);
            InitializeComponent();
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            OnClockActiveClick(lblClockActive, null);


        }

        public enum ClockMode
        {
            Play,
            Pause,
            Stop
        }

        private System.Timers.Timer IdleChecker
        {
            get { return idleCheck; }
            set
            {
                if (idleCheck != null)
                {
                    idleCheck.Elapsed -= OnIdleCheckElapsed;
                }
                idleCheck = value;
                if (idleCheck != null)
                {
                    idleCheck.Elapsed += OnIdleCheckElapsed;
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
            var issue = new RedmineData.Issue(cbIssues.Text);

            if (!issue.IsValid
                && App.Constants.History.Contains(issue))
            {
                App.Constants.History.Remove(issue);
                var item = cbIssues.SelectedItem;
                cbIssues.SelectedItem = cbIssues.Items[0];
                cbIssues.Items.Remove(item);

                App.Constants.History.Save();
            }
        }


        private bool ContainIssue(object isId)
        {
            foreach (var obj in cbIssues.Items)
            {
                if (String.Equals(obj.ToString(), isId.ToString()))
                {
                    return true;
                }
            }
            return false;
        }


        private void ListTimeEntryActivites()
        {
            try
            {
                var manager = new RedmineManager(App.Constants.Config.Url, App.Constants.Config.ApiKey);

                var parameters = new NameValueCollection { };

                var responseList = manager.GetObjectList<TimeEntryActivity>(parameters);

                cbActivity.Items.Clear();
                cbActivity.DataSource = responseList;
                cbActivity.DisplayMember = "Name";
                cbActivity.ValueMember = "Id";

                if (cbActivity.Items.Count > 0)
                    cbActivity.SelectedItem = cbActivity.Items[0];

                App.Constants.History.Load();

                foreach (var item in App.Constants.History)
                {
                    cbIssues.Items.Add(item);
                }

                cbIssues.SelectedItem = cbIssues.Items[0];
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("ListActivites", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
                close = true;
                Application.Exit();
            }
        }

        private void LoadIssue()
        {
            try
            {
                tbComment.Tag = null;
                tbComment.Text = "";
                tbComment.ReadOnly = true;

                int isId = 0;

                if (!int.TryParse(cbIssues.Text, out isId))
                {
                    lblIssue.Text = "";
                    lblIssue.Tag = null;
                    lblIssue.Visible = false;
                    lblParentIssue.Text = "";
                    lblParentIssue.Visible = false;
                    btnRemoveItem.Visible = false;
                    return;
                }

                var manager = new RedmineManager(App.Constants.Config.Url, App.Constants.Config.ApiKey);

                var parameters = new NameValueCollection { };

                mainIssue = manager.GetObject<Issue>(isId.ToString(), parameters);
                parentIssue = null;

                lblIssue.Text = mainIssue.Subject;
                lblIssue.Tag = App.Constants.Config.Url + "issues/" + isId;
                lblIssue.Visible = true;

                btnRemoveItem.Visible = true;

                var tmp = new RedmineData.Issue(isId);

                if (!App.Constants.History.Contains(tmp))
                {
                    cbIssues.Items.Add(tmp);
                    App.Constants.History.Add(tmp);
                    App.Constants.History.Save();
                }


                if (mainIssue == null)
                {
                    lblIssue.Text = "";
                    lblIssue.Visible = false;
                    lblIssue.Tag = null;
                    btnRemoveItem.Visible = false;
                }


                if (mainIssue != null && mainIssue.ParentIssue != null)
                {
                    parentIssue = manager.GetObject<Issue>(mainIssue.ParentIssue.Id.ToString(), parameters);

                    lblParentIssue.Text = parentIssue.Subject;
                    lblParentIssue.Visible = true;
                }

                if (parentIssue == null)
                {
                    lblParentIssue.Text = "";
                    lblParentIssue.Visible = false;
                }
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("LoadIssue", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void OnClockClick(System.Object sender, System.EventArgs e)
        {
            var tmpTime = startTime;
            switch (clockMode)
            {
                case ClockMode.Pause:
                    clockMode = ClockMode.Play;
                    WorkTimer.Start();
                    btnClock.Text = "Pause";
                    break;

                case ClockMode.Play:
                    clockMode = ClockMode.Pause;
                    WorkTimer.Stop();
                    btnClock.Text = "Play";
                    break;

                case ClockMode.Stop:
                    clockMode = ClockMode.Play;
                    WorkTimer.Stop();
                    startTime = DateTime.MinValue;
                    btnClock.Text = "Pause";
                    break;
            }
            AppLogger.Log.Info("Clock: " + clockMode + " Time: " + tmpTime.ToLongTimeString());
        }

        private void OnExitClick(System.Object sender, System.EventArgs e)
        {
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


                if (!App.Constants.Config.Load())
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

                ListTimeEntryActivites();
                IdleChecker.Start();
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
            if (totalIdleTimeInSeconds > 30)
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
                lblClockIndle.Text = idleTime.ToLongTimeString();

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
                Process.Start(App.Constants.Config.Url + "issues");
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
                bool isNumeric = int.TryParse(cbIssues.Text, out n);

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


                DateTime time = submitMode == SubmitMode.Work ? startTime : idleTime;

                decimal hours = (decimal)(time.Hour * 60 + time.Minute) / 60m;

                var manager = new RedmineManager(App.Constants.Config.Url, App.Constants.Config.ApiKey);

                var response = manager.CreateObject<TimeEntry>(new TimeEntry()
                {
                    Issue = new IdentifiableName() { Id = Int32.Parse(cbIssues.Text) },
                    Activity = new IdentifiableName() { Id = ((TimeEntryActivity)cbActivity.SelectedItem).Id },
                    Comments = tbComment.Text,
                    Hours = decimal.Round(hours, 2),
                    SpentOn = DateTime.Now
                });

                if (submitMode == SubmitMode.Work)
                {
                    OnStopClick(btnStop, null);

                    tbComment.Text = "";
                    tbComment.Tag = null;
                    tbComment.ReadOnly = true;
                }
                else
                {
                    idleTime = DateTime.MinValue;
                    lblClockIndle.Text = idleTime.ToLongTimeString();
                }
                MessageBox.Show("Time Tracker Saved Successfully", "Thank you", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void OnSettingsClick(System.Object sender, System.EventArgs e)
        {
            new frmSettings().ShowDialog();
        }

        private void OnStopClick(System.Object sender, System.EventArgs e)
        {
            clockMode = ClockMode.Stop;
            AppLogger.Log.Info("Clock: " + clockMode + " Time: " + startTime.ToLongTimeString());

            WorkTimer.Stop();
            btnClock.Text = "Play";
            startTime = DateTime.MinValue;

            lblClockActive.Text = startTime.ToLongTimeString();
        }

        private void OnWorkTimeElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (clockMode == ClockMode.Play)
            {
                startTime = startTime.AddSeconds(1);
                lblClockActive.Text = startTime.ToLongTimeString();

                if (small != null)
                    small.UpdateWorkTime(startTime);
            }
            else if (clockMode == ClockMode.Stop)
            { lblClockActive.Text = DateTime.MinValue.ToLongTimeString(); }
        }

        private void OnIssueChanged(object sender, EventArgs e)
        {
            LoadIssue();
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
            lblClockIndle.Text = idleTime.ToLongTimeString();
        }

        private void OnNewCommentClick(object sender, EventArgs e)
        {
            tbComment.Tag = new RedmineData.Issue.Comment() { Id = Guid.NewGuid() };
            tbComment.Text = "";
            tbComment.ReadOnly = false;
        }

        private void OnRemoveCommentClick(object sender, EventArgs e)
        {
            var comment = tbComment.Tag as RedmineData.Issue.Comment;
            tbComment.Tag = null;
            tbComment.Text = "";
            tbComment.ReadOnly = true;

            var tmpIssue = App.Constants.History.GetIssue(cbIssues.SelectedItem);

            if (tmpIssue != null && comment != null)
            {
                tmpIssue.Comments.Remove(comment);
                App.Constants.History.Save();
            }
        }

        private void OnSelectComment(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag != null)
            {
                tbComment.Text = e.ClickedItem.Tag.ToString();
                tbComment.Tag = e.ClickedItem.Tag;
                tbComment.ReadOnly = false;
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
            var tmpIssue = App.Constants.History.GetIssue(cbIssues.SelectedItem);

            if (tmpIssue != null)
            {
                var tmpComment = tmpIssue.Comments.Where(x => x.Equals(tbComment.Tag)).FirstOrDefault();

                if (tmpComment != null)
                {
                    tmpComment.Text = tbComment.Text;
                    App.Constants.History.Save();
                }
                else
                {
                    tmpComment = tbComment.Tag as RedmineData.Issue.Comment;

                    if (tmpComment != null)
                    {
                        if (tmpIssue.Id == -1)
                            tmpComment.IsGlobal = true;
                        else
                        {
                            if (tmpComment.IsGlobal)
                            {
                                tmpComment = new RedmineData.Issue.Comment()
                                    {
                                        Id = Guid.NewGuid(),
                                    };

                                tbComment.Tag = tmpComment;
                            }
                        }

                        tmpComment.Text = tbComment.Text;
                        tmpIssue.Comments.Add(tmpComment);
                        App.Constants.History.Save();
                    }
                }
            }
        }

        private void OnCommentClick(object sender, EventArgs e)
        {
            if (tbComment.Tag != null)
                return;

            ShowComments();
        }

        private void ShowComments()
        {
            var tmp = App.Constants.History.GetIssue(cbIssues.SelectedItem);

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

            tmp = App.Constants.History.GetIssue(-1);

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
            var comment = tbComment.Tag as RedmineData.Issue.Comment;

            if (comment != null && !comment.Text.Equals(tbComment.Text))
            {
                AcceptComment();
            }
        }

        private void OnCommentMouseEnter(object sender, EventArgs e)
        {
            ShowComments();
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
                small.ShowDialog();
                System.Diagnostics.Debug.WriteLine(WindowState.ToString());
            }
        }

        private void OnHideMouseEnter(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

    }
}