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
            this.btnSend = new System.Windows.Forms.Button();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStop = new System.Windows.Forms.Button();
            this.btnClock = new System.Windows.Forms.Button();
            this.lblClockActive = new System.Windows.Forms.Label();
            this.cbActivity = new System.Windows.Forms.ComboBox();
            this.lblParentIssue = new System.Windows.Forms.Label();
            this.lblIssue = new System.Windows.Forms.LinkLabel();
            this.issueInfoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblClockIndle = new System.Windows.Forms.Label();
            this.pManage = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.tbIssue = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.pComments = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemoveComment = new System.Windows.Forms.Button();
            this.btnNewComment = new System.Windows.Forms.Button();
            this.btnResetIdle = new System.Windows.Forms.Button();
            this.cmComments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lHide = new System.Windows.Forms.Label();
            this.issueInfoPanel.SuspendLayout();
            this.pManage.SuspendLayout();
            this.pComments.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkExit
            // 
            this.lnkExit.AutoSize = true;
            this.lnkExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkExit.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExit.ForeColor = System.Drawing.Color.Blue;
            this.lnkExit.Location = new System.Drawing.Point(207, 7);
            this.lnkExit.Name = "lnkExit";
            this.lnkExit.Size = new System.Drawing.Size(29, 17);
            this.lnkExit.TabIndex = 31;
            this.lnkExit.Text = "Exit";
            this.lnkExit.Click += new System.EventHandler(this.OnExitClick);
            // 
            // lnkSettings
            // 
            this.lnkSettings.AutoSize = true;
            this.lnkSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkSettings.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSettings.ForeColor = System.Drawing.Color.Blue;
            this.lnkSettings.Location = new System.Drawing.Point(70, 7);
            this.lnkSettings.Name = "lnkSettings";
            this.lnkSettings.Size = new System.Drawing.Size(57, 17);
            this.lnkSettings.TabIndex = 27;
            this.lnkSettings.Text = "Settings";
            this.lnkSettings.Click += new System.EventHandler(this.OnSettingsClick);
            // 
            // lnkIssues
            // 
            this.lnkIssues.AutoSize = true;
            this.lnkIssues.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkIssues.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkIssues.ForeColor = System.Drawing.Color.Blue;
            this.lnkIssues.Location = new System.Drawing.Point(126, 264);
            this.lnkIssues.Name = "lnkIssues";
            this.lnkIssues.Size = new System.Drawing.Size(103, 17);
            this.lnkIssues.TabIndex = 24;
            this.lnkIssues.Text = "Redmine Issues";
            this.lnkIssues.Click += new System.EventHandler(this.OnRedmineIssuesClick);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(135, 289);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(96, 32);
            this.btnSend.TabIndex = 22;
            this.btnSend.Text = "Submit";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.OnSubmitClick);
            // 
            // tbComment
            // 
            this.tbComment.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbComment.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbComment.Location = new System.Drawing.Point(0, 0);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(177, 76);
            this.tbComment.TabIndex = 21;
            this.tbComment.Click += new System.EventHandler(this.OnCommentClick);
            this.tbComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnCommentKeyDown);
            this.tbComment.Leave += new System.EventHandler(this.OnCommentLostFocus);
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 1000;
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(142, 82);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(87, 33);
            this.btnStop.TabIndex = 20;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.OnStopClick);
            // 
            // btnClock
            // 
            this.btnClock.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClock.Location = new System.Drawing.Point(26, 82);
            this.btnClock.Name = "btnClock";
            this.btnClock.Size = new System.Drawing.Size(95, 33);
            this.btnClock.TabIndex = 19;
            this.btnClock.Text = "Play";
            this.btnClock.UseVisualStyleBackColor = true;
            this.btnClock.Click += new System.EventHandler(this.OnClockClick);
            // 
            // lblClockActive
            // 
            this.lblClockActive.BackColor = System.Drawing.Color.Transparent;
            this.lblClockActive.Font = new System.Drawing.Font("Segoe WP", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClockActive.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblClockActive.Location = new System.Drawing.Point(20, 5);
            this.lblClockActive.Name = "lblClockActive";
            this.lblClockActive.Size = new System.Drawing.Size(166, 45);
            this.lblClockActive.TabIndex = 18;
            this.lblClockActive.Text = "00:00:00";
            this.lblClockActive.Click += new System.EventHandler(this.OnClockActiveClick);
            // 
            // cbActivity
            // 
            this.cbActivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbActivity.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbActivity.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbActivity.FormattingEnabled = true;
            this.cbActivity.Location = new System.Drawing.Point(28, 121);
            this.cbActivity.Name = "cbActivity";
            this.cbActivity.Size = new System.Drawing.Size(203, 28);
            this.cbActivity.TabIndex = 17;
            // 
            // lblParentIssue
            // 
            this.lblParentIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblParentIssue.AutoSize = true;
            this.lblParentIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblParentIssue.Location = new System.Drawing.Point(0, 5);
            this.lblParentIssue.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lblParentIssue.Name = "lblParentIssue";
            this.lblParentIssue.Size = new System.Drawing.Size(15, 17);
            this.lblParentIssue.TabIndex = 33;
            // 
            // lblIssue
            // 
            this.lblIssue.AutoSize = true;
            this.lblIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblIssue.Location = new System.Drawing.Point(15, 27);
            this.lblIssue.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblIssue.Name = "lblIssue";
            this.lblIssue.Size = new System.Drawing.Size(0, 18);
            this.lblIssue.TabIndex = 34;
            this.lblIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnIssueLinkClick);
            // 
            // issueInfoPanel
            // 
            this.issueInfoPanel.Controls.Add(this.lblParentIssue);
            this.issueInfoPanel.Controls.Add(this.lblIssue);
            this.issueInfoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.issueInfoPanel.Location = new System.Drawing.Point(28, 330);
            this.issueInfoPanel.Name = "issueInfoPanel";
            this.issueInfoPanel.Size = new System.Drawing.Size(200, 100);
            this.issueInfoPanel.TabIndex = 35;
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveItem.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRemoveItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRemoveItem.ForeColor = System.Drawing.Color.Red;
            this.btnRemoveItem.Location = new System.Drawing.Point(105, 289);
            this.btnRemoveItem.Margin = new System.Windows.Forms.Padding(125, 0, 0, 0);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(27, 32);
            this.btnRemoveItem.TabIndex = 35;
            this.btnRemoveItem.Text = "X";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            this.btnRemoveItem.Click += new System.EventHandler(this.OnRemoveItem);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe WP", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblVersion.Location = new System.Drawing.Point(7, 7);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(0, 19);
            this.lblVersion.TabIndex = 37;
            // 
            // lblClockIndle
            // 
            this.lblClockIndle.AutoSize = true;
            this.lblClockIndle.BackColor = System.Drawing.Color.Transparent;
            this.lblClockIndle.Font = new System.Drawing.Font("Segoe WP", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblClockIndle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblClockIndle.Location = new System.Drawing.Point(164, 50);
            this.lblClockIndle.Name = "lblClockIndle";
            this.lblClockIndle.Size = new System.Drawing.Size(65, 19);
            this.lblClockIndle.TabIndex = 38;
            this.lblClockIndle.Text = "00:00:00";
            this.lblClockIndle.Click += new System.EventHandler(this.OnClockIdleClick);
            // 
            // pManage
            // 
            this.pManage.BackColor = System.Drawing.Color.Transparent;
            this.pManage.Controls.Add(this.button3);
            this.pManage.Controls.Add(this.tbIssue);
            this.pManage.Controls.Add(this.button2);
            this.pManage.Controls.Add(this.pComments);
            this.pManage.Controls.Add(this.btnResetIdle);
            this.pManage.Controls.Add(this.lblClockIndle);
            this.pManage.Controls.Add(this.btnRemoveItem);
            this.pManage.Controls.Add(this.btnClock);
            this.pManage.Controls.Add(this.lblClockActive);
            this.pManage.Controls.Add(this.cbActivity);
            this.pManage.Controls.Add(this.issueInfoPanel);
            this.pManage.Controls.Add(this.btnStop);
            this.pManage.Controls.Add(this.btnSend);
            this.pManage.Controls.Add(this.lnkIssues);
            this.pManage.Location = new System.Drawing.Point(-5, 36);
            this.pManage.Name = "pManage";
            this.pManage.Size = new System.Drawing.Size(258, 455);
            this.pManage.TabIndex = 39;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(28, 153);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 22);
            this.button3.TabIndex = 44;
            this.button3.Text = "Comments";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnCommentShowClick);
            // 
            // tbIssue
            // 
            this.tbIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tbIssue.Location = new System.Drawing.Point(28, 288);
            this.tbIssue.Name = "tbIssue";
            this.tbIssue.Size = new System.Drawing.Size(72, 32);
            this.tbIssue.TabIndex = 43;
            this.tbIssue.WordWrap = false;
            this.tbIssue.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnIssueMouseClick);
            this.tbIssue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnIssueKeyDown);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(28, 261);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 42;
            this.button2.Text = "Issues";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnSearchIssueClick);
            // 
            // pComments
            // 
            this.pComments.Controls.Add(this.panel1);
            this.pComments.Controls.Add(this.tbComment);
            this.pComments.Location = new System.Drawing.Point(28, 179);
            this.pComments.Name = "pComments";
            this.pComments.Size = new System.Drawing.Size(202, 76);
            this.pComments.TabIndex = 40;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemoveComment);
            this.panel1.Controls.Add(this.btnNewComment);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(177, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(25, 76);
            this.panel1.TabIndex = 22;
            // 
            // btnRemoveComment
            // 
            this.btnRemoveComment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRemoveComment.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRemoveComment.ForeColor = System.Drawing.Color.Red;
            this.btnRemoveComment.Location = new System.Drawing.Point(0, 41);
            this.btnRemoveComment.Name = "btnRemoveComment";
            this.btnRemoveComment.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnRemoveComment.Size = new System.Drawing.Size(25, 35);
            this.btnRemoveComment.TabIndex = 1;
            this.btnRemoveComment.Text = "X";
            this.btnRemoveComment.UseVisualStyleBackColor = true;
            this.btnRemoveComment.Click += new System.EventHandler(this.OnRemoveCommentClick);
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
            this.btnNewComment.Click += new System.EventHandler(this.OnNewCommentClick);
            // 
            // btnResetIdle
            // 
            this.btnResetIdle.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetIdle.Location = new System.Drawing.Point(142, 82);
            this.btnResetIdle.Name = "btnResetIdle";
            this.btnResetIdle.Size = new System.Drawing.Size(87, 33);
            this.btnResetIdle.TabIndex = 39;
            this.btnResetIdle.Text = "Reset";
            this.btnResetIdle.UseVisualStyleBackColor = true;
            this.btnResetIdle.Click += new System.EventHandler(this.OnResetIdleClick);
            // 
            // cmComments
            // 
            this.cmComments.Name = "cmComments";
            this.cmComments.Size = new System.Drawing.Size(61, 4);
            this.cmComments.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnSelectComment);
            // 
            // lHide
            // 
            this.lHide.AutoSize = true;
            this.lHide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lHide.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHide.ForeColor = System.Drawing.Color.Blue;
            this.lHide.Location = new System.Drawing.Point(152, 7);
            this.lHide.Name = "lHide";
            this.lHide.Size = new System.Drawing.Size(37, 17);
            this.lHide.TabIndex = 40;
            this.lHide.Text = "Hide";
            this.lHide.MouseEnter += new System.EventHandler(this.OnHideMouseEnter);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 469);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Resize += new System.EventHandler(this.OnFormResize);
            this.issueInfoPanel.ResumeLayout(false);
            this.issueInfoPanel.PerformLayout();
            this.pManage.ResumeLayout(false);
            this.pManage.PerformLayout();
            this.pComments.ResumeLayout(false);
            this.pComments.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lnkExit;
        internal System.Windows.Forms.Label lnkSettings;
        internal System.Windows.Forms.Label lnkIssues;
        internal System.Windows.Forms.Button btnSend;
        internal System.Windows.Forms.TextBox tbComment;
        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.Button btnStop;
        internal System.Windows.Forms.Button btnClock;
        internal System.Windows.Forms.Label lblClockActive;
        internal System.Windows.Forms.ComboBox cbActivity;
        private System.Windows.Forms.Label lblParentIssue;
        private System.Windows.Forms.LinkLabel lblIssue;
        private System.Windows.Forms.FlowLayoutPanel issueInfoPanel;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblClockIndle;
        private System.Windows.Forms.Panel pManage;
        internal System.Windows.Forms.Button btnResetIdle;
        private System.Windows.Forms.Panel pComments;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRemoveComment;
        private System.Windows.Forms.ContextMenuStrip cmComments;
        private System.Windows.Forms.Button btnNewComment;
        internal System.Windows.Forms.Label lHide;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbIssue;
        private System.Windows.Forms.Button button3;
    }
}