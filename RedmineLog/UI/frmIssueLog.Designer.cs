namespace RedmineLog
{
    partial class frmIssueLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIssueLog));
            this.fpLogItemList = new RedmineLog.UI.Common.ExFlowLayoutPanel();
            this.cHeader = new RedmineLog.UI.Items.IssueLogItem();
            this.SuspendLayout();
            // 
            // fpLogItemList
            // 
            this.fpLogItemList.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpLogItemList.Location = new System.Drawing.Point(0, 49);
            this.fpLogItemList.Name = "fpLogItemList";
            this.fpLogItemList.Size = new System.Drawing.Size(343, 460);
            this.fpLogItemList.TabIndex = 3;
            // 
            // cHeader
            // 
            this.cHeader.BackColor = System.Drawing.Color.Wheat;
            this.cHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.cHeader.Location = new System.Drawing.Point(0, 0);
            this.cHeader.Name = "cHeader";
            this.cHeader.Size = new System.Drawing.Size(343, 49);
            this.cHeader.TabIndex = 0;
            // 
            // frmIssueLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 509);
            this.Controls.Add(this.fpLogItemList);
            this.Controls.Add(this.cHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmIssueLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmSearch";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnSearchLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Items.IssueLogItem cHeader;
        internal RedmineLog.UI.Common.ExFlowLayoutPanel fpLogItemList;
    }
}