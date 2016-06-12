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
            this.lblPerson = new System.Windows.Forms.Label();
            this.lbIssueType = new System.Windows.Forms.Label();
            this.lbIssueId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // llIssue
            // 
            this.llIssue.Location = new System.Drawing.Point(53, 42);
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
            // lblPerson
            // 
            this.lblPerson.Location = new System.Drawing.Point(4, 66);
            this.lblPerson.Name = "lblPerson";
            this.lblPerson.Size = new System.Drawing.Size(142, 20);
            this.lblPerson.TabIndex = 3;
            this.lblPerson.Text = "Person";
            this.lblPerson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.lbIssueId.Location = new System.Drawing.Point(4, 44);
            this.lbIssueId.Name = "lbIssueId";
            this.lbIssueId.Size = new System.Drawing.Size(38, 13);
            this.lbIssueId.TabIndex = 5;
            this.lbIssueId.Text = "#1234";
            // 
            // SearchItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.Controls.Add(this.lbIssueId);
            this.Controls.Add(this.lbIssueType);
            this.Controls.Add(this.lblPerson);
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
        private System.Windows.Forms.Label lblPerson;
        private System.Windows.Forms.Label lbIssueType;
        private System.Windows.Forms.Label lbIssueId;
    }
}
