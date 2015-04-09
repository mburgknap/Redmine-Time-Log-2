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
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.HideTooltipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ntIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lnkExit = new System.Windows.Forms.Label();
            this.rdIssue = new System.Windows.Forms.RadioButton();
            this.lnkSettings = new System.Windows.Forms.Label();
            this.lnkIssues = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.Label2 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnClock = new System.Windows.Forms.Button();
            this.lblClockActive = new System.Windows.Forms.Label();
            this.cbActivity = new System.Windows.Forms.ComboBox();
            this.lblParentIssue = new System.Windows.Forms.Label();
            this.lblIssue = new System.Windows.Forms.LinkLabel();
            this.issueInfoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.cbIssues = new System.Windows.Forms.ComboBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblClockIndle = new System.Windows.Forms.Label();
            this.pManage = new System.Windows.Forms.Panel();
            this.btnResetIdle = new System.Windows.Forms.Button();
            this.ContextMenuStrip1.SuspendLayout();
            this.issueInfoPanel.SuspendLayout();
            this.pManage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(137, 6);
            // 
            // HideTooltipToolStripMenuItem
            // 
            this.HideTooltipToolStripMenuItem.Name = "HideTooltipToolStripMenuItem";
            this.HideTooltipToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.HideTooltipToolStripMenuItem.Text = "Hide Tooltip";
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HideTooltipToolStripMenuItem,
            this.ToolStripSeparator1,
            this.ExitToolStripMenuItem});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(141, 54);
            // 
            // ntIcon
            // 
            this.ntIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ntIcon.ContextMenuStrip = this.ContextMenuStrip1;
            this.ntIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ntIcon.Icon")));
            this.ntIcon.Text = "Time";
            this.ntIcon.Visible = true;
            this.ntIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnIconClick);
            // 
            // lnkExit
            // 
            this.lnkExit.AutoSize = true;
            this.lnkExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkExit.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExit.ForeColor = System.Drawing.Color.Blue;
            this.lnkExit.Location = new System.Drawing.Point(197, 14);
            this.lnkExit.Name = "lnkExit";
            this.lnkExit.Size = new System.Drawing.Size(29, 17);
            this.lnkExit.TabIndex = 31;
            this.lnkExit.Text = "Exit";
            this.lnkExit.Click += new System.EventHandler(this.OnExitClick);
            // 
            // rdIssue
            // 
            this.rdIssue.AutoSize = true;
            this.rdIssue.Checked = true;
            this.rdIssue.Enabled = false;
            this.rdIssue.Location = new System.Drawing.Point(30, 251);
            this.rdIssue.Name = "rdIssue";
            this.rdIssue.Size = new System.Drawing.Size(50, 17);
            this.rdIssue.TabIndex = 30;
            this.rdIssue.TabStop = true;
            this.rdIssue.Text = "Issue";
            this.rdIssue.UseVisualStyleBackColor = true;
            // 
            // lnkSettings
            // 
            this.lnkSettings.AutoSize = true;
            this.lnkSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkSettings.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSettings.ForeColor = System.Drawing.Color.Blue;
            this.lnkSettings.Location = new System.Drawing.Point(124, 14);
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
            this.lnkIssues.Location = new System.Drawing.Point(128, 251);
            this.lnkIssues.Name = "lnkIssues";
            this.lnkIssues.Size = new System.Drawing.Size(103, 17);
            this.lnkIssues.TabIndex = 24;
            this.lnkIssues.Text = "Redmine Issues";
            this.lnkIssues.Click += new System.EventHandler(this.OnRedmineIssuesClick);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(160, 274);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(71, 35);
            this.btnSend.TabIndex = 22;
            this.btnSend.Text = "Submit";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.OnSubmitClick);
            // 
            // txtComment
            // 
            this.txtComment.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComment.Location = new System.Drawing.Point(28, 172);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(203, 69);
            this.txtComment.TabIndex = 21;
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 1000;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(25, 152);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(75, 17);
            this.Label2.TabIndex = 25;
            this.Label2.Text = "Comment";
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
            this.cbActivity.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbActivity.FormattingEnabled = true;
            this.cbActivity.Location = new System.Drawing.Point(28, 121);
            this.cbActivity.Name = "cbActivity";
            this.cbActivity.Size = new System.Drawing.Size(203, 28);
            this.cbActivity.TabIndex = 17;
            this.cbActivity.Text = "Select Activity";
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
            this.issueInfoPanel.Location = new System.Drawing.Point(30, 315);
            this.issueInfoPanel.Name = "issueInfoPanel";
            this.issueInfoPanel.Size = new System.Drawing.Size(200, 100);
            this.issueInfoPanel.TabIndex = 35;
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveItem.BackColor = System.Drawing.Color.White;
            this.btnRemoveItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRemoveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRemoveItem.Location = new System.Drawing.Point(114, 275);
            this.btnRemoveItem.Margin = new System.Windows.Forms.Padding(125, 0, 0, 0);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(18, 32);
            this.btnRemoveItem.TabIndex = 35;
            this.btnRemoveItem.Text = "X";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            this.btnRemoveItem.Click += new System.EventHandler(this.OnRemoveItem);
            // 
            // cbIssues
            // 
            this.cbIssues.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cbIssues.FormattingEnabled = true;
            this.cbIssues.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbIssues.ItemHeight = 24;
            this.cbIssues.Location = new System.Drawing.Point(30, 275);
            this.cbIssues.Name = "cbIssues";
            this.cbIssues.Size = new System.Drawing.Size(121, 32);
            this.cbIssues.TabIndex = 36;
            this.cbIssues.SelectedValueChanged += new System.EventHandler(this.OnIssueIDChanged);
            this.cbIssues.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnAcceptIssue);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe WP", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblVersion.Location = new System.Drawing.Point(23, 14);
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
            this.pManage.Controls.Add(this.btnResetIdle);
            this.pManage.Controls.Add(this.lblClockIndle);
            this.pManage.Controls.Add(this.btnRemoveItem);
            this.pManage.Controls.Add(this.btnClock);
            this.pManage.Controls.Add(this.cbIssues);
            this.pManage.Controls.Add(this.lblClockActive);
            this.pManage.Controls.Add(this.cbActivity);
            this.pManage.Controls.Add(this.issueInfoPanel);
            this.pManage.Controls.Add(this.btnStop);
            this.pManage.Controls.Add(this.Label2);
            this.pManage.Controls.Add(this.rdIssue);
            this.pManage.Controls.Add(this.txtComment);
            this.pManage.Controls.Add(this.btnSend);
            this.pManage.Controls.Add(this.lnkIssues);
            this.pManage.Location = new System.Drawing.Point(-5, 36);
            this.pManage.Name = "pManage";
            this.pManage.Size = new System.Drawing.Size(258, 455);
            this.pManage.TabIndex = 39;
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
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 469);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lnkExit);
            this.Controls.Add(this.lnkSettings);
            this.Controls.Add(this.pManage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RedmineLog";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ContextMenuStrip1.ResumeLayout(false);
            this.issueInfoPanel.ResumeLayout(false);
            this.issueInfoPanel.PerformLayout();
            this.pManage.ResumeLayout(false);
            this.pManage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem HideTooltipToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.NotifyIcon ntIcon;
        internal System.Windows.Forms.Label lnkExit;
        internal System.Windows.Forms.RadioButton rdIssue;
        internal System.Windows.Forms.Label lnkSettings;
        internal System.Windows.Forms.Label lnkIssues;
        internal System.Windows.Forms.Button btnSend;
        internal System.Windows.Forms.TextBox txtComment;
        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button btnStop;
        internal System.Windows.Forms.Button btnClock;
        internal System.Windows.Forms.Label lblClockActive;
        internal System.Windows.Forms.ComboBox cbActivity;
        private System.Windows.Forms.Label lblParentIssue;
        private System.Windows.Forms.LinkLabel lblIssue;
        private System.Windows.Forms.FlowLayoutPanel issueInfoPanel;
        private System.Windows.Forms.ComboBox cbIssues;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblClockIndle;
        private System.Windows.Forms.Panel pManage;
        internal System.Windows.Forms.Button btnResetIdle;
    }
}