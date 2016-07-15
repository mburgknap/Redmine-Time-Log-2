namespace RedmineLog.UI.Items
{
    partial class SearchItemView
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
            this.llIssue = new System.Windows.Forms.LinkLabel();
            this.lblProject = new System.Windows.Forms.Label();
            this.lblParentIssue = new System.Windows.Forms.Label();
            this.lbIssueType = new System.Windows.Forms.Label();
            this.lbIssueId = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // llIssue
            // 
            this.llIssue.Location = new System.Drawing.Point(53, 43);
            this.llIssue.Name = "llIssue";
            this.llIssue.Size = new System.Drawing.Size(255, 20);
            this.llIssue.TabIndex = 0;
            this.llIssue.TabStop = true;
            this.llIssue.Text = "Issue";
            // 
            // lblProject
            // 
            this.lblProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblProject.Location = new System.Drawing.Point(4, 4);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(202, 19);
            this.lblProject.TabIndex = 1;
            this.lblProject.Text = "Project";
            // 
            // lblParentIssue
            // 
            this.lblParentIssue.Location = new System.Drawing.Point(14, 23);
            this.lblParentIssue.Name = "lblParentIssue";
            this.lblParentIssue.Size = new System.Drawing.Size(291, 13);
            this.lblParentIssue.TabIndex = 2;
            this.lblParentIssue.Text = "Parent Issue";
            // 
            // lbIssueType
            // 
            this.lbIssueType.Location = new System.Drawing.Point(152, 66);
            this.lbIssueType.Name = "lbIssueType";
            this.lbIssueType.Size = new System.Drawing.Size(153, 23);
            this.lbIssueType.TabIndex = 4;
            this.lbIssueType.Text = "Issue type";
            this.lbIssueType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbIssueId
            // 
            this.lbIssueId.AutoSize = true;
            this.lbIssueId.Location = new System.Drawing.Point(5, 44);
            this.lbIssueId.Name = "lbIssueId";
            this.lbIssueId.Size = new System.Drawing.Size(38, 13);
            this.lbIssueId.TabIndex = 5;
            this.lbIssueId.Text = "#1234";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(308, 1);
            this.panel1.TabIndex = 6;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblUser.Location = new System.Drawing.Point(14, 71);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(33, 13);
            this.lblUser.TabIndex = 7;
            this.lblUser.Text = "User";
            // 
            // SearchItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbIssueId);
            this.Controls.Add(this.lbIssueType);
            this.Controls.Add(this.lblParentIssue);
            this.Controls.Add(this.lblProject);
            this.Controls.Add(this.llIssue);
            this.Name = "SearchItemView";
            this.Size = new System.Drawing.Size(308, 90);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel llIssue;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.Label lblParentIssue;
        private System.Windows.Forms.Label lbIssueType;
        private System.Windows.Forms.Label lbIssueId;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lblUser;
    }
}
