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
        public SubIssueView(SubIssue.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
            inGlobalEvent.Register(this);
        }

        public void Init(frmSubIssue inView)
        {
            Form = inView;
            Form.tbSubject.TextChanged += OnSubjectTextChanged;
            Form.tbDescription.TextChanged += OnDescriptionTextChanged;
            Form.cbPerson.SelectedIndexChanged += OnPersonEdit;
            Form.cbPriority.SelectedIndexChanged += OnPriorityEdit;
            Form.cbTracker.SelectedIndexChanged += OnTrackerEdit;
            Form.btnAccept.Click += OnAcceptClick;
            Load();
        }

        private void OnDescriptionTextChanged(object sender, EventArgs e)
        {
            model.Description = Form.tbDescription.Text;
            model.Sync.Value(SyncTarget.Source, "Description");
        }

        private void OnSubjectTextChanged(object sender, EventArgs e)
        {
            model.Subject = Form.tbSubject.Text;
            model.Sync.Value(SyncTarget.Source, "Subject");
        }

        private void OnTrackerEdit(object sender, EventArgs e)
        {
            if (model.Trackers.Count > Form.cbTracker.SelectedIndex && Form.cbTracker.SelectedIndex >= 0)
            {
                model.Tracker = model.Trackers[Form.cbTracker.SelectedIndex];
                model.Sync.Value(SyncTarget.Source, "Tracker");
            }
        }

        private void OnPriorityEdit(object sender, EventArgs e)
        {
            if (model.Priorities.Count > Form.cbPriority.SelectedIndex && Form.cbPriority.SelectedIndex >= 0)
            {
                model.Priority = model.Priorities[Form.cbPriority.SelectedIndex];
                model.Sync.Value(SyncTarget.Source, "Priority");
            }
        }

        private void OnPersonEdit(object sender, EventArgs e)
        {
            if (model.Users.Count > Form.cbPerson.SelectedIndex && Form.cbPerson.SelectedIndex >= 0)
            {
                model.User = model.Users[Form.cbPerson.SelectedIndex];
                model.Sync.Value(SyncTarget.Source, "User");
            }
        }

        private void OnTrackersChange()
        {
            Form.cbTracker.Set(model,
               (ui, data) =>
               {
                   ui.DataSource = null;
                   ui.Items.Clear();
                   ui.DataSource = data.Trackers;
                   ui.DisplayMember = "Name";
                   ui.ValueMember = "Id";

                   data.Tracker = data.Trackers.Where(x => x.IsDefault).FirstOrDefault();

                   if (data.Tracker != null)
                   {
                       ui.SelectedItem = ui.Items[data.Trackers.IndexOf(data.Tracker)];
                   }
                   else if (ui.Items.Count > 0)
                   {
                       ui.SelectedItem = ui.Items[0];
                       data.Tracker = data.Trackers[0];
                   }
               });
        }

        private void OnPrioritiesChange()
        {
            Form.cbPriority.Set(model,
               (ui, data) =>
               {
                   ui.DataSource = null;
                   ui.Items.Clear();
                   ui.DataSource = data.Priorities;
                   ui.DisplayMember = "Name";
                   ui.ValueMember = "Id";

                   data.Priority = data.Priorities.Where(x => x.IsDefault).FirstOrDefault();

                   if (data.Priority != null)
                   {
                       ui.SelectedItem = ui.Items[data.Priorities.IndexOf(data.Priority)];
                   }
                   else if (ui.Items.Count > 0)
                   {
                       ui.SelectedItem = ui.Items[0];
                       data.Priority = data.Priorities[0];
                   }
               });
        }

        private void OnUsersChange()
        {
            Form.cbPerson.Set(model,
               (ui, data) =>
               {
                   ui.DataSource = null;
                   ui.Items.Clear();
                   ui.DataSource = data.Users;
                   ui.DisplayMember = "Name";
                   ui.ValueMember = "Id";


                   data.User = data.Users.Where(x => x.IsDefault).FirstOrDefault();

                   if (data.User != null)
                   {
                       ui.SelectedItem = ui.Items[data.Users.IndexOf(data.User)];
                   }
                   else if (ui.Items.Count > 0)
                   {
                       ui.SelectedItem = ui.Items[0];
                       data.User = data.Users[0];
                   }
               });
        }

        private void OnAcceptClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    AddSubIssueEvent.Fire(this, model.ToSubIssueData());
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
