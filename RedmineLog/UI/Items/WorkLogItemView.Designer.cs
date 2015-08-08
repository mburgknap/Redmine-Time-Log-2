namespace RedmineLog.UI.Items
{
    partial class WorkLogItemView
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
            this.lblIdIssue = new System.Windows.Forms.Label();
            this.lblComment = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblActivityType = new System.Windows.Forms.Label();
            this.separator = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblIdIssue
            // 
            this.lblIdIssue.BackColor = System.Drawing.Color.Transparent;
            this.lblIdIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblIdIssue.Location = new System.Drawing.Point(3, 15);
            this.lblIdIssue.Name = "lblIdIssue";
            this.lblIdIssue.Size = new System.Drawing.Size(42, 13);
            this.lblIdIssue.TabIndex = 0;
            this.lblIdIssue.Text = "13465";
            this.lblIdIssue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblComment
            // 
            this.lblComment.BackColor = System.Drawing.Color.Transparent;
            this.lblComment.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblComment.Location = new System.Drawing.Point(52, 6);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(178, 39);
            this.lblComment.TabIndex = 2;
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTime.ForeColor = System.Drawing.Color.Red;
            this.lblTime.Location = new System.Drawing.Point(236, 6);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(76, 13);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "label4";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblActivityType
            // 
            this.lblActivityType.BackColor = System.Drawing.Color.Transparent;
            this.lblActivityType.ForeColor = System.Drawing.Color.Blue;
            this.lblActivityType.Location = new System.Drawing.Point(236, 24);
            this.lblActivityType.Name = "lblActivityType";
            this.lblActivityType.Size = new System.Drawing.Size(79, 18);
            this.lblActivityType.TabIndex = 4;
            this.lblActivityType.Text = "Projektowanie";
            this.lblActivityType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // separator
            // 
            this.separator.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.separator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.separator.Location = new System.Drawing.Point(0, 48);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(315, 1);
            this.separator.TabIndex = 5;
            // 
            // WorkLogItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.separator);
            this.Controls.Add(this.lblActivityType);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.lblIdIssue);
            this.Name = "WorkLogItemView";
            this.Size = new System.Drawing.Size(315, 49);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblIdIssue;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblActivityType;
        private System.Windows.Forms.Panel separator;
    }
}
