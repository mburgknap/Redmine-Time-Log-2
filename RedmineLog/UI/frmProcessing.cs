using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI
{
    public partial class frmProcessing : Form
    {
        public frmProcessing()
        {
            InitializeComponent();
        }

        public void Show(Form inForm, Action inAction)
        {
            inForm.Load -= inForm_Load;
            inForm.Load += inForm_Load;
            inForm.FormClosed -= inForm_FormClosed;
            inForm.FormClosed += inForm_FormClosed;

            inForm_Load(inForm, EventArgs.Empty);
            Show();
            new Task(() =>
            {
                if (inAction != null) inAction();
                this.Close();
            }).Start();
        }

        void inForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this.IsDisposed)
                this.Close();
        }

        void inForm_Load(object sender, EventArgs e)
        {
            this.Width = ((Form)sender).Width;
            this.Height = ((Form)sender).Height;
            Location = new Point(((Form)sender).Location.X, ((Form)sender).Location.Y);
        }
    }
}
