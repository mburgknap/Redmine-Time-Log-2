
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RedmineLog.Properties;
using Redmine.Enumerations;
using Redmine.Net.Api;
using System.Collections.Specialized;
using Redmine.Net.Api.Types;

namespace RedmineLog
{
    public partial class frmMain2 : Form
    {
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LastInput rLI);

        internal struct LastInput
        {
            public uint cSize;
            public uint dtime;
        }


        private System.Timers.Timer withEventsField__tmrActivity;
        private System.Timers.Timer _tmrActivity
        {
            get { return withEventsField__tmrActivity; }
            set
            {
                if (withEventsField__tmrActivity != null)
                {
                    withEventsField__tmrActivity.Elapsed -= _tmr_Elapsed;
                }
                withEventsField__tmrActivity = value;
                if (withEventsField__tmrActivity != null)
                {
                    withEventsField__tmrActivity.Elapsed += _tmr_Elapsed;
                }
            }
        }
        private System.Timers.Timer withEventsField__idleCheck;
        private System.Timers.Timer _idleCheck
        {
            get { return withEventsField__idleCheck; }
            set
            {
                if (withEventsField__idleCheck != null)
                {
                    withEventsField__idleCheck.Elapsed -= _activityTimer_Elapsed;
                }
                withEventsField__idleCheck = value;
                if (withEventsField__idleCheck != null)
                {
                    withEventsField__idleCheck.Elapsed += _activityTimer_Elapsed;
                }
            }

        }
        private int _idleTime;
        private int _time;
        private Mode _mode = Mode.Stop;
        private Time _currentTime;

        private bool _close = false;
        private bool _visible = false;

        private bool _hideTooltip = false;



        public struct Time
        {
            public int Hour;
            public int Minute;
            public int Second;
        }

        public enum Mode
        {
            Play,
            Pause,
            Stop
        }

        public frmMain2()
        {
            FormClosing += frmMain_FormClosing;
            CheckForIllegalCrossThreadCalls = false;
            _tmrActivity = new System.Timers.Timer(1000);
            _idleCheck = new System.Timers.Timer(1000);
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }


        private void frmMain_Load(object sender, System.EventArgs e)
        {
            try
            {
                bool saveSettings = false;

                bool.TryParse(Settings.Default.SaveSettings, out saveSettings);

                if (!saveSettings)
                {
                    var objSettings = new frmSettings2();

                    if (objSettings.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                }

                listActivites();
                _idleCheck.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Message: {1}{0}StackTrace: {2}", System.Environment.NewLine, ex.Message, ex.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _close = true;
                Application.Exit();
            }
        }


        private void listActivites()
        {
            try
            {
                Redmine.Enumerations.Activities obj = new Redmine.Enumerations.Activities();

                //cmbActivity.Items.Clear()
                cmbActivity.DataSource = obj.Activites;
                cmbActivity.DisplayMember = "Name";
                cmbActivity.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Source:{3}{0}Message: {1}{0}StackTrace: {2}", System.Environment.NewLine, ex.Message, ex.StackTrace, ex.Source), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _close = true;
                Application.Exit();
            }
        }


        private void btnStop_Click(System.Object sender, System.EventArgs e)
        {
            listActivites();
            _mode = Mode.Stop;
            _tmrActivity.Stop();
            btnClock.Text = "Play";
            lblClock.Text = "00:00:00";
            _time = 0;
        }

        private void btnClock_Click(System.Object sender, System.EventArgs e)
        {
            switch (_mode)
            {
                case Mode.Pause:
                    _mode = Mode.Play;
                    _tmrActivity.Start();
                    btnClock.Text = "Pause";
                    break;
                case Mode.Play:
                    _mode = Mode.Pause;
                    _tmrActivity.Stop();
                    btnClock.Text = "Play";
                    break;
                case Mode.Stop:
                    _mode = Mode.Play;
                    _tmrActivity.Start();
                    _time = 0;
                    btnClock.Text = "Pause";
                    break;
            }
        }


        private void _tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _time += 1;
            calculateTime();
        }

        private void calculateTime()
        {
            int h = 0;
            int m = 0;
            int s = 0;
            int remainder = 0;
            //_time = 5000
            h = Math.DivRem(_time, 3600, out remainder);
            m = Math.DivRem(remainder, 60, out s);
            if (s == 0 & m == 0 & h == 0)
            {
                s = _time;
            }
            _currentTime.Hour = h;
            _currentTime.Minute = m;
            _currentTime.Second = s;

            lblClock.Text = h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
            ntIcon.Text = lblClock.Text;
            if (_hideTooltip == false)
            {
                ntIcon.BalloonTipText = lblClock.Text;
                ntIcon.ShowBalloonTip(1000);
            }
        }



        private void _activityTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
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
                if (_tmrActivity.Enabled == true)
                {
                    _tmrActivity.Enabled = false;
                }
            }
            else
            {
                if (_tmrActivity.Enabled == false & _mode == Mode.Play)
                {
                    _tmrActivity.Enabled = true;
                }
            }

        }

        private void Label1_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Process.Start(Settings.Default.RedmineURL + "issues");

            }
            catch (Exception ex)
            {
            }
        }


        private void btnSend_Click(System.Object sender, System.EventArgs e)
        {

            int n;
            bool isNumeric = int.TryParse(txtIssueID.Text, out n);

            if (isNumeric == false)
            {
                MessageBox.Show("Invalid project ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                MessageBox.Show("No comment entered", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _mode = Mode.Stop;
            _tmrActivity.Stop();
            bool rslt = false;
            int entryID = Int32.Parse(txtIssueID.Text);
            Redmine.TimeEntry objTimeEntry = new Redmine.TimeEntry(entryID);
            float hours = _currentTime.Hour + (float)_currentTime.Minute / 60f;
            int activityID = 0;

            activityID = ((Activity)cmbActivity.SelectedItem).ID;

            rslt = objTimeEntry.RecordEntry(DateTime.Now, hours, activityID, txtComment.Text, Redmine.TimeEntry.Type.Issue);


            if ((rslt == true))
            {
                txtComment.Text = "";
                btnClock.Text = "Play";
                lblClock.Text = "00:00:00";
                _time = 0;
                MessageBox.Show("Time Tracker Saved Successfully", "Thank you", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Label4_Click(System.Object sender, System.EventArgs e)
        {
            frmSettings2 obj = new frmSettings2();

            obj.ShowDialog();
        }




        private void frmMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (_close == false)
            {
                e.Cancel = true;
                this.Hide();

            }
            else
            {
            }
        }

        private void lnkToggle_Click(System.Object sender, System.EventArgs e)
        {

        }

        private void ntIcon_MouseDoubleClick(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();
        }

        private void btnExit_Click(System.Object sender, System.EventArgs e)
        {
            _close = true;
            Application.Exit();
        }

        private void HideTooltipToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {

        }



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(llIssueUrl.Text);
        }

        private void txtIssueID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadIssue();
            }
        }

        private void LoadIssue()
        {
            int isId = 0;

            if (!int.TryParse(txtIssueID.Text, out isId))
            {
                lblIssue.Text = "";
                return;
            }

            var manager = new RedmineManager(Settings.Default.RedmineURL, Settings.Default.ApiKey);

            var parameters = new NameValueCollection { };

            var responseList = manager.GetObjectList<Issue>(parameters);

            Issue mainIssue = null;

            foreach (var issue in responseList)
            {

                if (issue.Id == isId)
                {
                    mainIssue = issue;
                    lblIssue.Text = issue.Subject;
                    llIssueUrl.Text = Settings.Default.RedmineURL + "issues/" + isId;
                    lblIssue.Visible = true;
                    llIssueUrl.Visible = true;

                    if (!txtIssueID.Items.Contains(isId))
                        txtIssueID.Items.Add(isId);
                    break;
                }
            }

            if (mainIssue == null)
            {
                lblIssue.Text = "";
                lblIssue.Visible = false;
                llIssueUrl.Text = "";
                llIssueUrl.Visible = false;

            }

            if (mainIssue != null
                && mainIssue.ParentIssue != null)
                foreach (var issue in responseList)
                {

                    if (issue.Id == mainIssue.ParentIssue.Id)
                    {
                        lblParentIssue.Text = issue.Subject;
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

        private void txtIssueID_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadIssue();
        }



    }

}
