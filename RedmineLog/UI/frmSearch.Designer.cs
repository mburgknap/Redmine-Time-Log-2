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
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.cbProjects = new System.Windows.Forms.ComboBox();
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
            this.fpIssueItemList.Location = new System.Drawing.Point(0, 68);
            this.fpIssueItemList.Name = "fpIssueItemList";
            this.fpIssueItemList.Size = new System.Drawing.Size(343, 470);
            this.fpIssueItemList.TabIndex = 2;
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo.Location = new System.Drawing.Point(0, 45);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblInfo.Size = new System.Drawing.Size(343, 23);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Brak zadań spełniających zadane wyszukiwanie";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbSearchText);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
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
            // cbProjects
            // 
            this.cbProjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProjects.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbProjects.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbProjects.FormattingEnabled = true;
            this.cbProjects.Location = new System.Drawing.Point(0, 0);
            this.cbProjects.Name = "cbProjects";
            this.cbProjects.Size = new System.Drawing.Size(343, 25);
            this.cbProjects.TabIndex = 52;
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 538);
            this.Controls.Add(this.fpIssueItemList);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbProjects);
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
        internal System.Windows.Forms.Label lblInfo;
        internal System.Windows.Forms.ComboBox cbProjects;
    }
}