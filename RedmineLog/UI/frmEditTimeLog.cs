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
    public partial class frmEditTimeLog : Form, ISetup
    {
        public Action OnChange;
        private IAppSettings settings;

        public frmEditTimeLog()
        {
            InitializeComponent();
            this.Initialize<EditLog.IView, frmEditTimeLog>();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            this.SetupLocation(settings.Display, 0, -160);
        }

        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
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
        }
        public void Load()
        {
            new frmProcessing().Show(Form,
                          () =>
                          {
                              OnWorkActivitiesChange();
                              OnEditItemChange();
                              OnTimeChange();
                          });
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
                new frmProcessing().Show(Form,
                    () =>
                    {
                        SaveEvent.Fire(this);
                    });
            }
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    SaveEvent.Fire(this);
                }, () =>
                {
                    Form.Close();
                });
        }

        private void OnActivityTypeChange(object sender, EventArgs e)
        {
            if (model.WorkActivities.Count > Form.cbEventType.SelectedIndex && Form.cbEventType.SelectedIndex >= 0)
            {
                model.Activity = model.WorkActivities[Form.cbEventType.SelectedIndex];
                model.EditItem.IdActivity = model.Activity.Id;
                model.Sync.Value(SyncTarget.Source, "Activity");
            }
        }


        private void OnWorkActivitiesChange()
        {
            Form.cbEventType.Set(model,
               (ui, data) =>
               {
                   ui.DataSource = null;
                   ui.Items.Clear();
                   ui.DataSource = data.WorkActivities;
                   ui.DisplayMember = "Name";
                   ui.ValueMember = "Id";

                   if (ui.Items.Count > 0)
                   {
                       ui.SelectedItem = ui.Items[0];
                       data.Activity = data.WorkActivities[0];
                   }
               });
        }

        private void OnEditItemChange()
        {
            Form.Set(model,
              (ui, data) =>
              {
                  ui.Text = "(" + data.EditItem.IdIssue.ToString() + ") " + data.EditItem.ProjectName;
                  ui.calWorkDate.SetDate(data.EditItem.Date);
                  ui.calWorkDate.AddMonthlyBoldedDate(data.EditItem.Date);
                  ui.tbMessage.Text = data.EditItem.Comment;
              });
        }
        private void OnTimeChange()
        {
            Form.Set(model,
              (ui, data) =>
              {
                  ui.nHour.Value = data.Time.Hours;
                  ui.nMinute.Value = data.Time.Minutes;
              });
        }

    }
}