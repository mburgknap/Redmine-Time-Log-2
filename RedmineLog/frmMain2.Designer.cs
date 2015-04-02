namespace RedmineLog
{
    partial class frmMain2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain2));
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.HideTooltipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ntIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnExit = new System.Windows.Forms.Label();
            this.rdIssue = new System.Windows.Forms.RadioButton();
            this.rdProject = new System.Windows.Forms.RadioButton();
            this.lnkToggle = new System.Windows.Forms.Label();
            this.lnkSettings = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtProjectID = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.Label2 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnClock = new System.Windows.Forms.Button();
            this.lblClock = new System.Windows.Forms.Label();
            this.cmbActivity = new System.Windows.Forms.ComboBox();
            this.ContextMenuStrip1.SuspendLayout();
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
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.Blue;
            this.btnExit.Location = new System.Drawing.Point(204, 41);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(29, 17);
            this.btnExit.TabIndex = 31;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // rdIssue
            // 
            this.rdIssue.AutoSize = true;
            this.rdIssue.Location = new System.Drawing.Point(114, 328);
            this.rdIssue.Name = "rdIssue";
            this.rdIssue.Size = new System.Drawing.Size(50, 17);
            this.rdIssue.TabIndex = 30;
            this.rdIssue.Text = "Issue";
            this.rdIssue.UseVisualStyleBackColor = true;
            // 
            // rdProject
            // 
            this.rdProject.AutoSize = true;
            this.rdProject.Checked = true;
            this.rdProject.Location = new System.Drawing.Point(45, 328);
            this.rdProject.Name = "rdProject";
            this.rdProject.Size = new System.Drawing.Size(58, 17);
            this.rdProject.TabIndex = 29;
            this.rdProject.TabStop = true;
            this.rdProject.Text = "Project";
            this.rdProject.UseVisualStyleBackColor = true;
            // 
            // lnkToggle
            // 
            this.lnkToggle.AutoSize = true;
            this.lnkToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkToggle.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkToggle.ForeColor = System.Drawing.Color.Blue;
            this.lnkToggle.Location = new System.Drawing.Point(31, 41);
            this.lnkToggle.Name = "lnkToggle";
            this.lnkToggle.Size = new System.Drawing.Size(65, 17);
            this.lnkToggle.TabIndex = 28;
            this.lnkToggle.Text = "Collapse";
            this.lnkToggle.Click += new System.EventHandler(this.lnkToggle_Click);
            // 
            // lnkSettings
            // 
            this.lnkSettings.AutoSize = true;
            this.lnkSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkSettings.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSettings.ForeColor = System.Drawing.Color.Blue;
            this.lnkSettings.Location = new System.Drawing.Point(120, 41);
            this.lnkSettings.Name = "lnkSettings";
            this.lnkSettings.Size = new System.Drawing.Size(57, 17);
            this.lnkSettings.TabIndex = 27;
            this.lnkSettings.Text = "Settings";
            this.lnkSettings.Click += new System.EventHandler(this.Label4_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(42, 148);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(55, 17);
            this.Label3.TabIndex = 26;
            this.Label3.Text = "Activity";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.Blue;
            this.Label1.Location = new System.Drawing.Point(163, 328);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(70, 17);
            this.Label1.TabIndex = 24;
            this.Label1.Text = "Project ID";
            this.Label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // txtProjectID
            // 
            this.txtProjectID.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectID.Location = new System.Drawing.Point(45, 351);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.Size = new System.Drawing.Size(95, 31);
            this.txtProjectID.TabIndex = 23;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(177, 351);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(71, 31);
            this.btnSend.TabIndex = 22;
            this.btnSend.Text = "Submit";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtComment
            // 
            this.txtComment.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComment.Location = new System.Drawing.Point(45, 219);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(203, 103);
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
            this.Label2.Location = new System.Drawing.Point(42, 199);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(75, 17);
            this.Label2.TabIndex = 25;
            this.Label2.Text = "Comment";
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(161, 112);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(87, 33);
            this.btnStop.TabIndex = 20;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClock
            // 
            this.btnClock.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClock.Location = new System.Drawing.Point(45, 112);
            this.btnClock.Name = "btnClock";
            this.btnClock.Size = new System.Drawing.Size(95, 33);
            this.btnClock.TabIndex = 19;
            this.btnClock.Text = "Play";
            this.btnClock.UseVisualStyleBackColor = true;
            this.btnClock.Click += new System.EventHandler(this.btnClock_Click);
            // 
            // lblClock
            // 
            this.lblClock.AutoSize = true;
            this.lblClock.Font = new System.Drawing.Font("Century Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClock.Location = new System.Drawing.Point(35, 51);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(213, 58);
            this.lblClock.TabIndex = 18;
            this.lblClock.Text = "00:00:00";
            // 
            // cmbActivity
            // 
            this.cmbActivity.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbActivity.FormattingEnabled = true;
            this.cmbActivity.Location = new System.Drawing.Point(45, 168);
            this.cmbActivity.Name = "cmbActivity";
            this.cmbActivity.Size = new System.Drawing.Size(203, 28);
            this.cmbActivity.TabIndex = 17;
            this.cmbActivity.Text = "Select Activity";
            // 
            // frmMain2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 411);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.rdIssue);
            this.Controls.Add(this.rdProject);
            this.Controls.Add(this.lnkToggle);
            this.Controls.Add(this.lnkSettings);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtProjectID);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnClock);
            this.Controls.Add(this.lblClock);
            this.Controls.Add(this.cmbActivity);
            this.Name = "frmMain2";
            this.Text = "frmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem HideTooltipToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.NotifyIcon ntIcon;
        internal System.Windows.Forms.Label btnExit;
        internal System.Windows.Forms.RadioButton rdIssue;
        internal System.Windows.Forms.RadioButton rdProject;
        internal System.Windows.Forms.Label lnkToggle;
        internal System.Windows.Forms.Label lnkSettings;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtProjectID;
        internal System.Windows.Forms.Button btnSend;
        internal System.Windows.Forms.TextBox txtComment;
        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button btnStop;
        internal System.Windows.Forms.Button btnClock;
        internal System.Windows.Forms.Label lblClock;
        internal System.Windows.Forms.ComboBox cmbActivity;
    }
}