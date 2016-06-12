namespace RedmineLog.UI
{
    partial class frmSearch
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbSearchText = new System.Windows.Forms.TextBox();
            this.fpIssueItemList = new RedmineLog.UI.Common.ExFlowLayoutPanel(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(286, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 20);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.TabStop = false;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // tbSearchText
            // 
            this.tbSearchText.AcceptsTab = true;
            this.tbSearchText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSearchText.Location = new System.Drawing.Point(0, 0);
            this.tbSearchText.Name = "tbSearchText";
            this.tbSearchText.Size = new System.Drawing.Size(255, 20);
            this.tbSearchText.TabIndex = 0;
            this.tbSearchText.TabStop = false;
            // 
            // fpIssueItemList
            // 
            this.fpIssueItemList.AutoScroll = true;
            this.fpIssueItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpIssueItemList.Location = new System.Drawing.Point(0, 20);
            this.fpIssueItemList.Name = "fpIssueItemList";
            this.fpIssueItemList.Size = new System.Drawing.Size(343, 474);
            this.fpIssueItemList.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbSearchText);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 20);
            this.panel1.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Location = new System.Drawing.Point(255, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(31, 20);
            this.btnClear.TabIndex = 1;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "X";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 494);
            this.Controls.Add(this.fpIssueItemList);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmSearch";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnSearchLoad);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnSearch;
        internal System.Windows.Forms.TextBox tbSearchText;
        internal Common.ExFlowLayoutPanel fpIssueItemList;
        internal System.Windows.Forms.Button btnClear;
    }
}