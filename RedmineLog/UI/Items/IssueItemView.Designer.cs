namespace RedmineLog.UI.Items
{
    partial class IssueItemView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbProject = new System.Windows.Forms.Label();
            this.lkIssue = new System.Windows.Forms.LinkLabel();
            this.lbParentIssue = new System.Windows.Forms.Label();
            this.lbTime = new System.Windows.Forms.Label();
            this.lbTracker = new System.Windows.Forms.Label();
            this.lbIssueId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbProject
            // 
            this.lbProject.Location = new System.Drawing.Point(3, 6);
            this.lbProject.Name = "lbProject";
            this.lbProject.Size = new System.Drawing.Size(198, 13);
            this.lbProject.TabIndex = 0;
            // 
            // lkIssue
            // 
            this.lkIssue.Location = new System.Drawing.Point(40, 44);
            this.lkIssue.Name = "lkIssue";
            this.lkIssue.Size = new System.Drawing.Size(193, 29);
            this.lkIssue.TabIndex = 1;
            // 
            // lbParentIssue
            // 
            this.lbParentIssue.Location = new System.Drawing.Point(12, 25);
            this.lbParentIssue.Name = "lbParentIssue";
            this.lbParentIssue.Size = new System.Drawing.Size(193, 13);
            this.lbParentIssue.TabIndex = 2;
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbTime.ForeColor = System.Drawing.Color.Red;
            this.lbTime.Location = new System.Drawing.Point(239, 44);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(57, 13);
            this.lbTime.TabIndex = 3;
            this.lbTime.Text = "01:03:34";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTracker
            // 
            this.lbTracker.ForeColor = System.Drawing.Color.DarkBlue;
            this.lbTracker.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lbTracker.Location = new System.Drawing.Point(229, 5);
            this.lbTracker.Name = "lbTracker";
            this.lbTracker.Size = new System.Drawing.Size(73, 33);
            this.lbTracker.TabIndex = 4;
            this.lbTracker.Text = "zadanie developerskie";
            this.lbTracker.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbIssueId
            // 
            this.lbIssueId.AutoSize = true;
            this.lbIssueId.Location = new System.Drawing.Point(0, 43);
            this.lbIssueId.Name = "lbIssueId";
            this.lbIssueId.Size = new System.Drawing.Size(38, 13);
            this.lbIssueId.TabIndex = 5;
            this.lbIssueId.Text = "#1234";
            // 
            // IssueItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.Controls.Add(this.lbIssueId);
            this.Controls.Add(this.lbTracker);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.lbParentIssue);
            this.Controls.Add(this.lkIssue);
            this.Controls.Add(this.lbProject);
            this.Name = "IssueItemView";
            this.Size = new System.Drawing.Size(308, 79);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbProject;
        private System.Windows.Forms.LinkLabel lkIssue;
        private System.Windows.Forms.Label lbParentIssue;
        internal System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label lbTracker;
        private System.Windows.Forms.Label lbIssueId;
    }
}
