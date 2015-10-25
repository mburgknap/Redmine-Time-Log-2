namespace RedmineLog
{
    partial class frmWorkLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWorkLog));
            this.blLoadMore = new System.Windows.Forms.Button();
            this.fpWokLogList = new RedmineLog.UI.Common.ExFlowLayoutPanel(this.components);
            this.cHeader = new RedmineLog.UI.Items.WorkLogItemView();
            this.SuspendLayout();
            // 
            // blLoadMore
            // 
            this.blLoadMore.AutoEllipsis = true;
            this.blLoadMore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.blLoadMore.Location = new System.Drawing.Point(0, 486);
            this.blLoadMore.Name = "blLoadMore";
            this.blLoadMore.Size = new System.Drawing.Size(343, 23);
            this.blLoadMore.TabIndex = 3;
            this.blLoadMore.Text = "More ...";
            this.blLoadMore.UseVisualStyleBackColor = true;
            // 
            // fpWokLogList
            // 
            this.fpWokLogList.AutoScroll = true;
            this.fpWokLogList.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpWokLogList.Location = new System.Drawing.Point(0, 68);
            this.fpWokLogList.Name = "fpWokLogList";
            this.fpWokLogList.Size = new System.Drawing.Size(343, 418);
            this.fpWokLogList.TabIndex = 4;
            // 
            // cHeader
            // 
            this.cHeader.BackColor = System.Drawing.Color.Wheat;
            this.cHeader.Data = null;
            this.cHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.cHeader.Location = new System.Drawing.Point(0, 0);
            this.cHeader.Name = "cHeader";
            this.cHeader.Size = new System.Drawing.Size(343, 68);
            this.cHeader.TabIndex = 5;
            // 
            // frmWorkLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 509);
            this.Controls.Add(this.fpWokLogList);
            this.Controls.Add(this.cHeader);
            this.Controls.Add(this.blLoadMore);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmWorkLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmSearch";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnWorkLogLoad);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button blLoadMore;
        private UI.Items.WorkLogItemView cHeader;
        internal UI.Common.ExFlowLayoutPanel fpWokLogList;
    }
}