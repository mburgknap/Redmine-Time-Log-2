﻿namespace RedmineLog.UI.Items
{
    partial class IssueLogItemView
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
            this.lblIssue = new System.Windows.Forms.LinkLabel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblActivityType = new System.Windows.Forms.Label();
            this.separator = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblIdIssue
            // 
            this.lblIdIssue.BackColor = System.Drawing.Color.Transparent;
            this.lblIdIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblIdIssue.Location = new System.Drawing.Point(3, 2);
            this.lblIdIssue.Name = "lblIdIssue";
            this.lblIdIssue.Size = new System.Drawing.Size(51, 13);
            this.lblIdIssue.TabIndex = 0;
            this.lblIdIssue.Text = "#1234";
            this.lblIdIssue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIssue
            // 
            this.lblIssue.BackColor = System.Drawing.Color.Transparent;
            this.lblIssue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblIssue.Location = new System.Drawing.Point(50, 17);
            this.lblIssue.Name = "lblIssue";
            this.lblIssue.Size = new System.Drawing.Size(262, 20);
            this.lblIssue.TabIndex = 2;
            this.lblIssue.DragEnter += new System.Windows.Forms.DragEventHandler(this.lblIssue_DragEnter);
            this.lblIssue.DragLeave += new System.EventHandler(this.lblIssue_DragLeave);
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTime.ForeColor = System.Drawing.Color.Red;
            this.lblTime.Location = new System.Drawing.Point(247, 2);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(68, 13);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "label4";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblActivityType
            // 
            this.lblActivityType.BackColor = System.Drawing.Color.Transparent;
            this.lblActivityType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblActivityType.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblActivityType.Location = new System.Drawing.Point(59, -1);
            this.lblActivityType.Name = "lblActivityType";
            this.lblActivityType.Size = new System.Drawing.Size(192, 17);
            this.lblActivityType.TabIndex = 4;
            this.lblActivityType.Text = "zadanie developerskie";
            this.lblActivityType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // separator
            // 
            this.separator.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.separator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.separator.Location = new System.Drawing.Point(0, 43);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(315, 1);
            this.separator.TabIndex = 5;
            // 
            // IssueLogItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.separator);
            this.Controls.Add(this.lblActivityType);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblIssue);
            this.Controls.Add(this.lblIdIssue);
            this.Name = "IssueLogItemView";
            this.Size = new System.Drawing.Size(315, 44);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblIdIssue;
        private System.Windows.Forms.LinkLabel lblIssue;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblActivityType;
        private System.Windows.Forms.Panel separator;
    }
}
