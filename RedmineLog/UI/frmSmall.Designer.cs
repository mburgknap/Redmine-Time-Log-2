namespace RedmineLog
{
    partial class frmSmall
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSmall));
            this.lbWorkTime = new System.Windows.Forms.Label();
            this.lbIdleTime = new System.Windows.Forms.Label();
            this.lbParentIssue = new System.Windows.Forms.Label();
            this.lbIssue = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lbComment = new System.Windows.Forms.Label();
            this.lbProject = new System.Windows.Forms.Label();
            this.lblTracker = new System.Windows.Forms.Label();
            this.btnHide = new System.Windows.Forms.Label();
            this.cbAutoHide = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbWorkTime
            // 
            this.lbWorkTime.AutoSize = true;
            this.lbWorkTime.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbWorkTime.ForeColor = System.Drawing.Color.ForestGreen;
            this.lbWorkTime.Location = new System.Drawing.Point(27, 95);
            this.lbWorkTime.Name = "lbWorkTime";
            this.lbWorkTime.Size = new System.Drawing.Size(72, 21);
            this.lbWorkTime.TabIndex = 0;
            this.lbWorkTime.Text = "00:00:00";
            this.lbWorkTime.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.lbWorkTime.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            // 
            // lbIdleTime
            // 
            this.lbIdleTime.AutoSize = true;
            this.lbIdleTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbIdleTime.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbIdleTime.Location = new System.Drawing.Point(100, 107);
            this.lbIdleTime.Name = "lbIdleTime";
            this.lbIdleTime.Size = new System.Drawing.Size(55, 15);
            this.lbIdleTime.TabIndex = 1;
            this.lbIdleTime.Text = "00:00:00";
            this.lbIdleTime.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.lbIdleTime.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            // 
            // lbParentIssue
            // 
            this.lbParentIssue.AutoSize = true;
            this.lbParentIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbParentIssue.Location = new System.Drawing.Point(3, 29);
            this.lbParentIssue.Margin = new System.Windows.Forms.Padding(0);
            this.lbParentIssue.Name = "lbParentIssue";
            this.lbParentIssue.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lbParentIssue.Size = new System.Drawing.Size(0, 16);
            this.lbParentIssue.TabIndex = 2;
            this.lbParentIssue.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            // 
            // lbIssue
            // 
            this.lbIssue.AutoSize = true;
            this.lbIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbIssue.Location = new System.Drawing.Point(18, 60);
            this.lbIssue.Margin = new System.Windows.Forms.Padding(15, 2, 0, 0);
            this.lbIssue.Name = "lbIssue";
            this.lbIssue.Size = new System.Drawing.Size(0, 13);
            this.lbIssue.TabIndex = 3;
            this.lbIssue.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lbComment);
            this.flowLayoutPanel1.Controls.Add(this.lbProject);
            this.flowLayoutPanel1.Controls.Add(this.lbParentIssue);
            this.flowLayoutPanel1.Controls.Add(this.lblTracker);
            this.flowLayoutPanel1.Controls.Add(this.lbIssue);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(25, 3);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(130, 92);
            this.flowLayoutPanel1.TabIndex = 4;
            this.flowLayoutPanel1.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.flowLayoutPanel1.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            // 
            // lbComment
            // 
            this.lbComment.AutoSize = true;
            this.lbComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbComment.Location = new System.Drawing.Point(3, 3);
            this.lbComment.Margin = new System.Windows.Forms.Padding(0);
            this.lbComment.Name = "lbComment";
            this.lbComment.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lbComment.Size = new System.Drawing.Size(5, 13);
            this.lbComment.TabIndex = 6;
            this.lbComment.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            // 
            // lbProject
            // 
            this.lbProject.AutoSize = true;
            this.lbProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbProject.Location = new System.Drawing.Point(3, 16);
            this.lbProject.Margin = new System.Windows.Forms.Padding(0);
            this.lbProject.Name = "lbProject";
            this.lbProject.Size = new System.Drawing.Size(0, 13);
            this.lbProject.TabIndex = 4;
            this.lbProject.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            // 
            // lblTracker
            // 
            this.lblTracker.AutoSize = true;
            this.lblTracker.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTracker.Location = new System.Drawing.Point(3, 45);
            this.lblTracker.Margin = new System.Windows.Forms.Padding(0);
            this.lblTracker.Name = "lblTracker";
            this.lblTracker.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblTracker.Size = new System.Drawing.Size(5, 13);
            this.lblTracker.TabIndex = 5;
            this.lblTracker.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            // 
            // btnHide
            // 
            this.btnHide.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnHide.Location = new System.Drawing.Point(0, 4);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(22, 91);
            this.btnHide.TabIndex = 5;
            this.btnHide.Text = ">";
            this.btnHide.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnHide.Click += new System.EventHandler(this.OnHideClick);
            this.btnHide.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.btnHide.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            // 
            // cbAutoHide
            // 
            this.cbAutoHide.AutoSize = true;
            this.cbAutoHide.Location = new System.Drawing.Point(4, 103);
            this.cbAutoHide.Name = "cbAutoHide";
            this.cbAutoHide.Size = new System.Drawing.Size(15, 14);
            this.cbAutoHide.TabIndex = 6;
            this.cbAutoHide.UseVisualStyleBackColor = true;
            this.cbAutoHide.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.cbAutoHide.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            // 
            // frmSmall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(160, 122);
            this.Controls.Add(this.cbAutoHide);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lbIdleTime);
            this.Controls.Add(this.lbWorkTime);
            this.Controls.Add(this.btnHide);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSmall";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmSmall";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.OnFormDeactivate);
            this.MouseEnter += new System.EventHandler(this.OnMouseEnter);
            this.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblTracker;
        internal System.Windows.Forms.Label lbParentIssue;
        internal System.Windows.Forms.LinkLabel lbIssue;
        internal System.Windows.Forms.Label lbProject;
        internal System.Windows.Forms.Label lbWorkTime;
        internal System.Windows.Forms.Label lbIdleTime;
        internal System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal System.Windows.Forms.Label lbComment;
        private System.Windows.Forms.Label btnHide;
        private System.Windows.Forms.CheckBox cbAutoHide;
    }
}