using RedmineLog.Common.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedmineLog.UI.Common;
using Appccelerate.EventBroker;
using RedmineLog.Common;
using Ninject;
using Appccelerate.Events;
using System.Reactive.Linq;
using System.Reactive;

namespace RedmineLog.UI
{
    public partial class frmSubIssue : Form, ISetup
    {
        private IAppSettings settings;
        public frmSubIssue()
        {
            InitializeComponent();
            this.Initialize<SubIssue.IView, frmSubIssue>();
            Load += OnAddIssueLoad;
        }

        private void OnAddIssueLoad(object sender, EventArgs e)
        {
            this.SetupLocation(settings.Display, 0, -150);
        }

        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
        }
    }

    internal class SubIssueView : SubIssue.IView, IView<frmSubIssue>
    {
        private SubIssue.IModel model;

        private frmSubIssue Form;

        [EventPublication(SubIssue.Events.Load, typeof(Publish<SubIssue.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(Main.Events.AddSubIssue)]
        public event EventHandler<Args<SubIssueData>> AddSubIssueEvent;


        [Inject]
        public SubIssueView(SubIssue.IModel inModel)
        {
            model = inModel;
            model.Priorities.OnUpdate.Subscribe(OnUpdatePriorities);
            model.Trackers.OnUpdate.Subscribe(OnUpdateTrackers);
            model.Users.OnUpdate.Subscribe(OnUpdateUsers);
        }

        private void OnUpdateTrackers(TrackerDataList obj)
        {
            Form.cbTracker.Set(obj,
                  (ui, data) =>
                  {
                      ui.DataSource = null;
                      ui.Items.Clear();
                      ui.DataSource = data;
                      ui.DisplayMember = "Name";
                      ui.ValueMember = "Id";

                      model.SubIssueData.Value.Tracker = data.Where(x => x.IsDefault).FirstOrDefault();

                      if (model.SubIssueData.Value.Tracker != null)
                      {
                          ui.SelectedItem = ui.Items[data.IndexOf(model.SubIssueData.Value.Tracker)];
                      }
                      else if (ui.Items.Count > 0)
                      {
                          ui.SelectedItem = ui.Items[0];
                          model.SubIssueData.Value.Tracker = data[0];
                      }
                  });
        }

        private void OnUpdateUsers(UserDataList obj)
        {
            Form.cbPerson.Set(obj,
              (ui, data) =>
              {
                  ui.DataSource = null;
                  ui.Items.Clear();
                  ui.DataSource = data;
                  ui.DisplayMember = "Name";
                  ui.ValueMember = "Id";


                  model.SubIssueData.Value.User = data.Where(x => x.IsDefault).FirstOrDefault();

                  if (model.SubIssueData.Value.User != null)
                  {
                      ui.SelectedItem = ui.Items[data.IndexOf(model.SubIssueData.Value.User)];
                  }
                  else if (ui.Items.Count > 0)
                  {
                      ui.SelectedItem = ui.Items[0];
                      model.SubIssueData.Value.User = data[0];
                  }
              });
        }

        private void OnUpdatePriorities(PriorityDataList obj)
        {
            Form.cbPriority.Set(obj,
                  (ui, data) =>
                  {
                      ui.DataSource = null;
                      ui.Items.Clear();
                      ui.DataSource = data;
                      ui.DisplayMember = "Name";
                      ui.ValueMember = "Id";

                      model.SubIssueData.Value.Priority = data.Where(x => x.IsDefault).FirstOrDefault();

                      if (model.SubIssueData.Value.Priority != null)
                      {
                          ui.SelectedItem = ui.Items[data.IndexOf(model.SubIssueData.Value.Priority)];
                      }
                      else if (ui.Items.Count > 0)
                      {
                          ui.SelectedItem = ui.Items[0];
                          model.SubIssueData.Value.Priority = data[0];
                      }
                  });
        }

        EventProperty<EventArgs> subjectChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> descriptionChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> personChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> priorityChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> trackerChanged = new EventProperty<EventArgs>();

        public void Init(frmSubIssue inView)
        {
            Form = inView;
            subjectChanged.Build(Observable.FromEventPattern<EventArgs>(Form.tbSubject, "TextChanged"));
            descriptionChanged.Build(Observable.FromEventPattern<EventArgs>(Form.tbDescription, "TextChanged"));
            personChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbPerson, "SelectedIndexChanged"));
            priorityChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbPriority, "SelectedIndexChanged"));
            trackerChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbTracker, "SelectedIndexChanged"));

            subjectChanged.Subscribe(OnNotifySubject);
            descriptionChanged.Subscribe(OnNotifyDescription);
            trackerChanged.Subscribe(OnNotifyTracker);
            priorityChanged.Subscribe(OnNotifyPriority);
            personChanged.Subscribe(OnNotifyPerson);

            Form.btnAccept.Click += OnAcceptClick;
            Load();
        }

        private void OnNotifyPerson(EventPattern<EventArgs> obj)
        {
            if (model.Users.Value.Count > Form.cbPerson.SelectedIndex && Form.cbPerson.SelectedIndex >= 0)
            {
                model.SubIssueData.Value.User = model.Users.Value[Form.cbPerson.SelectedIndex];
                model.SubIssueData.Update();
            }
        }

        private void OnNotifyPriority(EventPattern<EventArgs> obj)
        {
            if (model.Priorities.Value.Count > Form.cbPriority.SelectedIndex && Form.cbPriority.SelectedIndex >= 0)
            {
                model.SubIssueData.Value.Priority = model.Priorities.Value[Form.cbPriority.SelectedIndex];
                model.SubIssueData.Update();
            }
        }

        private void OnNotifyTracker(EventPattern<EventArgs> obj)
        {
            if (model.Trackers.Value.Count > Form.cbTracker.SelectedIndex && Form.cbTracker.SelectedIndex >= 0)
            {
                model.SubIssueData.Value.Tracker = model.Trackers.Value[Form.cbTracker.SelectedIndex];
                model.SubIssueData.Update();
            }
        }

        private void OnNotifyDescription(EventPattern<EventArgs> obj)
        {
            model.SubIssueData.Value.Description = Form.tbDescription.Text;
            model.SubIssueData.Update();
        }

        private void OnNotifySubject(EventPattern<EventArgs> obj)
        {
            model.SubIssueData.Value.Subject = Form.tbSubject.Text;
            model.SubIssueData.Update();
        }


        private void OnAcceptClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    AddSubIssueEvent.Fire(this, model.SubIssueData.Value);
                }, () =>
                {

                    NotifyBox.Show("Sub issue added", "Info");
                    this.Form.Close();
                });

        }

        public void Load()
        {
            new frmProcessing().Show(Form,
                  () =>
                  {
                      LoadEvent.Fire(this);
                  });
        }
    }
}
