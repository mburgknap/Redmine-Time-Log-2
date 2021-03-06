﻿namespace RedmineLog.UI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditTimeLog));
            this.nHour = new System.Windows.Forms.NumericUpDown();
            this.nMinute = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbActivityType = new System.Windows.Forms.ComboBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.calWorkDate = new System.Windows.Forms.MonthCalendar();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMinute)).BeginInit();
            this.SuspendLayout();
            // 
            // nHour
            // 
            this.nHour.Location = new System.Drawing.Point(168, 26);
            this.nHour.Name = "nHour";
            this.nHour.Size = new System.Drawing.Size(54, 20);
            this.nHour.TabIndex = 0;
            // 
            // nMinute
            // 
            this.nMinute.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nMinute.Location = new System.Drawing.Point(228, 26);
            this.nMinute.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nMinute.Name = "nMinute";
            this.nMinute.Size = new System.Drawing.Size(60, 20);
            this.nMinute.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "HH : MM";
            // 
            // cbEventType
            // 
            this.cbActivityType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbActivityType.FormattingEnabled = true;
            this.cbActivityType.Location = new System.Drawing.Point(170, 67);
            this.cbActivityType.Name = "cbEventType";
            this.cbActivityType.Size = new System.Drawing.Size(118, 21);
            this.cbActivityType.TabIndex = 3;
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(6, 7);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(158, 81);
            this.tbMessage.TabIndex = 4;
            // 
            // calWorkDate
            // 
            this.calWorkDate.FirstDayOfWeek = System.Windows.Forms.Day.Monday;
            this.calWorkDate.Location = new System.Drawing.Point(6, 100);
            this.calWorkDate.MaxSelectionCount = 1;
            this.calWorkDate.Name = "calWorkDate";
            this.calWorkDate.ShowToday = false;
            this.calWorkDate.TabIndex = 5;
            this.calWorkDate.TrailingForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(130, 262);
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
            this.ClientSize = new System.Drawing.Size(292, 290);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.calWorkDate);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.cbActivityType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nMinute);
            this.Controls.Add(this.nHour);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEditTimeLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmEditTimeLog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.nHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMinute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.ComboBox cbActivityType;
        internal System.Windows.Forms.NumericUpDown nHour;
        internal System.Windows.Forms.NumericUpDown nMinute;
        internal System.Windows.Forms.TextBox tbMessage;
        internal System.Windows.Forms.MonthCalendar calWorkDate;
    }
}