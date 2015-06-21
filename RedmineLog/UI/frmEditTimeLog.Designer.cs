namespace RedmineLog.UI
{
    partial class frmEditTimeLog
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
            this.nHour = new System.Windows.Forms.NumericUpDown();
            this.nMinute = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEventType = new System.Windows.Forms.ComboBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.calWorkDate = new System.Windows.Forms.MonthCalendar();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMinute)).BeginInit();
            this.SuspendLayout();
            // 
            // nHour
            // 
            this.nHour.Location = new System.Drawing.Point(32, 24);
            this.nHour.Name = "nHour";
            this.nHour.Size = new System.Drawing.Size(54, 20);
            this.nHour.TabIndex = 0;
            this.nHour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmEditTimeLog_KeyPress);
            // 
            // nMinute
            // 
            this.nMinute.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nMinute.Location = new System.Drawing.Point(92, 24);
            this.nMinute.Name = "nMinute";
            this.nMinute.Size = new System.Drawing.Size(60, 20);
            this.nMinute.TabIndex = 1;
            this.nMinute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmEditTimeLog_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "HH : MM";
            // 
            // cbEventType
            // 
            this.cbEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEventType.FormattingEnabled = true;
            this.cbEventType.Location = new System.Drawing.Point(11, 50);
            this.cbEventType.Name = "cbEventType";
            this.cbEventType.Size = new System.Drawing.Size(157, 21);
            this.cbEventType.TabIndex = 3;
            this.cbEventType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmEditTimeLog_KeyPress);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(10, 77);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(158, 81);
            this.tbMessage.TabIndex = 4;
            this.tbMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmEditTimeLog_KeyPress);
            // 
            // calWorkDate
            // 
            this.calWorkDate.FirstDayOfWeek = System.Windows.Forms.Day.Monday;
            this.calWorkDate.Location = new System.Drawing.Point(11, 170);
            this.calWorkDate.MaxSelectionCount = 1;
            this.calWorkDate.Name = "calWorkDate";
            this.calWorkDate.ShowToday = false;
            this.calWorkDate.TabIndex = 5;
            this.calWorkDate.TrailingForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.calWorkDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmEditTimeLog_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(10, 334);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(158, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // frmEditTimeLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(177, 359);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.calWorkDate);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.cbEventType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nMinute);
            this.Controls.Add(this.nHour);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmEditTimeLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmEditTimeLog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmEditTimeLog_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.nHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMinute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nHour;
        private System.Windows.Forms.NumericUpDown nMinute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbEventType;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.MonthCalendar calWorkDate;
        internal System.Windows.Forms.Button btnSave;
    }
}