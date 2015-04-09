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

namespace RedmineLog
{



    public partial class frmMain : Form
    {
        public enum SubmitMode
        {
            Work,
            Idle
        }

        private SubmitMode submitMode = SubmitMode.Work;
        private ClockMode clockMode = ClockMode.Stop;
        private bool close = false;

        private DateTime startTime;
        private DateTime idleTime;

        private bool hideTooltip = false;
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
            if (!string.IsNullOrWhiteSpace(cbIssues.Text)
                && ContainIssue(cbIssues.Text))
            {
                Settings.Default.WorkingIssueList = Settings.Default.WorkingIssueList.Replace(cbIssues.Text + ";", ";");
                var item = cbIssues.SelectedItem;
                cbIssues.SelectedItem = cbIssues.Items[0];
                cbIssues.Items.Remove(item);

                Settings.Default.Save();
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
                    lblIssue.Tag = null;
                    lblIssue.Visible = false;
                    lblParentIssue.Text = "";
                    lblParentIssue.Visible = false;
                    btnRemoveItem.Visible = false;
                    return;
                }

                var manager = new RedmineManager(Settings.Default.RedmineURL, Settings.Default.ApiKey);

                var parameters = new NameValueCollection { };

                mainIssue = manager.GetObject<Issue>(isId.ToString(), parameters);
                parentIssue = null;

                lblIssue.Text = mainIssue.Subject;
                lblIssue.Tag = Settings.Default.RedmineURL + "issues/" + isId;
                lblIssue.Visible = true;
                btnRemoveItem.Visible = true;

                if (!ContainIssue(isId))
                {
                    cbIssues.Items.Add(isId);
                    Settings.Default.WorkingIssueList += isId + ";";
                    Settings.Default.Save();
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
                    startTime = DateTime.MinValue;
                    btnClock.Text = "Play";
                    break;

                case ClockMode.Stop:
                    clockMode = ClockMode.Play;
                    WorkTimer.Start();
                    startTime = DateTime.MinValue;
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
            if (totalIdleTimeInSeconds > 5)
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
                lblClockIndle.Text = idleTime.ToString("HH:mm:ss");
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
                Process.Start(Settings.Default.RedmineURL + "issues");
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
                if (string.IsNullOrEmpty(txtComment.Text.Trim()))
                {
                    MessageBox.Show("No comment entered", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                DateTime time = submitMode == SubmitMode.Work ? startTime : idleTime;

                decimal hours = (decimal)(time.Hour * 60 + time.Minute) / 60m;

                var manager = new RedmineManager(Settings.Default.RedmineURL, Settings.Default.ApiKey);

                var response = manager.CreateObject<TimeEntry>(new TimeEntry()
                {
                    Issue = new IdentifiableName() { Id = Int32.Parse(cbIssues.Text) },
                    Activity = new IdentifiableName() { Id = ((TimeEntryActivity)cbActivity.SelectedItem).Id },
                    Comments = txtComment.Text,
                    Hours = decimal.Round(hours, 2),
                    SpentOn = DateTime.Now
                });

                txtComment.Text = "";

                if (submitMode == SubmitMode.Work)
                {
                    OnStopClick(btnStop, null);
                }
                else
                {
                    idleTime = DateTime.MinValue;
                    lblClockIndle.Text = idleTime.ToString("HH:mm:ss");
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
            WorkTimer.Stop();
            btnClock.Text = "Play";
            startTime = DateTime.MinValue;
            lblClockActive.Text = startTime.ToString("HH:mm:ss");
        }

        private void OnWorkTimeElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (clockMode == ClockMode.Play)
            {
                startTime = startTime.AddSeconds(1);
                lblClockActive.Text = startTime.ToString("HH:mm:ss");
            }
        }

        private void OnIssueIDChanged(object sender, EventArgs e)
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
            idleTime = DateTime.MinValue;
            lblClockIndle.Text = idleTime.ToString("HH:mm:ss");
        }

    }
}