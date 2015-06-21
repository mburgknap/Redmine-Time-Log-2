using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using RedmineLog.Logic;
using RedmineLog.Logic.Data;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmMain : Form
    {
        /*     public enum SubmitMode
             {
                 Work,
                 Idle,
                 All
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
             private System.Timers.Timer workTimer;
             private bool saveIdleTimePopup = false;
             private int saveIdleTimePopupTime;

             private Thread backgroundThread;*/

        public frmMain()
        {
            InitializeComponent();
            this.Initialize<Main.IView, frmMain>();
            CheckForIllegalCrossThreadCalls = false;
            lblVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        private void OnMainLoad(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }
        /*
                public enum ClockMode
                {
                    Play,
                    Pause,
                    Stop
                }



      
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

                        foreach (var item in responseList)
                        {
                            App.Context.Activity.Add(item.Id, item.Name);
                        }

                        cbActivity.Items.Clear();
                        cbActivity.DataSource = App.Context.Activity;
                        cbActivity.DisplayMember = "Name";
                        cbActivity.ValueMember = "Id";

                        if (cbActivity.Items.Count > 0)
                            cbActivity.SelectedItem = cbActivity.Items[0];

                        App.Context.History.Load();
                        App.Context.IssuesCache.Load();
                        App.Context.Work.Load();

                        idleTime = App.Context.Work.IdleTime;
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

                private void LoadIssue(bool inForce = false)
                {
                    try
                    {
                        bool cacheChanged = false;

                        tbComment.Text = "";
                        tbComment.ReadOnly = true;

                        int newIssueId = 0;

                        if (string.IsNullOrWhiteSpace(tbIssue.Text))
                        {
                            newIssueId = -1;
                        }
                        else
                            if (!int.TryParse(tbIssue.Text, out newIssueId))
                            {
                                lblProject.Text = "";
                                lblProject.Visible = false;
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

                        if (issueData != null && issueData.Id != newIssueId && clockMode == ClockMode.Play)
                        {
                            SaveIssueWorkTime(issueData);
                        }

                        issueComment = null;
                        tbComment.Text = "";
                        tbComment.ReadOnly = true;

                        var tmpIssue = App.Context.IssuesCache.GetIssue(newIssueId);

                        if (inForce && tmpIssue != null)
                        {
                            App.Context.IssuesCache.RemoveIssue(tmpIssue.Id);

                            if (tmpIssue.IdParent.HasValue)
                                App.Context.IssuesCache.RemoveIssue(tmpIssue.IdParent.Value);

                            tmpIssue = null;
                        }

                        if (tmpIssue == null)
                        {
                            var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);
                            var parameters = new NameValueCollection { };

                            var issue = manager.GetObject<Issue>(newIssueId.ToString(), parameters);
                            var project = manager.GetObject<Project>(issue.Project.Id.ToString(), parameters);

                            tmpIssue = new RedmineIssues.Item(issue, project);

                            App.Context.IssuesCache.Add(tmpIssue);
                            issueData = new LogData.Issue(tmpIssue.Id);
                            cacheChanged = true;
                        }

                        if (tmpIssue == null)
                        {
                            lblProject.Text = "";
                            lblProject.Visible = false;
                            lblIssue.Text = "";
                            lblIssue.Visible = false;
                            btnRemoveItem.Visible = false;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(tmpIssue.Project))
                            {
                                lblProject.Text = "Project : " + tmpIssue.Project;
                                lblProject.Visible = true;
                            }
                            else
                            {
                                lblProject.Text = "";
                                lblProject.Visible = false;
                            }

                            if (!string.IsNullOrWhiteSpace(tmpIssue.Subject))
                            {
                                lblIssue.Text = "Task : " + tmpIssue.Subject;
                                lblIssue.Tag = App.Context.Config.Url + "issues/" + newIssueId;
                                lblIssue.Visible = true;

                                btnRemoveItem.Visible = true;
                            }
                            else
                            {
                                lblIssue.Text = "";
                                lblIssue.Visible = false;
                                btnRemoveItem.Visible = false;
                            }

                            var tmp = App.Context.History.GetIssue(newIssueId);

                            if (tmp == null)
                            {
                                tmp = new LogData.Issue(newIssueId);
                                App.Context.History.Add(tmp);
                                App.Context.History.Save();
                            }

                            issueData = tmp;
                        }

                        if (tmpIssue != null
                            && tmpIssue.IdParent.HasValue)
                        {
                            var tmpParentIssue = App.Context.IssuesCache.GetIssue(tmpIssue.IdParent.Value);

                            if (tmpParentIssue == null)
                            {
                                var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);
                                var parameters = new NameValueCollection { };

                                tmpParentIssue = new RedmineIssues.Item(manager.GetObject<Issue>(tmpIssue.IdParent.Value.ToString(), parameters), null);

                                App.Context.IssuesCache.Add(tmpParentIssue);
                                cacheChanged = true;
                            }

                            lblParentIssue.Text = "Issue : " + tmpParentIssue.Subject;
                            lblParentIssue.Visible = true;
                        }
                        else
                        {
                            lblParentIssue.Text = "";
                            lblParentIssue.Visible = false;
                        }

                        if (cacheChanged)
                            App.Context.IssuesCache.Save();

                        SetupIssueWorkTime(issueData);
                        ManageHide();
                    }
                    catch (Exception ex)
                    {
                        AppLogger.Log.Error("LoadIssue", ex);
                        MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
                    }
                }

                private void SetupIssueWorkTime(LogData.Issue mainIssue)
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

                private void SaveIssueWorkTime(LogData.Issue mainIssue)
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
                    App.Context.Work.IdleTime = idleTime;
                    if (issueData != null)
                        SaveIssueWorkTime(issueData);
                    else
                        App.Context.Work.Save();

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
                    this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);

                     return;

                     try
                     {
                         if (!App.Context.Config.Load())
                         {
                             var objSettings = new frmSettings();

                             if (objSettings.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                             {
                                 Application.Exit();
                                 return;
                             }
                         }

                         saveIdleTimePopupTime = App.Context.Config.SnoozeTime;
                         lblIssue.Text = "";

                         LoadAppData();

                         backgroundThread = new Thread(new ThreadStart(this.BackgroundService));
                         backgroundThread.Start();
                         IdleTimer.Start();
                     }
                     catch (Exception ex)
                     {
                         AppLogger.Log.Error("frmMain_Load", ex);
                         MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
                         Application.Exit();
                     }
                }
        
                private void BackgroundService()
                {
                    while (!close)
                    {
                        if (this.WindowState != FormWindowState.Minimized && WorkTimer.Enabled && DateTime.Now.Minute % 2 == 0)
                        {
                            OnHideClick(this, EventArgs.Empty);
                        }

                        if (!saveIdleTimePopup && new TimeSpan(idleTime.Hour, idleTime.Minute, idleTime.Second).TotalMinutes > saveIdleTimePopupTime)
                        {
                            saveIdleTimePopup = true;

                            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                            {
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show("Zapisz czas nieaktywności", "Przypomienie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    OnClockIdleClick(lblClockIndle, null);
                                    saveIdleTimePopupTime += App.Context.Config.SnoozeTime;
                                    saveIdleTimePopup = false;
                                }));
                            }));
                        }

                        Thread.Sleep(1000 * App.Context.Config.ServiceSleepTime);
                    }
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
                    if (totalIdleTimeInSeconds > App.Context.Config.IdleStateWaitTime)
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
                    SubmitWorkTime(submitMode);
                }

                private void SubmitWorkTime(SubmitMode inSubmitMode)
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

                        decimal hours = decimal.Zero;

                        switch (inSubmitMode)
                        {
                            case SubmitMode.Work:
                                {
                                    hours = (decimal)(workTime.Hour * 60 + workTime.Minute) / 60m;
                                    break;
                                }
                            case SubmitMode.Idle:
                                {
                                    hours = (decimal)(idleTime.Hour * 60 + idleTime.Minute) / 60m;
                                    break;
                                }
                            case SubmitMode.All:
                                {
                                    hours = ((decimal)(workTime.Hour * 60 + workTime.Minute) / 60m) + ((decimal)(idleTime.Hour * 60 + idleTime.Minute) / 60m);
                                    break;
                                }
                        }

                        var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);

                        var response = manager.CreateObject<TimeEntry>(new TimeEntry()
                        {
                            Issue = new IdentifiableName() { Id = issueData.Id },
                            Activity = new IdentifiableName() { Id = ((ActivityData.TaskActivity)cbActivity.SelectedItem).Id },
                            Comments = tbComment.Text,
                            Hours = decimal.Round(hours, 2),
                            SpentOn = DateTime.Now
                        });

                        switch (inSubmitMode)
                        {
                            case SubmitMode.Work:
                                {
                                    SetClockMode(ClockMode.Stop);
                                    SetText(lblClockActive, workTime.ToLongTimeString());
                                    App.Context.Work.RemoveId(issueData.Id);
                                    break;
                                }
                            case SubmitMode.Idle:
                                {
                                    idleTime = DateTime.MinValue;
                                    SetText(lblClockIndle, idleTime.ToLongTimeString());
                                    break;
                                }
                            case SubmitMode.All:
                                {
                                    SetClockMode(ClockMode.Stop);
                                    SetText(lblClockActive, workTime.ToLongTimeString());
                                    App.Context.Work.RemoveId(issueData.Id);
                                    idleTime = DateTime.MinValue;
                                    SetText(lblClockIndle, idleTime.ToLongTimeString());
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        AppLogger.Log.Error("btnSend_Click", ex);
                        MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
                    }
                }

                private void ManageHide()
                {
                    if (clockMode == ClockMode.Play && issueData != null && issueData.Id > 0)
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
                    tbComment.Focus();
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

                        if (tmp != null && issueComment != null)
                        {
                            tmp.IdComment = issueComment.Id;
                            App.Context.Work.Save();
                        }
                        else
                        {
                            OnNewCommentClick(this, EventArgs.Empty);
                            tbComment.Text = e.ClickedItem.Text;
                            issueComment.Text = e.ClickedItem.Text;
                            AcceptComment();
                        }
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
                    if (issueComment != null
                        && !issueComment.Text.Equals(tbComment.Text))
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
                    else if (WindowState == FormWindowState.Minimized
                        && issueData != null)
                    {
                        var issue = App.Context.IssuesCache.GetIssue(issueData.Id);

                        small = new frmSmall(this);

                        small.SetMainIssue(issue);

                        if (issue.IdParent.HasValue)
                            issue = App.Context.IssuesCache.GetIssue(issue.IdParent.Value);
                        else
                            issue = null;

                        small.SetParentIssue(issue);
                        small.UpdateWorkTime(workTime);
                        small.UpdateIdleTime(idleTime);
                        small.ShowDialog();
                        System.Diagnostics.Debug.WriteLine(WindowState.ToString());
                    }
                }



                private void OnIssueKeyDown(object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == (Keys.LButton | Keys.MButton | Keys.Back))
                        LoadIssue(true);
                }


                private void OnIssueMouseClick(object sender, MouseEventArgs e)
                {
                    tbIssue.SelectAll();
                    tbIssue.Focus();
                }

                private void OnClockClick(object sender, EventArgs e)
                {
                    if (clockMode == ClockMode.Play)
                    { SetClockMode(ClockMode.Pause); return; }

                    if (clockMode == ClockMode.Pause
                        || clockMode == ClockMode.Stop)
                    { SetClockMode(ClockMode.Play); }
                }

                private void OnFormClosed(object sender, FormClosedEventArgs e)
                {
                    close = true;
                }

                private void OnHideClick(object sender, EventArgs e)
                {
                    if (clockMode == ClockMode.Play
                        && issueData != null
                        && issueData.Id > 0)
                        WindowState = FormWindowState.Minimized;
                }

                private void OnSubmitAllClick(object sender, EventArgs e)
                {
                    SubmitWorkTime(SubmitMode.All);
                }

                private void btnWorkTime_Click(object sender, EventArgs e)
                {
                    AcceptComment();

                    var worklog = new frmWorkLog();
                    worklog.Init(this.Location);
                    worklog.OnSelect = (id, comment) =>
                    {
                        tbIssue.Text = id.ToString();
                        LoadIssue();

                        if (issueData != null)
                        {
                            var tmpIssue = App.Context.History.GetIssue(issueData.Id);

                            foreach (var item in tmpIssue.Comments)
                            {
                                if (!item.IsGlobal && String.Equals(item.Text, comment))
                                {
                                    issueComment = item;
                                    tbComment.ReadOnly = false;
                                    tbComment.Text = comment;
                                    break;
                                }
                            }

                            if (issueComment == null)
                            {
                                OnNewCommentClick(null, EventArgs.Empty);
                                tbComment.Text = comment;
                                AcceptComment();
                            }
                        }
                    };
                    worklog.ShowDialog();
                }

                private void tbComment_KeyPress(object sender, KeyPressEventArgs e)
                {
                    if (e.KeyChar == 19)
                    {
                        AcceptComment();
                    }
                }*/
    }



    internal class MainView : Main.IView, IView<frmMain>
    {
        private Main.IModel model;

        private frmMain Form;

        [Inject]
        public MainView(Main.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            inGlobalEvent.Register(this);
            model.Sync.Bind(SyncTarget.View, this);
            AppTimers.Init(inGlobalEvent);
        }

        [EventPublication(Main.Events.Load, typeof(Publish<Main.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(Main.Events.Exit, typeof(Publish<Main.IView>))]
        public event EventHandler ExitEvent;

        [EventPublication(Main.Events.AddComment, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> AddCommentEvent;

        [EventPublication(Main.Events.UpdateComment, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> UpdateCommentEvent;

        [EventPublication(Main.Events.DelComment, typeof(Publish<Main.IView>))]
        public event EventHandler DelCommentEvent;

        [EventPublication(Main.Events.AddIssue, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> AddIssueEvent;

        [EventPublication(Main.Events.DelIssue, typeof(Publish<Main.IView>))]
        public event EventHandler DelIssueEvent;

        [EventPublication(Main.Events.Submit, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<Main.Actions>> SubmitEvent;

        [EventPublication(Main.Events.Reset, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<Main.Actions>> ResetEvent;

        [EventPublication(Main.Events.Link, typeof(Publish<Main.IView>))]
        public event EventHandler<Args<string>> GoLinkEvent;

        [EventSubscription(AppTimers.WorkUpdate, typeof(OnPublisher))]
        public void OnWorkUpdateEvent(object sender, Args<int> arg)
        {
            model.WorkTime = model.WorkTime.Add(new TimeSpan(0, 0, arg.Data));
            model.Sync.Value(SyncTarget.View, "WorkTime");
        }

        [EventSubscription(AppTimers.IdleUpdate, typeof(OnPublisher))]
        public void OnIdleUpdateEvent(object sender, Args<int> arg)
        {
            model.IdleTime = model.IdleTime.Add(new TimeSpan(0, 0, arg.Data));
            model.Sync.Value(SyncTarget.View, "IdleTime");
        }

        void OnWorkTimeChange()
        {
            SetText(Form.lblClockActive, model.WorkTime.ToString());
        }

        void OnIdleTimeChange()
        {
            SetText(Form.lblClockIndle, model.IdleTime.ToString());
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

        private Main.Actions currentMode;

        public void Init(frmMain inView)
        {
            Form = inView;
            Form.tbIssue.KeyDown += AddIssue;
            Form.btnRemoveItem.Click += DelIssue;
            Form.btnComments.Click += LoadComment;
            Form.btnNewComment.Click += AddComment;
            Form.btnRemoveComment.Click += DelComment;
            Form.tbComment.KeyDown += SaveComment;
            Form.cmComments.ItemClicked += SelectComment;
            Form.btnIssues.Click += OnSearchIssueClick;
            Form.btnSubmit.Click += OnSubmitClick;
            Form.btnSubmitAll.Click += OnSubmitAllClick;
            Form.btnResetIdle.Click += OnResetClockClick;
            Form.lHide.Click += OnHideClick;
            Form.lnkExit.Click += OnExitClick;
            Form.lblClockActive.Click += OnWorkMode;
            Form.lblClockIndle.Click += OnIdleMode;
            Form.cbActivity.SelectedIndexChanged += OnActivityTypeChange;
            Form.lnkSettings.Click += OnSettingClick;
            Form.lnkIssues.Click += OnRedmineIssuesLink;
            Form.lblIssue.Click += OnRedmineIssueLink;
            Form.tbComment.Click += OnCommentClick;

            OnWorkMode(this, EventArgs.Empty);

            LoadEvent.Fire(this);
        }

        private void OnCommentClick(object sender, EventArgs e)
        {
            if (Form.tbComment.ReadOnly)
            {
                LoadComment(Form.btnComments, EventArgs.Empty);
            }
        }

        private void OnRedmineIssuesLink(object sender, EventArgs e)
        {
            GoLinkEvent.Fire(this, "Redmine");
        }

        private void OnRedmineIssueLink(object sender, EventArgs e)
        {
            GoLinkEvent.Fire(this, "Issue");
        }

        private void OnSettingClick(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
        }

        private void OnActivityTypeChange(object sender, EventArgs e)
        {
            if (model.WorkActivities.Count > Form.cbActivity.SelectedIndex)
                model.Activity = model.WorkActivities[Form.cbActivity.SelectedIndex];
        }


        private void OnIdleMode(object sender, EventArgs e)
        {
            Form.pManage.BackColor = Color.Azure;
            currentMode = Main.Actions.Idle;
        }

        private void OnWorkMode(object sender, EventArgs e)
        {
            Form.pManage.BackColor = Color.Wheat;
            currentMode = Main.Actions.Issue;
        }

        private void OnResetClockClick(object sender, EventArgs e)
        {
            ResetEvent.Fire(this, currentMode);
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
            ExitEvent.Fire(this);
            Form.Close();
        }

        private void OnHideClick(object sender, EventArgs e)
        {
            Form.WindowState = FormWindowState.Minimized;

            var form = new frmSmall();

            form.FormClosed += (s, arg) =>
            {
                Form.WindowState = FormWindowState.Normal;
            };

            form.ShowDialog();
        }

        private void OnSubmitAllClick(object sender, EventArgs e)
        {
            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
            SubmitEvent.Fire(this, Main.Actions.All);
        }

        private void OnSubmitClick(object sender, EventArgs e)
        {
            UpdateCommentEvent.Fire(this, Form.tbComment.Text);
            SubmitEvent.Fire(this, currentMode);
        }

        private void SelectComment(object sender, ToolStripItemClickedEventArgs e)
        {
            var comment = e.ClickedItem.Tag as CommentData;

            if (comment.IsGlobal)
                AddCommentEvent.Fire(this, comment.Text);
            else
            {
                model.Comment = e.ClickedItem.Tag as CommentData;
                model.Sync.Value(SyncTarget.View, "Comment");
            }
        }

        private void SaveComment(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                UpdateCommentEvent.Fire(this, Form.tbComment.Text);
        }

        private void DelComment(object sender, EventArgs e)
        {
            DelCommentEvent.Fire(this);
        }

        private void AddComment(object sender, EventArgs e)
        {
            AddCommentEvent.Fire(this, Form.tbComment.Text);
        }

        private void DelIssue(object sender, EventArgs e)
        {
            DelIssueEvent.Fire(this);
        }

        private void AddIssue(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                AddIssueEvent.Fire(this, Form.tbIssue.Text);
        }

        private void OnSearchIssueClick(object sender, EventArgs e)
        {
            new frmSearch().ShowDialog();
        }

        private void LoadComment(object sender, EventArgs e)
        {
            Form.cmComments.Items.Clear();

            var list = model.IssueComments.Where(x => !x.IsGlobal).ToList();

            if (list.Count > 0)
            {
                Form.cmComments.Items.Add(new ToolStripSeparator());
                var tmpStrip = new ToolStripStatusLabel("Issue comments");
                tmpStrip.Font = new Font(tmpStrip.Font, FontStyle.Bold);
                Form.cmComments.Items.Add(tmpStrip);
                Form.cmComments.Items.Add(new ToolStripSeparator());
            }

            foreach (var item in list)
            {
                var cmItem = Form.cmComments.Items.Add(item.Text);
                cmItem.Overflow = ToolStripItemOverflow.AsNeeded;
                cmItem.Tag = item;
            }


            list = model.IssueComments.Where(x => x.IsGlobal).ToList();

            if (list.Count > 0)
            {
                Form.cmComments.Items.Add(new ToolStripSeparator());

                var tmpStrip = new ToolStripStatusLabel("Global comments");
                tmpStrip.Font = new Font(tmpStrip.Font, FontStyle.Bold);
                Form.cmComments.Items.Add(tmpStrip);
                Form.cmComments.Items.Add(new ToolStripSeparator());
            }

            foreach (var item in list)
            {
                var cmItem = Form.cmComments.Items.Add(item.Text);
                cmItem.Overflow = ToolStripItemOverflow.AsNeeded;
                cmItem.Tag = item;
            }

            if (Form.cmComments.Items.Count > 0)
                Form.cmComments.Show(Form.tbComment, 0, 0);

        }

        void OnCommentChange()
        {
            Form.tbComment.Text = model.Comment != null ? model.Comment.Text : string.Empty;
            Form.tbComment.ReadOnly = model.Comment == null;
        }
        void OnWorkActivitiesChange()
        {
            Form.cbActivity.Items.Clear();
            Form.cbActivity.DataSource = model.WorkActivities;
            Form.cbActivity.DisplayMember = "Name";
            Form.cbActivity.ValueMember = "Id";

            if (Form.cbActivity.Items.Count > 0)
            {
                Form.cbActivity.SelectedItem = Form.cbActivity.Items[0];
                model.Activity = model.WorkActivities[0];
            }
        }
        void OnIssueParentInfoChange()
        {
            if (model.IssueParentInfo != null)
            {
                Form.lblParentIssue.Text = model.IssueParentInfo.Subject + " :";
                Form.lblParentIssue.Visible = true;
            }
            else
                Form.lblParentIssue.Visible = false;
        }
        void OnIssueInfoChange()
        {
            Form.tbIssue.Text = model.IssueInfo.Id > 0 ? model.IssueInfo.Id.ToString() : "";

            Form.lblProject.Text = model.IssueInfo.Project;
            Form.lblTracker.Text = model.IssueInfo.Id > 0 ? "(" + model.IssueInfo.Tracker + ")" : "";
            Form.lblIssue.Text = model.IssueInfo.Subject;

            Form.btnRemoveItem.Visible = model.IssueInfo.Id > 0;
            Form.lHide.Visible = model.IssueInfo.Id > 0;
        }

        public void Info(string inMessage)
        {
            MessageBox.Show(inMessage);
        }

        public void GoLink(Uri inUri)
        {
            try
            {
                System.Diagnostics.Process.Start(inUri.ToString());
            }
            catch (Exception ex)
            {
                AppLogger.Log.Error("GoLink", ex);
                MessageBox.Show("Error occured, error detail saved in application logs ", "Warrnig");
            }
        }
    }
}