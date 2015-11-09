namespace RedmineLog
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lnkExit = new System.Windows.Forms.Label();
            this.lnkSettings = new System.Windows.Forms.Label();
            this.lnkIssues = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.lblClockActive = new System.Windows.Forms.Label();
            this.cbActivity = new System.Windows.Forms.ComboBox();
            this.lblParentIssue = new System.Windows.Forms.Label();
            this.lblIssue = new System.Windows.Forms.LinkLabel();
            this.issueInfoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblProject = new System.Windows.Forms.Label();
            this.lblTracker = new System.Windows.Forms.Label();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblClockIdle = new System.Windows.Forms.Label();
            this.pManage = new System.Windows.Forms.Panel();
            this.btnStopWork = new System.Windows.Forms.Button();
            this.lbClockTodayTime = new System.Windows.Forms.Label();
            this.cbResolveIssue = new System.Windows.Forms.CheckBox();
            this.btnIssueMode = new System.Windows.Forms.Button();
            this.btnSubmitAll = new System.Windows.Forms.Button();
            this.btnComments = new System.Windows.Forms.Button();
            this.tbIssue = new System.Windows.Forms.TextBox();
            this.pComments = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemoveComment = new System.Windows.Forms.Button();
            this.btnNewComment = new System.Windows.Forms.Button();
            this.btnResetIdle = new System.Windows.Forms.Button();
            this.cmComments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lHide = new System.Windows.Forms.Label();
            this.cmIssuesKind = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmMyBugs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMyIssues = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMyWork = new System.Windows.Forms.ToolStripMenuItem();
            this.ttStartTime = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.llDay2 = new System.Windows.Forms.LinkLabel();
            this.llDay1 = new System.Windows.Forms.LinkLabel();
            this.llDay3 = new System.Windows.Forms.LinkLabel();
            this.llDay4 = new System.Windows.Forms.LinkLabel();
            this.llDay5 = new System.Windows.Forms.LinkLabel();
            this.llDay6 = new System.Windows.Forms.LinkLabel();
            this.llDay7 = new System.Windows.Forms.LinkLabel();
            this.btnWorkReportSync = new System.Windows.Forms.Button();
            this.btnWorkReportMode = new System.Windows.Forms.Button();
            this.cHeader = new RedmineLog.UI.Items.IssueItemView();
            this.fpIssueList = new RedmineLog.UI.Common.ExFlowLayoutPanel(this.components);
            this.issueInfoPanel.SuspendLayout();
            this.pManage.SuspendLayout();
            this.pComments.SuspendLayout();
            this.panel1.SuspendLayout();
            this.cmIssuesKind.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkExit
            // 
            this.lnkExit.AutoSize = true;
            this.lnkExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkExit.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExit.ForeColor = System.Drawing.Color.Blue;
            this.lnkExit.Location = new System.Drawing.Point(534, 7);
            this.lnkExit.Name = "lnkExit";
            this.lnkExit.Size = new System.Drawing.Size(29, 17);
            this.lnkExit.TabIndex = 31;
            this.lnkExit.Text = "Exit";
            // 
            // lnkSettings
            // 
            this.lnkSettings.AutoSize = true;
            this.lnkSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkSettings.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSettings.ForeColor = System.Drawing.Color.Blue;
            this.lnkSettings.Location = new System.Drawing.Point(397, 7);
            this.lnkSettings.Name = "lnkSettings";
            this.lnkSettings.Size = new System.Drawing.Size(57, 17);
            this.lnkSettings.TabIndex = 27;
            this.lnkSettings.Text = "Settings";
            // 
            // lnkIssues
            // 
            this.lnkIssues.AutoSize = true;
            this.lnkIssues.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkIssues.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkIssues.ForeColor = System.Drawing.Color.Blue;
            this.lnkIssues.Location = new System.Drawing.Point(147, 447);
            this.lnkIssues.Name = "lnkIssues";
            this.lnkIssues.Size = new System.Drawing.Size(103, 17);
            this.lnkIssues.TabIndex = 24;
            this.lnkIssues.Text = "Redmine Issues";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Location = new System.Drawing.Point(151, 231);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(61, 32);
            this.btnSubmit.TabIndex = 22;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // tbComment
            // 
            this.tbComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbComment.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbComment.Location = new System.Drawing.Point(0, 0);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.ReadOnly = true;
            this.tbComment.Size = new System.Drawing.Size(219, 92);
            this.tbComment.TabIndex = 21;
            // 
            // lblClockActive
            // 
            this.lblClockActive.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblClockActive.BackColor = System.Drawing.Color.Transparent;
            this.lblClockActive.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClockActive.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblClockActive.Location = new System.Drawing.Point(-3, -8);
            this.lblClockActive.Name = "lblClockActive";
            this.lblClockActive.Size = new System.Drawing.Size(183, 56);
            this.lblClockActive.TabIndex = 18;
            this.lblClockActive.Text = "00:00:00";
            this.lblClockActive.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cbActivity
            // 
            this.cbActivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbActivity.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbActivity.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbActivity.FormattingEnabled = true;
            this.cbActivity.Location = new System.Drawing.Point(111, 72);
            this.cbActivity.Name = "cbActivity";
            this.cbActivity.Size = new System.Drawing.Size(139, 25);
            this.cbActivity.TabIndex = 17;
            // 
            // lblParentIssue
            // 
            this.lblParentIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblParentIssue.AutoSize = true;
            this.lblParentIssue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblParentIssue.Location = new System.Drawing.Point(5, 19);
            this.lblParentIssue.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblParentIssue.Name = "lblParentIssue";
            this.lblParentIssue.Size = new System.Drawing.Size(6, 19);
            this.lblParentIssue.TabIndex = 33;
            // 
            // lblIssue
            // 
            this.lblIssue.AutoSize = true;
            this.lblIssue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblIssue.Location = new System.Drawing.Point(10, 58);
            this.lblIssue.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblIssue.Name = "lblIssue";
            this.lblIssue.Size = new System.Drawing.Size(0, 19);
            this.lblIssue.TabIndex = 34;
            // 
            // issueInfoPanel
            // 
            this.issueInfoPanel.Controls.Add(this.lblProject);
            this.issueInfoPanel.Controls.Add(this.lblParentIssue);
            this.issueInfoPanel.Controls.Add(this.lblTracker);
            this.issueInfoPanel.Controls.Add(this.lblIssue);
            this.issueInfoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.issueInfoPanel.Location = new System.Drawing.Point(6, 267);
            this.issueInfoPanel.Name = "issueInfoPanel";
            this.issueInfoPanel.Size = new System.Drawing.Size(244, 153);
            this.issueInfoPanel.TabIndex = 35;
            // 
            // lblProject
            // 
            this.lblProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProject.AutoSize = true;
            this.lblProject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblProject.Location = new System.Drawing.Point(0, 0);
            this.lblProject.Margin = new System.Windows.Forms.Padding(0);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(11, 19);
            this.lblProject.TabIndex = 35;
            // 
            // lblTracker
            // 
            this.lblTracker.AutoSize = true;
            this.lblTracker.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTracker.Location = new System.Drawing.Point(3, 38);
            this.lblTracker.Name = "lblTracker";
            this.lblTracker.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.lblTracker.Size = new System.Drawing.Size(5, 20);
            this.lblTracker.TabIndex = 36;
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveItem.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRemoveItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRemoveItem.ForeColor = System.Drawing.Color.Red;
            this.btnRemoveItem.Location = new System.Drawing.Point(91, 231);
            this.btnRemoveItem.Margin = new System.Windows.Forms.Padding(125, 0, 0, 0);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(27, 32);
            this.btnRemoveItem.TabIndex = 35;
            this.btnRemoveItem.Text = "X";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe WP", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblVersion.Location = new System.Drawing.Point(319, 7);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(0, 19);
            this.lblVersion.TabIndex = 37;
            // 
            // lblClockIdle
            // 
            this.lblClockIdle.AutoSize = true;
            this.lblClockIdle.BackColor = System.Drawing.Color.Transparent;
            this.lblClockIdle.Font = new System.Drawing.Font("Segoe WP", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblClockIdle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblClockIdle.Location = new System.Drawing.Point(86, 44);
            this.lblClockIdle.Name = "lblClockIdle";
            this.lblClockIdle.Size = new System.Drawing.Size(88, 25);
            this.lblClockIdle.TabIndex = 38;
            this.lblClockIdle.Text = "00:00:00";
            // 
            // pManage
            // 
            this.pManage.BackColor = System.Drawing.Color.Transparent;
            this.pManage.Controls.Add(this.btnStopWork);
            this.pManage.Controls.Add(this.lbClockTodayTime);
            this.pManage.Controls.Add(this.cbResolveIssue);
            this.pManage.Controls.Add(this.btnIssueMode);
            this.pManage.Controls.Add(this.btnSubmitAll);
            this.pManage.Controls.Add(this.btnComments);
            this.pManage.Controls.Add(this.tbIssue);
            this.pManage.Controls.Add(this.pComments);
            this.pManage.Controls.Add(this.btnResetIdle);
            this.pManage.Controls.Add(this.lblClockIdle);
            this.pManage.Controls.Add(this.btnRemoveItem);
            this.pManage.Controls.Add(this.lblClockActive);
            this.pManage.Controls.Add(this.cbActivity);
            this.pManage.Controls.Add(this.issueInfoPanel);
            this.pManage.Controls.Add(this.btnSubmit);
            this.pManage.Controls.Add(this.lnkIssues);
            this.pManage.Location = new System.Drawing.Point(322, 36);
            this.pManage.Name = "pManage";
            this.pManage.Size = new System.Drawing.Size(259, 473);
            this.pManage.TabIndex = 39;
            // 
            // btnStopWork
            // 
            this.btnStopWork.Font = new System.Drawing.Font("Century Gothic", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopWork.Location = new System.Drawing.Point(180, 5);
            this.btnStopWork.Name = "btnStopWork";
            this.btnStopWork.Size = new System.Drawing.Size(70, 24);
            this.btnStopWork.TabIndex = 50;
            this.btnStopWork.Text = "Stop";
            this.btnStopWork.UseVisualStyleBackColor = true;
            // 
            // lbClockTodayTime
            // 
            this.lbClockTodayTime.AutoSize = true;
            this.lbClockTodayTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbClockTodayTime.Location = new System.Drawing.Point(8, 53);
            this.lbClockTodayTime.Name = "lbClockTodayTime";
            this.lbClockTodayTime.Size = new System.Drawing.Size(57, 13);
            this.lbClockTodayTime.TabIndex = 49;
            this.lbClockTodayTime.Text = "00:00:00";
            // 
            // cbResolveIssue
            // 
            this.cbResolveIssue.AutoSize = true;
            this.cbResolveIssue.Location = new System.Drawing.Point(160, 205);
            this.cbResolveIssue.Name = "cbResolveIssue";
            this.cbResolveIssue.Size = new System.Drawing.Size(92, 17);
            this.cbResolveIssue.TabIndex = 48;
            this.cbResolveIssue.Text = "Resolve issue";
            this.cbResolveIssue.UseVisualStyleBackColor = true;
            // 
            // btnIssueMode
            // 
            this.btnIssueMode.Location = new System.Drawing.Point(6, 201);
            this.btnIssueMode.Name = "btnIssueMode";
            this.btnIssueMode.Size = new System.Drawing.Size(79, 23);
            this.btnIssueMode.TabIndex = 47;
            this.btnIssueMode.TabStop = false;
            this.btnIssueMode.Text = "Issues";
            this.btnIssueMode.UseVisualStyleBackColor = true;
            this.btnIssueMode.Click += new System.EventHandler(this.btnBugs_Click);
            // 
            // btnSubmitAll
            // 
            this.btnSubmitAll.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmitAll.Location = new System.Drawing.Point(218, 231);
            this.btnSubmitAll.Name = "btnSubmitAll";
            this.btnSubmitAll.Size = new System.Drawing.Size(32, 32);
            this.btnSubmitAll.TabIndex = 45;
            this.btnSubmitAll.Text = "All";
            this.btnSubmitAll.UseVisualStyleBackColor = true;
            // 
            // btnComments
            // 
            this.btnComments.Location = new System.Drawing.Point(6, 72);
            this.btnComments.Name = "btnComments";
            this.btnComments.Size = new System.Drawing.Size(75, 25);
            this.btnComments.TabIndex = 44;
            this.btnComments.Text = "Comments";
            this.btnComments.UseVisualStyleBackColor = true;
            // 
            // tbIssue
            // 
            this.tbIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tbIssue.Location = new System.Drawing.Point(6, 231);
            this.tbIssue.Name = "tbIssue";
            this.tbIssue.Size = new System.Drawing.Size(79, 32);
            this.tbIssue.TabIndex = 43;
            this.tbIssue.WordWrap = false;
            // 
            // pComments
            // 
            this.pComments.Controls.Add(this.tbComment);
            this.pComments.Controls.Add(this.panel1);
            this.pComments.Location = new System.Drawing.Point(6, 103);
            this.pComments.Name = "pComments";
            this.pComments.Size = new System.Drawing.Size(244, 92);
            this.pComments.TabIndex = 40;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemoveComment);
            this.panel1.Controls.Add(this.btnNewComment);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(219, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(25, 92);
            this.panel1.TabIndex = 22;
            // 
            // btnRemoveComment
            // 
            this.btnRemoveComment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRemoveComment.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRemoveComment.ForeColor = System.Drawing.Color.Red;
            this.btnRemoveComment.Location = new System.Drawing.Point(0, 57);
            this.btnRemoveComment.Name = "btnRemoveComment";
            this.btnRemoveComment.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnRemoveComment.Size = new System.Drawing.Size(25, 35);
            this.btnRemoveComment.TabIndex = 1;
            this.btnRemoveComment.Text = "X";
            this.btnRemoveComment.UseVisualStyleBackColor = true;
            // 
            // btnNewComment
            // 
            this.btnNewComment.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNewComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnNewComment.ForeColor = System.Drawing.Color.Green;
            this.btnNewComment.Location = new System.Drawing.Point(0, 0);
            this.btnNewComment.Name = "btnNewComment";
            this.btnNewComment.Padding = new System.Windows.Forms.Padding(2, 3, 0, 0);
            this.btnNewComment.Size = new System.Drawing.Size(25, 40);
            this.btnNewComment.TabIndex = 0;
            this.btnNewComment.Text = "+";
            this.btnNewComment.UseVisualStyleBackColor = true;
            // 
            // btnResetIdle
            // 
            this.btnResetIdle.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetIdle.Location = new System.Drawing.Point(180, 35);
            this.btnResetIdle.Name = "btnResetIdle";
            this.btnResetIdle.Size = new System.Drawing.Size(70, 31);
            this.btnResetIdle.TabIndex = 39;
            this.btnResetIdle.Text = "Reset";
            this.btnResetIdle.UseVisualStyleBackColor = true;
            // 
            // cmComments
            // 
            this.cmComments.Name = "cmComments";
            this.cmComments.Size = new System.Drawing.Size(61, 4);
            // 
            // lHide
            // 
            this.lHide.AutoSize = true;
            this.lHide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lHide.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHide.ForeColor = System.Drawing.Color.Blue;
            this.lHide.Location = new System.Drawing.Point(479, 7);
            this.lHide.Name = "lHide";
            this.lHide.Size = new System.Drawing.Size(37, 17);
            this.lHide.TabIndex = 40;
            this.lHide.Text = "Hide";
            // 
            // cmIssuesKind
            // 
            this.cmIssuesKind.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmMyBugs,
            this.tsmMyIssues,
            this.tsmMyWork});
            this.cmIssuesKind.Name = "cmIssuesKind";
            this.cmIssuesKind.Size = new System.Drawing.Size(126, 70);
            // 
            // tsmMyBugs
            // 
            this.tsmMyBugs.Name = "tsmMyBugs";
            this.tsmMyBugs.Size = new System.Drawing.Size(125, 22);
            this.tsmMyBugs.Text = "My bugs";
            // 
            // tsmMyIssues
            // 
            this.tsmMyIssues.Name = "tsmMyIssues";
            this.tsmMyIssues.Size = new System.Drawing.Size(125, 22);
            this.tsmMyIssues.Text = "My issues";
            // 
            // tsmMyWork
            // 
            this.tsmMyWork.Name = "tsmMyWork";
            this.tsmMyWork.Size = new System.Drawing.Size(125, 22);
            this.tsmMyWork.Text = "My work";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.86141F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.86141F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.86141F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.86141F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.86141F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.86141F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.86141F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.985045F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.985045F));
            this.tableLayoutPanel1.Controls.Add(this.llDay2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.llDay1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.llDay3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.llDay4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.llDay5, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.llDay6, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.llDay7, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnWorkReportSync, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnWorkReportMode, 8, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 515);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 23);
            this.tableLayoutPanel1.TabIndex = 43;
            // 
            // llDay2
            // 
            this.llDay2.AutoSize = true;
            this.llDay2.BackColor = System.Drawing.Color.Transparent;
            this.llDay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llDay2.Font = new System.Drawing.Font("Segoe UI Black", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.llDay2.LinkColor = System.Drawing.Color.Black;
            this.llDay2.Location = new System.Drawing.Point(78, 0);
            this.llDay2.Name = "llDay2";
            this.llDay2.Size = new System.Drawing.Size(69, 23);
            this.llDay2.TabIndex = 0;
            this.llDay2.TabStop = true;
            this.llDay2.Text = "00:00";
            this.llDay2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llDay1
            // 
            this.llDay1.AutoSize = true;
            this.llDay1.BackColor = System.Drawing.Color.Transparent;
            this.llDay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llDay1.Font = new System.Drawing.Font("Segoe UI Black", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.llDay1.LinkColor = System.Drawing.Color.Black;
            this.llDay1.Location = new System.Drawing.Point(3, 0);
            this.llDay1.Name = "llDay1";
            this.llDay1.Size = new System.Drawing.Size(69, 23);
            this.llDay1.TabIndex = 7;
            this.llDay1.TabStop = true;
            this.llDay1.Text = "00:00";
            this.llDay1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llDay3
            // 
            this.llDay3.AutoSize = true;
            this.llDay3.BackColor = System.Drawing.Color.Transparent;
            this.llDay3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llDay3.Font = new System.Drawing.Font("Segoe UI Black", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.llDay3.LinkColor = System.Drawing.Color.Black;
            this.llDay3.Location = new System.Drawing.Point(153, 0);
            this.llDay3.Name = "llDay3";
            this.llDay3.Size = new System.Drawing.Size(69, 23);
            this.llDay3.TabIndex = 1;
            this.llDay3.TabStop = true;
            this.llDay3.Text = "00:00";
            this.llDay3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llDay4
            // 
            this.llDay4.AutoSize = true;
            this.llDay4.BackColor = System.Drawing.Color.Transparent;
            this.llDay4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llDay4.Font = new System.Drawing.Font("Segoe UI Black", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.llDay4.LinkColor = System.Drawing.Color.Black;
            this.llDay4.Location = new System.Drawing.Point(228, 0);
            this.llDay4.Name = "llDay4";
            this.llDay4.Size = new System.Drawing.Size(69, 23);
            this.llDay4.TabIndex = 2;
            this.llDay4.TabStop = true;
            this.llDay4.Text = "00:00";
            this.llDay4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llDay5
            // 
            this.llDay5.AutoSize = true;
            this.llDay5.BackColor = System.Drawing.Color.Transparent;
            this.llDay5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llDay5.Font = new System.Drawing.Font("Segoe UI Black", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.llDay5.LinkColor = System.Drawing.Color.Black;
            this.llDay5.Location = new System.Drawing.Point(303, 0);
            this.llDay5.Name = "llDay5";
            this.llDay5.Size = new System.Drawing.Size(69, 23);
            this.llDay5.TabIndex = 3;
            this.llDay5.TabStop = true;
            this.llDay5.Text = "00:00";
            this.llDay5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llDay6
            // 
            this.llDay6.AutoSize = true;
            this.llDay6.BackColor = System.Drawing.Color.Transparent;
            this.llDay6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llDay6.Font = new System.Drawing.Font("Segoe UI Black", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.llDay6.Location = new System.Drawing.Point(378, 0);
            this.llDay6.Name = "llDay6";
            this.llDay6.Size = new System.Drawing.Size(69, 23);
            this.llDay6.TabIndex = 4;
            this.llDay6.TabStop = true;
            this.llDay6.Text = "00:00";
            this.llDay6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llDay7
            // 
            this.llDay7.AutoSize = true;
            this.llDay7.BackColor = System.Drawing.Color.Transparent;
            this.llDay7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llDay7.Font = new System.Drawing.Font("Segoe UI Black", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.llDay7.Location = new System.Drawing.Point(453, 0);
            this.llDay7.Name = "llDay7";
            this.llDay7.Size = new System.Drawing.Size(69, 23);
            this.llDay7.TabIndex = 5;
            this.llDay7.TabStop = true;
            this.llDay7.Text = "00:00";
            this.llDay7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnWorkReportSync
            // 
            this.btnWorkReportSync.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWorkReportSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWorkReportSync.Image = global::RedmineLog.Properties.Resources.Refresh;
            this.btnWorkReportSync.Location = new System.Drawing.Point(528, 3);
            this.btnWorkReportSync.Name = "btnWorkReportSync";
            this.btnWorkReportSync.Size = new System.Drawing.Size(23, 17);
            this.btnWorkReportSync.TabIndex = 8;
            this.btnWorkReportSync.Text = "button1";
            this.btnWorkReportSync.UseVisualStyleBackColor = true;
            // 
            // btnWorkReportMode
            // 
            this.btnWorkReportMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWorkReportMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWorkReportMode.Image = global::RedmineLog.Properties.Resources.Calendar;
            this.btnWorkReportMode.Location = new System.Drawing.Point(557, 3);
            this.btnWorkReportMode.Name = "btnWorkReportMode";
            this.btnWorkReportMode.Size = new System.Drawing.Size(24, 17);
            this.btnWorkReportMode.TabIndex = 9;
            this.btnWorkReportMode.Text = "button2";
            this.btnWorkReportMode.UseVisualStyleBackColor = true;
            // 
            // cHeader
            // 
            this.cHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.cHeader.Data = null;
            this.cHeader.Location = new System.Drawing.Point(3, 4);
            this.cHeader.Name = "cHeader";
            this.cHeader.Size = new System.Drawing.Size(310, 61);
            this.cHeader.TabIndex = 42;
            // 
            // fpIssueList
            // 
            this.fpIssueList.AutoScroll = true;
            this.fpIssueList.Location = new System.Drawing.Point(1, 71);
            this.fpIssueList.Name = "fpIssueList";
            this.fpIssueList.Size = new System.Drawing.Size(320, 438);
            this.fpIssueList.TabIndex = 41;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 538);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cHeader);
            this.Controls.Add(this.fpIssueList);
            this.Controls.Add(this.lHide);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lnkExit);
            this.Controls.Add(this.lnkSettings);
            this.Controls.Add(this.pManage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "RedmineLog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnMainLoad);
            this.issueInfoPanel.ResumeLayout(false);
            this.issueInfoPanel.PerformLayout();
            this.pManage.ResumeLayout(false);
            this.pManage.PerformLayout();
            this.pComments.ResumeLayout(false);
            this.pComments.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.cmIssuesKind.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lnkExit;
        internal System.Windows.Forms.Label lnkSettings;
        internal System.Windows.Forms.Label lnkIssues;
        internal System.Windows.Forms.Button btnSubmit;
        internal System.Windows.Forms.TextBox tbComment;
        internal System.Windows.Forms.Label lblClockActive;
        internal System.Windows.Forms.ComboBox cbActivity;
        private System.Windows.Forms.FlowLayoutPanel issueInfoPanel;
        private System.Windows.Forms.Label lblVersion;
        internal System.Windows.Forms.Button btnResetIdle;
        private System.Windows.Forms.Panel pComments;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lHide;
        internal System.Windows.Forms.Button btnSubmitAll;
        internal System.Windows.Forms.Label lblParentIssue;
        internal System.Windows.Forms.LinkLabel lblIssue;
        internal System.Windows.Forms.Button btnRemoveItem;
        internal System.Windows.Forms.Label lblProject;
        internal System.Windows.Forms.TextBox tbIssue;
        internal System.Windows.Forms.Button btnComments;
        internal System.Windows.Forms.ContextMenuStrip cmComments;
        internal System.Windows.Forms.Button btnRemoveComment;
        internal System.Windows.Forms.Button btnNewComment;
        internal System.Windows.Forms.Label lblClockIdle;
        internal System.Windows.Forms.Panel pManage;
        protected internal System.Windows.Forms.Label lblTracker;
        private System.Windows.Forms.ContextMenuStrip cmIssuesKind;
        internal System.Windows.Forms.ToolStripMenuItem tsmMyBugs;
        internal System.Windows.Forms.ToolStripMenuItem tsmMyIssues;
        internal System.Windows.Forms.ToolStripMenuItem tsmMyWork;
        private System.Windows.Forms.Button btnIssueMode;
        internal System.Windows.Forms.CheckBox cbResolveIssue;
        internal System.Windows.Forms.Label lbClockTodayTime;
        internal System.Windows.Forms.ToolTip ttStartTime;
        internal UI.Common.ExFlowLayoutPanel fpIssueList;
        internal UI.Items.IssueItemView cHeader;
        internal System.Windows.Forms.Button btnStopWork;
        internal System.Windows.Forms.LinkLabel llDay1;
        internal System.Windows.Forms.LinkLabel llDay2;
        internal System.Windows.Forms.LinkLabel llDay3;
        internal System.Windows.Forms.LinkLabel llDay4;
        internal System.Windows.Forms.LinkLabel llDay5;
        internal System.Windows.Forms.LinkLabel llDay6;
        internal System.Windows.Forms.LinkLabel llDay7;
        internal System.Windows.Forms.Button btnWorkReportSync;
        internal System.Windows.Forms.Button btnWorkReportMode;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}