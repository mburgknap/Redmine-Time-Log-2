using Appccelerate.EventBroker;
using Ninject;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using RedmineLog.Logic;
using RedmineLog.UI.Common;
using System;
using System.Drawing;
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
            this.Initialize<EditLog.IView, frmEditTimeLog>();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width - 350, SystemInformation.VirtualScreen.Height - this.Height - 160);
        }
    }

    internal class EditTimeLogView : EditLog.IView, IView<frmEditTimeLog>
    {
        private EditLog.IModel model;

        private frmEditTimeLog Form;

        [Inject]
        public EditTimeLogView(EditLog.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
            inGlobalEvent.Register(this);
        }

        [EventPublication(EditLog.Events.Load, typeof(Publish<EditLog.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(EditLog.Events.Save, typeof(Publish<EditLog.IView>))]
        public event EventHandler SaveEvent;

        public void Init(frmEditTimeLog inView)
        {
            Form = inView;
            Form.cbEventType.SelectedIndexChanged += OnActivityTypeChange;
            Form.btnSave.Click += OnSaveClick;
            Form.nHour.KeyDown += OnSaveKeyDown;
            Form.nMinute.KeyDown += OnSaveKeyDown;
            Form.tbMessage.KeyDown += OnSaveKeyDown;
            Form.KeyDown += OnSaveKeyDown;
            Form.cbEventType.KeyDown += OnSaveKeyDown;
            Form.calWorkDate.KeyDown += OnSaveKeyDown;
            Form.nHour.ValueChanged += OnHourEdit;
            Form.nMinute.ValueChanged += OnMinuteEdit;
            Form.tbMessage.TextChanged += OnMessageEdit;
            Form.calWorkDate.DateChanged += OnDateEdit;
            LoadEvent.Fire(this);
        }

        private void OnDateEdit(object sender, DateRangeEventArgs e)
        {
            model.EditItem.Date = e.Start;
            model.Sync.Value(SyncTarget.Source, "EditItem");
        }

        private void OnMessageEdit(object sender, EventArgs e)
        {
            model.EditItem.Comment = Form.tbMessage.Text;
            model.Sync.Value(SyncTarget.Source, "EditItem");
        }

        private void OnMinuteEdit(object sender, EventArgs e)
        {
            model.Time = new TimeSpan(model.Time.Hours, Convert.ToInt32(Form.nMinute.Value), 0);
            model.Sync.Value(SyncTarget.Source, "Time");
        }

        private void OnHourEdit(object sender, EventArgs e)
        {
            model.Time = new TimeSpan(Convert.ToInt32(Form.nHour.Value), model.Time.Minutes, 0);
            model.Sync.Value(SyncTarget.Source, "Time");
        }

        private void OnSaveKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveEvent.Fire(this);
            }
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            SaveEvent.Fire(this);
        }

        private void OnActivityTypeChange(object sender, EventArgs e)
        {
            if (model.WorkActivities.Count > Form.cbEventType.SelectedIndex
                && Form.cbEventType.SelectedIndex >= 0)
            {
                model.Activity = model.WorkActivities[Form.cbEventType.SelectedIndex];
                model.EditItem.IdActivity = model.Activity.Id;
                model.Sync.Value(SyncTarget.Source, "Activity");
            }
        }


        private void OnWorkActivitiesChange()
        {
            Form.cbEventType.DataSource = null;
            Form.cbEventType.Items.Clear();
            Form.cbEventType.DataSource = model.WorkActivities;
            Form.cbEventType.DisplayMember = "Name";
            Form.cbEventType.ValueMember = "Id";

            if (Form.cbEventType.Items.Count > 0)
            {
                Form.cbEventType.SelectedItem = Form.cbEventType.Items[0];
                model.Activity = model.WorkActivities[0];
            }
        }

        private void OnEditItemChange()
        {
            Form.Text = "(" + model.EditItem.IdIssue.ToString() + ") " + model.EditItem.ProjectName;
            Form.calWorkDate.SetDate(model.EditItem.Date);
            Form.calWorkDate.AddMonthlyBoldedDate(model.EditItem.Date);
            Form.tbMessage.Text = model.EditItem.Comment;
        }
        private void OnTimeChange()
        {
            Form.nHour.Value = model.Time.Hours;
            Form.nMinute.Value = model.Time.Minutes;
        }

    }
}