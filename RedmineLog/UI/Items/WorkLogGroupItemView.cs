using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI.Items
{
    public partial class WorkLogGroupItemView : UserControl
    {
        private bool daySummary;
        public WorkLogGroupItemView()
        {
            InitializeComponent();
        }
        internal Control Set(string project)
        {
            label1.Text = !string.IsNullOrWhiteSpace(project) ? project : " Global issue ";
            daySummary = false;
            return this;
        }

        internal Control Set(DateTime dateTime)
        {
            label1.Text = dateTime.ToShortDateString();
            daySummary = true;
            return this;
        }

        internal void Update(TimeSpan dayTime)
        {
            label2.Text = dayTime.ToString(@"hh\:mm");

            if (daySummary)
            {
                if (dayTime.TotalHours > 8)
                { label2.BackColor = Color.Green; }
                else
                { label2.BackColor = Color.Red; }
            }
        }
    }
}
