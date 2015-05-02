namespace RedmineLog
{
    partial class frmIdleInfo
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbParentIssue = new System.Windows.Forms.Label();
            this.lbIssue = new System.Windows.Forms.LinkLabel();
            this.lbtime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(163, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Issues";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 79);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(225, 77);
            this.textBox1.TabIndex = 2;
            // 
            // lbParentIssue
            // 
            this.lbParentIssue.AutoSize = true;
            this.lbParentIssue.Location = new System.Drawing.Point(105, 13);
            this.lbParentIssue.Name = "lbParentIssue";
            this.lbParentIssue.Size = new System.Drawing.Size(35, 13);
            this.lbParentIssue.TabIndex = 3;
            this.lbParentIssue.Text = "label1";
            // 
            // lbIssue
            // 
            this.lbIssue.AutoSize = true;
            this.lbIssue.Location = new System.Drawing.Point(119, 38);
            this.lbIssue.Name = "lbIssue";
            this.lbIssue.Size = new System.Drawing.Size(55, 13);
            this.lbIssue.TabIndex = 4;
            this.lbIssue.TabStop = true;
            this.lbIssue.Text = "linkLabel1";
            // 
            // lbtime
            // 
            this.lbtime.AutoSize = true;
            this.lbtime.Location = new System.Drawing.Point(91, 167);
            this.lbtime.Name = "lbtime";
            this.lbtime.Size = new System.Drawing.Size(49, 13);
            this.lbtime.TabIndex = 5;
            this.lbtime.Text = "00:00:00";
            // 
            // frmIdleInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 197);
            this.Controls.Add(this.lbtime);
            this.Controls.Add(this.lbIssue);
            this.Controls.Add(this.lbParentIssue);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIdleInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Idel info";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmIdleInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lbParentIssue;
        private System.Windows.Forms.LinkLabel lbIssue;
        private System.Windows.Forms.Label lbtime;
    }
}