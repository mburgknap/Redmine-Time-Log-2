using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI
{
    public partial class frmProcessing : Form
    {
        private Action action;
        private Form form;
        private Action finish;
        public frmProcessing()
        {
            InitializeComponent();
            FormClosed += OnProcessingClosed;
        }

        void OnProcessingClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void Show(Form inForm, Action inAction, Action inFinish = null)
        {
            action = inAction;
            finish = inFinish;
            form = inForm;

            if (inForm.Visible)
            {
                SetupLocation(inForm);
                ShowProgress();
            }
            else
            {
                inForm.Shown -= inFormShown;
                inForm.Shown += inFormShown;
            }
        }

        private void ShowProgress()
        {
            this.Load += (s, e) =>
            {
                new Task(() =>
                {
                    DoWork();

                }).Start();
            };

            Show();
        }

        void DoWork()
        {
            if (action != null) action();

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.Close();
                    if (finish != null) finish();
                }));
            }
            else
            {
                this.Close();
                if (finish != null) finish();
            }

        }

        void inFormShown(object sender, EventArgs e)
        {
            SetupLocation((Form)sender);
            ShowProgress();
        }

        private void SetupLocation(Form form)
        {
            this.Width = form.Width;
            this.Height = form.Height;
            Location = new Point(form.Location.X, form.Location.Y);
        }

    }
}
