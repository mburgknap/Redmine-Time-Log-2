using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Properties;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmMain : Form
    {
        private Mode clockMode = Mode.Stop;
        private bool close = false;

        private Time currentTime;

        private bool hideTooltip = false;
        private System.Timers.Timer idleCheck;
        private Issue mainIssue;
        private Issue parentIssue;
        private int time;
        private System.Timers.Timer workTimer;

        public frmMain()
        {
            FormClosing += OnFormClosing;
            CheckForIllegalCrossThreadCalls = false;
            WorkTimer = new System.Timers.Timer(1000);
            IdleChecker = new System.Timers.Timer(1000);
            InitializeComponent();
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        public enum Mode
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

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cbIssues.Text)
                && ContainIssue(cbIssues.Text))
            {
                Settings.Default.WorkingIssueList = Settings.Default.WorkingIssueList.Replace(";" + cbIssues.Text + ";", ";");
                var item = cbIssues.SelectedItem;
                cbIssues.SelectedItem = cbIssues.Items[0];
                cbIssues.Items.Remove(item);

                Settings.Default.Save();
            }
        }

        private void calculateTime()
        {
            int h = 0;
            int m = 0;
            int s = 0;
            int remainder = 0;
            //_time = 5000
            h = Math.DivRem(time, 3600, out remainder);
            m = Math.DivRem(remainder, 60, out s);
            if (s == 0 & m == 0 & h == 0)
            {
                s = time;
            }
            currentTime.Hour = h;
            currentTime.Minute = m;
            currentTime.Second = s;

            lblClock.Text = h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
            ntIcon.Text = lblClock.Text;
            if (hideTooltip == false)
            {
                ntIcon.BalloonTipText = lblClock.Text;
                ntIcon.ShowBalloonTip(1000);
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

        private void HideTooltipToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
        }

        private void ListTimeEntryActivites()
        {
            try
            {
                var manager = new RedmineManager(Settings.Default.RedmineURL, Settings.Default.ApiKey);

                var parameters = new NameValueCollection { };

                var responseList = manager.GetObjectList<TimeEntryActivity>(parameters);

                cbActivity.Items.Clear();
                cbActivity.DataSource = responseList;
                cbActivity.DisplayMember = "Name";
                cbActivity.ValueMember = "Id";

                cbIssues.Items.Add(string.Empty);
                foreach (var item in Settings.Default.WorkingIssueList.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        cbIssues.Items.Add(item);
                }
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
                int isId = 0;

                if (!int.TryParse(cbIssues.Text, out isId))
                {
                    lblIssue.Text = "";
                    lblIssue.Visible = false;
                    llIssueUrl.Text = "";
                    llIssueUrl.Visible = false;
                    lblParentIssue.Text = "";
                    lblParentIssue.Visible = false;
                    btnRemoveItem.Visible = false;
                    return;
                }

                var manager = new RedmineManager(Settings.Default.RedmineURL, Settings.Default.ApiKey);

                var parameters = new NameValueCollection { };

                var responseList = manager.GetObjectList<Issue>(parameters);

                mainIssue = null;
                parentIssue = null;

                foreach (var issue in responseList)
                {
                    if (issue.Id == isId)
                    {
                        mainIssue = issue;
                        lblIssue.Text = mainIssue.Subject;
                        llIssueUrl.Text = Settings.Default.RedmineURL + "issues/" + isId;
                        lblIssue.Visible = true;
                        llIssueUrl.Visible = true;
                        btnRemoveItem.Visible = true;

                        if (!ContainIssue(isId))
                        {
                            cbIssues.Items.Add(isId);
                            Settings.Default.WorkingIssueList += isId + ";";
                            Settings.Default.Save();
                        }
                        break;
                    }
                }

                if (mainIssue == null)
                {
                    lblIssue.Text = "";
                    lblIssue.Visible = false;
                    llIssueUrl.Text = "";
                    llIssueUrl.Visible = false;
                    btnRemoveItem.Visible = false;
                }


                if (mainIssue != null
                    && mainIssue.ParentIssue != null)


                    foreach (var issue in responseList)
                    {
                        if (issue.Id == mainIssue.ParentIssue.Id)
                        {
                            parentIssue = issue;
                            lblParentIssue.Text = parentIssue.Subject;
                            lblParentIssue.Visible = true;
                            return;
                        }
                    }
                else
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
            switch (clockMode)
            {
                case Mode.Pause:
                    clockMode = Mode.Play;
                    WorkTimer.Start();
                    btnClock.Text = "Pause";
                    break;

                case Mode.Play:
                    clockMode = Mode.Pause;
                    WorkTimer.Stop();
                    btnClock.Text = "Play";
                    break;

                case Mode.Stop:
                    clockMode = Mode.Play;
                    WorkTimer.Start();
                    time = 0;
                    btnClock.Text = "Pause";
                    break;
            }
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
                bool saveSettings = false;

                bool.TryParse(Settings.Default.PersistentSettings, out saveSettings);

                if (!saveSettings)
                {
                    var objSettings = new frmSettings();

                    if (objSettings.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                }

                lblIssue.Text = "";
                lblIssue.Visible = false;
                llIssueUrl.Text = "";
                llIssueUrl.Visible = false;
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
            }
            else
            {
                if (WorkTimer.Enabled == false & clockMode == Mode.Play)
                {
                    WorkTimer.Enabled = true;
                }
            }
        }

        private void OnIssueLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(llIssueUrl.Text);
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
                Process.Start(Settings.Default.RedmineURL + "issues");
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("btnSend_Click", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void OnSendClick(System.Object sender, System.EventArgs e)
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
                if (string.IsNullOrEmpty(txtComment.Text.Trim()))
                {
                    MessageBox.Show("No comment entered", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clockMode = Mode.Stop;
                WorkTimer.Stop();

                decimal hours = currentTime.Hour + (decimal)currentTime.Minute / 60m;

                var manager = new RedmineManager(Settings.Default.RedmineURL, Settings.Default.ApiKey);

                var response = manager.CreateObject<TimeEntry>(new TimeEntry()
                {
                    Issue = new IdentifiableName() { Id = Int32.Parse(cbIssues.Text) },
                    Activity = new IdentifiableName() { Id = ((TimeEntryActivity)cbActivity.SelectedItem).Id },
                    Comments = txtComment.Text,
                    Hours = hours,
                    SpentOn = DateTime.Now
                });

                txtComment.Text = "";
                btnClock.Text = "Play";
                lblClock.Text = "00:00:00";
                time = 0;
                MessageBox.Show("Time Tracker Saved Successfully", "Thank you", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("btnSend_Click", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        private void OnSetIssue(object sender, KeyEventArgs e)
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
            clockMode = Mode.Stop;
            WorkTimer.Stop();
            btnClock.Text = "Play";
            lblClock.Text = "00:00:00";
            time = 0;
        }

        private void OnWorkTimeElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            time += 1;
            calculateTime();
        }

        private void txtIssueID_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadIssue();
        }

        public struct Time
        {
            public int Hour;
            public int Minute;
            public int Second;
        }

        internal struct LastInput
        {
            public uint cSize;
            public uint dtime;
        }
    }
}