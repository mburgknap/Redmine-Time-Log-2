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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIssueLog));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cIssueId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIssueDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIssueId,
            this.cTime,
            this.cProject,
            this.cIssueDetail});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 21;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(343, 509);
            this.dataGridView1.TabIndex = 2;
            // 
            // cIssueId
            // 
            this.cIssueId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cIssueId.DefaultCellStyle = dataGridViewCellStyle1;
            this.cIssueId.HeaderText = "#id";
            this.cIssueId.MinimumWidth = 40;
            this.cIssueId.Name = "cIssueId";
            this.cIssueId.ReadOnly = true;
            this.cIssueId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIssueId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cIssueId.Width = 40;
            // 
            // cTime
            // 
            this.cTime.FillWeight = 57.91165F;
            this.cTime.HeaderText = "Time";
            this.cTime.Name = "cTime";
            this.cTime.ReadOnly = true;
            // 
            // cProject
            // 
            this.cProject.FillWeight = 80.00001F;
            this.cProject.HeaderText = "Project";
            this.cProject.Name = "cProject";
            this.cProject.ReadOnly = true;
            this.cProject.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIssueDetail
            // 
            this.cIssueDetail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cIssueDetail.DefaultCellStyle = dataGridViewCellStyle2;
            this.cIssueDetail.FillWeight = 162.0884F;
            this.cIssueDetail.HeaderText = "Issue";
            this.cIssueDetail.Name = "cIssueDetail";
            this.cIssueDetail.ReadOnly = true;
            this.cIssueDetail.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIssueDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 509);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSearch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmSearch";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnSearchLoad);
            this.MouseLeave += new System.EventHandler(this.OnSearchMouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIssueId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIssueDetail;
    }
}