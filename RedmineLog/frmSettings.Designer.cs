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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbRedmineURL = new System.Windows.Forms.TextBox();
            this.cbSaveSettings = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(13, 54);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(90, 13);
            this.Label2.TabIndex = 11;
            this.Label2.Text = "Redmine API Key";
            // 
            // tbApiKey
            // 
            this.tbApiKey.Location = new System.Drawing.Point(16, 70);
            this.tbApiKey.Name = "tbApiKey";
            this.tbApiKey.Size = new System.Drawing.Size(324, 20);
            this.tbApiKey.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(265, 95);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(184, 95);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 18);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(74, 13);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Redmine URL";
            // 
            // tbRedmineURL
            // 
            this.tbRedmineURL.Location = new System.Drawing.Point(15, 34);
            this.tbRedmineURL.Name = "tbRedmineURL";
            this.tbRedmineURL.Size = new System.Drawing.Size(325, 20);
            this.tbRedmineURL.TabIndex = 6;
            // 
            // cbSaveSettings
            // 
            this.cbSaveSettings.AutoSize = true;
            this.cbSaveSettings.Checked = true;
            this.cbSaveSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSaveSettings.Location = new System.Drawing.Point(16, 101);
            this.cbSaveSettings.Name = "cbSaveSettings";
            this.cbSaveSettings.Size = new System.Drawing.Size(110, 17);
            this.cbSaveSettings.TabIndex = 12;
            this.cbSaveSettings.Text = "Zapisz ustawienia";
            this.cbSaveSettings.UseVisualStyleBackColor = true;
            // 
            // frmSettings2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 137);
            this.Controls.Add(this.cbSaveSettings);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.tbApiKey);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.tbRedmineURL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox tbApiKey;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnConnect;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbRedmineURL;
        private System.Windows.Forms.CheckBox cbSaveSettings;
    }
}