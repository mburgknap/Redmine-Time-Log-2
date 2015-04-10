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
            this.lMainIssue = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbWorkTime
            // 
            this.lbWorkTime.AutoSize = true;
            this.lbWorkTime.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbWorkTime.ForeColor = System.Drawing.Color.ForestGreen;
            this.lbWorkTime.Location = new System.Drawing.Point(5, 48);
            this.lbWorkTime.Name = "lbWorkTime";
            this.lbWorkTime.Size = new System.Drawing.Size(72, 21);
            this.lbWorkTime.TabIndex = 0;
            this.lbWorkTime.Text = "00:00:00";
            this.lbWorkTime.Click += new System.EventHandler(this.OnFormClick);
            // 
            // lbIdleTime
            // 
            this.lbIdleTime.AutoSize = true;
            this.lbIdleTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbIdleTime.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbIdleTime.Location = new System.Drawing.Point(79, 62);
            this.lbIdleTime.Name = "lbIdleTime";
            this.lbIdleTime.Size = new System.Drawing.Size(55, 15);
            this.lbIdleTime.TabIndex = 1;
            this.lbIdleTime.Text = "00:00:00";
            this.lbIdleTime.Click += new System.EventHandler(this.OnFormClick);
            // 
            // lbParentIssue
            // 
            this.lbParentIssue.AutoSize = true;
            this.lbParentIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbParentIssue.Location = new System.Drawing.Point(3, 3);
            this.lbParentIssue.Margin = new System.Windows.Forms.Padding(0);
            this.lbParentIssue.Name = "lbParentIssue";
            this.lbParentIssue.Size = new System.Drawing.Size(0, 13);
            this.lbParentIssue.TabIndex = 2;
            // 
            // lMainIssue
            // 
            this.lMainIssue.AutoSize = true;
            this.lMainIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lMainIssue.Location = new System.Drawing.Point(18, 18);
            this.lMainIssue.Margin = new System.Windows.Forms.Padding(15, 2, 0, 0);
            this.lMainIssue.Name = "lMainIssue";
            this.lMainIssue.Size = new System.Drawing.Size(0, 13);
            this.lMainIssue.TabIndex = 3;
            this.lMainIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnlIssueLinkClicked);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lbParentIssue);
            this.flowLayoutPanel1.Controls.Add(this.lMainIssue);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(130, 46);
            this.flowLayoutPanel1.TabIndex = 4;
            this.flowLayoutPanel1.Click += new System.EventHandler(this.OnFormClick);
            // 
            // frmSmall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(136, 78);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lbIdleTime);
            this.Controls.Add(this.lbWorkTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSmall";
            this.ShowInTaskbar = false;
            this.Text = "frmSmall";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Click += new System.EventHandler(this.OnFormClick);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbWorkTime;
        private System.Windows.Forms.Label lbIdleTime;
        private System.Windows.Forms.Label lbParentIssue;
        private System.Windows.Forms.LinkLabel lMainIssue;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}