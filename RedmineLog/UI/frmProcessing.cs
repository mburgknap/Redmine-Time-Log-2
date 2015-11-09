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
        private Control control;
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
            Show(inForm, null, inAction, inFinish);
        }

        public void Show(Form inForm, Control inControl, Action inAction, Action inFinish = null)
        {
            action = inAction;
            finish = inFinish;
            form = inForm;
            control = inControl;

            if (inForm.Visible)
            {
                SetupLocation();
                ShowProgress();
            }
            else
            {
                inForm.Shown -= OnFormShown;
                inForm.Shown += OnFormShown;
            }
        }

        private void ShowProgress()
        {
            this.Load += (s, e) =>
            {
                Task tmpTask = null;
                tmpTask = new Task(() =>
                  {
                      tmpTask.Wait(50);
                      DoWork();
                      tmpTask.Wait(50);
                  });
                tmpTask.Start();
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

        void OnFormShown(object sender, EventArgs e)
        {
            SetupLocation();
            ShowProgress();
        }

        private void SetupLocation()
        {
            if (control != null)
                SetupLocation(form, control);
            else
                SetupLocation(form);
        }

        private void SetupLocation(Control form)
        {
            this.Width = form.Width;
            this.Height = form.Height;
            Location = new Point(form.Location.X, form.Location.Y);
        }

        private void SetupLocation(Form form, Control control)
        {
            this.Width = control.Width;
            this.Height = control.Height;
            Location = new Point(form.Location.X + control.Location.X, form.Location.Y + control.Location.Y);
        }

    }
}
