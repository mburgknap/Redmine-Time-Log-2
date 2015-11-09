using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using RedmineLog.Logic;
using RedmineLog.UI.Common;
using System;
using System.Drawing;
using System.Reactive;
using System.Reactive.Linq;
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
        public EditTimeLogView(EditLog.IModel inModel)
        {
            model = inModel;
            model.Activity.OnUpdate.Subscribe(OnUpdateActivity);
            model.EditItem.OnUpdate.Subscribe(OnUpdateEditItem);
            model.Time.OnUpdate.Subscribe(OnUpdateTime);
            model.WorkActivities.OnUpdate.Subscribe(OnUpdateWorkActivities);
        }

        private void OnUpdateWorkActivities(WorkActivityList obj)
        {
            Form.cbActivityType.Set(obj,
               (ui, data) =>
               {
                   ui.BeginUpdate();
                   ui.DataSource = null;
                   ui.Items.Clear();
                   ui.DataSource = data;
                   ui.DisplayMember = "Name";
                   ui.ValueMember = "Id";

                   if (ui.Items.Count > 0)
                   {
                       var tmp = model.Activity.Value != null ? model.Activity.Value : data[0];

                       OnUpdateActivity(tmp);
                       model.Activity.Notify(tmp);
                   }
                   ui.EndUpdate();
               });
        }

        private void OnNotifyActivity(EventPattern<EventArgs> obj)
        {
            if (model.WorkActivities.Value.Count > Form.cbActivityType.SelectedIndex
                && Form.cbActivityType.SelectedIndex >= 0)
            {
                model.Activity.Notify(model.WorkActivities.Value[Form.cbActivityType.SelectedIndex]);
                model.EditItem.Value.IdActivity = model.Activity.Value.Id;
            };
        }

        private void OnUpdateTime(TimeSpan obj)
        {
            Form.nHour.Value = obj.Hours;
            Form.nMinute.Value = obj.Minutes;
        }

        private void OnUpdateEditItem(WorkLogItem obj)
        {
            Form.Set(obj,
                (ui, data) =>
                {
                    ui.Text = "(" + data.IdIssue.ToString() + ") " + data.ProjectName;
                    ui.calWorkDate.SetDate(data.Date);
                    ui.calWorkDate.AddMonthlyBoldedDate(data.Date);
                    ui.tbMessage.Text = data.Comment;
                });
        }

        private void OnUpdateActivity(WorkActivityType obj)
        {
            activityTypeChanged.Dispose();
            Form.cbActivityType.SelectedIndex = obj != null ? model.WorkActivities.Value.IndexOf(obj) : 0;
            activityTypeChanged.Subscribe(Observer.Create<EventPattern<EventArgs>>(OnNotifyActivity));
        }

        EventProperty<EventArgs> activityTypeChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> hourChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> minuteChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> messageChanged = new EventProperty<EventArgs>();
        EventProperty<DateRangeEventArgs> workDateChanged = new EventProperty<DateRangeEventArgs>();

        [EventPublication(EditLog.Events.Save, typeof(OnPublisher))]
        public event EventHandler SaveEvent;

        [EventPublication(EditLog.Events.Load, typeof(OnPublisher))]
        public event EventHandler LoadEvent;

        public void Init(frmEditTimeLog inView)
        {
            Form = inView;

            activityTypeChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbActivityType, "SelectedIndexChanged"));
            hourChanged.Build(Observable.FromEventPattern<EventArgs>(Form.nHour, "ValueChanged"));
            minuteChanged.Build(Observable.FromEventPattern<EventArgs>(Form.nMinute, "ValueChanged"));
            messageChanged.Build(Observable.FromEventPattern<EventArgs>(Form.tbMessage, "TextChanged"));
            workDateChanged.Build(Observable.FromEventPattern<DateRangeEventArgs>(Form.calWorkDate, "DateChanged"));
            hourChanged.Subscribe(OnNotifyHour);
            minuteChanged.Subscribe(OnNotifyTimeMinute);
            workDateChanged.Subscribe(OnNotifyEditItemDate);
            messageChanged.Subscribe(OnNotifyComment);

            Form.btnSave.Click += OnSaveClick;
            Form.nHour.KeyDown += OnSaveKeyDown;
            Form.nMinute.KeyDown += OnSaveKeyDown;
            Form.tbMessage.KeyDown += OnSaveKeyDown;
            Form.KeyDown += OnSaveKeyDown;
            Form.cbActivityType.KeyDown += OnSaveKeyDown;
            Form.calWorkDate.KeyDown += OnSaveKeyDown;
            Load();
        }

        private void OnNotifyComment(EventPattern<EventArgs> obj)
        {
            model.EditItem.Value.Comment = Form.tbMessage.Text;
            model.EditItem.Notify();
        }

        private void OnNotifyEditItemDate(EventPattern<DateRangeEventArgs> obj)
        {
            model.EditItem.Value.Date = obj.EventArgs.Start;
            model.EditItem.Notify();
        }

        private void OnNotifyTimeMinute(EventPattern<EventArgs> obj)
        {
            model.Time.Notify(new TimeSpan(model.Time.Value.Hours, Convert.ToInt32(Form.nMinute.Value), 0));
        }

        private void OnNotifyHour(EventPattern<EventArgs> obj)
        {
            model.Time.Notify(new TimeSpan(Convert.ToInt32(Form.nHour.Value), model.Time.Value.Minutes, 0));
        }
        public void Load()
        {
            new frmProcessing().Show(Form,
                          () =>
                          {
                              LoadEvent.Fire(this);
                          });
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
    }
}