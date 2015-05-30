using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Logic;
using RedmineLog.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI
{
    public partial class frmEditTimeLog : Form
    {
        public Action OnChange;

        private TimeEntry timeEntry;
        public frmEditTimeLog()
        {
            InitializeComponent();
        }

        internal void Init(TimeEntry inTimeEntry)
        {
            timeEntry = inTimeEntry;

            this.Text = "(" + timeEntry.Issue.Id.ToString() + ") " + timeEntry.Project.Name;

            var time = new TimeSpan(0, (int)(timeEntry.Hours * 60), 0);

            nHour.Value = time.Hours;
            nMinute.Value = time.Minutes;

            cbEventType.Items.Clear();
            cbEventType.DataSource = App.Context.Activity;
            cbEventType.DisplayMember = "Name";
            cbEventType.ValueMember = "Id";

            for (int i = 0; i < App.Context.Activity.Count; i++)
            {
                if (App.Context.Activity[i].Id == timeEntry.Activity.Id)
                {
                    cbEventType.SelectedItem = cbEventType.Items[i];
                    break;
                }
            }
            
            calWorkDate.SetDate(timeEntry.SpentOn.GetValueOrDefault(DateTime.Now));
            calWorkDate.AddMonthlyBoldedDate(timeEntry.SpentOn.GetValueOrDefault(DateTime.Now));

            tbMessage.Text = timeEntry.Comments;
        }

        private void frmEditTimeLog_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 19)
            {
                try
                {
                    var manager = new RedmineManager(App.Context.Config.Url, App.Context.Config.ApiKey);

                    timeEntry.Hours = nHour.Value + nMinute.Value / 60;

                    timeEntry.SpentOn = calWorkDate.SelectionStart;
                    timeEntry.Comments = tbMessage.Text;

                    timeEntry.Activity.Id = (int)cbEventType.SelectedValue;
                    timeEntry.Activity.Name = cbEventType.Text;


                    manager.UpdateObject<TimeEntry>(timeEntry.Id.ToString(), timeEntry);

                    this.Close();

                    OnChange();
                }
                catch (Exception ex)
                {
                    AppLogger.Log.Error("frmEditTimeLog_KeyPress", ex);
                }
            }
        }
    }
}
