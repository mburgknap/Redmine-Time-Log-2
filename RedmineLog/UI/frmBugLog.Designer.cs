namespace RedmineLog.UI
{
    partial class frmBugLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBugLog));
            this.cHeader = new RedmineLog.UI.Items.BugLogItemView();
            this.fpBugList = new RedmineLog.UI.Common.ExFlowLayoutPanel(this.components);
            this.SuspendLayout();
            // 
            // cHeader
            // 
            this.cHeader.BackColor = System.Drawing.Color.Wheat;
            this.cHeader.Data = null;
            this.cHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.cHeader.Location = new System.Drawing.Point(0, 0);
            this.cHeader.Name = "cHeader";
            this.cHeader.Size = new System.Drawing.Size(347, 43);
            this.cHeader.TabIndex = 1;
            // 
            // fpBugList
            // 
            this.fpBugList.AutoScroll = true;
            this.fpBugList.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpBugList.Location = new System.Drawing.Point(0, 43);
            this.fpBugList.Name = "fpBugList";
            this.fpBugList.Size = new System.Drawing.Size(347, 460);
            this.fpBugList.TabIndex = 2;
            // 
            // frmBugLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 509);
            this.Controls.Add(this.fpBugList);
            this.Controls.Add(this.cHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBugLog";
            this.Text = "frmBugLog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnBugLogLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private Items.BugLogItemView cHeader;
        internal Common.ExFlowLayoutPanel fpBugList;

    }
}