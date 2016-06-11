namespace RedmineLog
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.Label2 = new System.Windows.Forms.Label();
            this.tbApiKey = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbRedmineURL = new System.Windows.Forms.TextBox();
            this.btnReloadCache = new System.Windows.Forms.Button();
            this.cbDisplay = new System.Windows.Forms.ComboBox();
            this.tbWorkHours = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbTimer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(13, 45);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(90, 13);
            this.Label2.TabIndex = 11;
            this.Label2.Text = "Redmine API Key";
            // 
            // tbApiKey
            // 
            this.tbApiKey.Location = new System.Drawing.Point(16, 61);
            this.tbApiKey.Name = "tbApiKey";
            this.tbApiKey.Size = new System.Drawing.Size(324, 20);
            this.tbApiKey.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(183, 135);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(157, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(74, 13);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Redmine URL";
            // 
            // tbRedmineURL
            // 
            this.tbRedmineURL.Location = new System.Drawing.Point(15, 25);
            this.tbRedmineURL.Name = "tbRedmineURL";
            this.tbRedmineURL.Size = new System.Drawing.Size(325, 20);
            this.tbRedmineURL.TabIndex = 6;
            // 
            // btnReloadCache
            // 
            this.btnReloadCache.Location = new System.Drawing.Point(12, 135);
            this.btnReloadCache.Name = "btnReloadCache";
            this.btnReloadCache.Size = new System.Drawing.Size(89, 23);
            this.btnReloadCache.TabIndex = 12;
            this.btnReloadCache.Text = "Reload cache";
            this.btnReloadCache.UseVisualStyleBackColor = true;
            // 
            // cbDisplay
            // 
            this.cbDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisplay.FormattingEnabled = true;
            this.cbDisplay.Location = new System.Drawing.Point(234, 84);
            this.cbDisplay.Name = "cbDisplay";
            this.cbDisplay.Size = new System.Drawing.Size(106, 21);
            this.cbDisplay.TabIndex = 13;
            // 
            // tbWorkHours
            // 
            this.tbWorkHours.Location = new System.Drawing.Point(71, 84);
            this.tbWorkHours.Name = "tbWorkHours";
            this.tbWorkHours.Size = new System.Drawing.Size(26, 20);
            this.tbWorkHours.TabIndex = 14;
            this.tbWorkHours.Text = "8";
            this.tbWorkHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Work day :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(180, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(103, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "hours";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(180, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Timer";
            // 
            // cbTimer
            // 
            this.cbTimer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTimer.FormattingEnabled = true;
            this.cbTimer.Location = new System.Drawing.Point(234, 108);
            this.cbTimer.Name = "cbTimer";
            this.cbTimer.Size = new System.Drawing.Size(106, 21);
            this.cbTimer.TabIndex = 19;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 170);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbTimer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbWorkHours);
            this.Controls.Add(this.cbDisplay);
            this.Controls.Add(this.btnReloadCache);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.tbApiKey);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.tbRedmineURL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox tbApiKey;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbRedmineURL;
        internal System.Windows.Forms.Button btnReloadCache;
        internal System.Windows.Forms.ComboBox cbDisplay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox tbWorkHours;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.ComboBox cbTimer;
    }
}