using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.UI.Common;
using RedmineLog.UI.Items;
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
    public partial class frmBugLog : Form, ISetup
    {
        private IAppSettings settings;
        public frmBugLog()
        {
            InitializeComponent();
            this.Initialize<BugLog.IView, frmBugLog>();
            cHeader.SetDescription();

        }
        private void OnBugLogLoad(object sender, EventArgs e)
        {
            this.SetupLocation(settings.Display, 0, -50);
        }

        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
        }
    }

    internal class BugLogView : BugLog.IView, IView<frmBugLog>
    {
        private BugLog.IModel model;

        private frmBugLog Form;

        [Inject]
        public BugLogView(BugLog.IModel inModel)
        {
            model = inModel;
            model.Bugs.OnUpdate.Subscribe(OnUpdateBugs);
        }

        private void OnUpdateBugs(BugLogList obj)
        {
            var list = new List<Control>();

            foreach (var item in (from p in obj
                                  group p by p.Project into g
                                  select new { Project = g.Key, Issues = g.ToList() }))
            {
                list.Add(new BugLogGroupItemView().Set(item.Project));
                KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);

                foreach (var issue in item.Issues)
                {
                    list.Add(new BugLogItemView().Set(issue));
                    KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);
                    KeyHelpers.BindMouseClick(list[list.Count - 1], OnClick);
                    KeyHelpers.BindSpecialClick(list[list.Count - 1], OnSpecialClick);
                }
            }


            Form.fpBugList.Set(obj,
              (ui, data) =>
              {
                  ui.Controls.Clear();
                  Form.fpBugList.Controls.AddRange(list.ToArray());
              });
        }


        [EventPublication(BugLog.Events.Load)]
        public event EventHandler LoadEvent;

        [EventPublication(BugLog.Events.Select)]
        public event EventHandler<Args<BugLogItem>> SelectEvent;

        [EventPublication(BugLog.Events.Resolve)]
        public event EventHandler<Args<BugLogItem>> ResolveEvent;

        [EventPublication(SubIssue.Events.SetSubIssue)]
        public event EventHandler<Args<int>> SetSubIssueEvent;


        private frmSubIssue addIssueForm;

        public void Init(frmBugLog inView)
        {
            Form = inView;
            KeyHelpers.BindKey(Form, OnKeyDown);
            Load();
        }

        private void OnSpecialClick(string action, object data)
        {
            if (action == "Select" && data is ICustomItem)
            {
                SelectBug(((ICustomItem)data).Data as BugLogItem);
                SelectEvent.Fire(this, ((ICustomItem)data).Data as BugLogItem);
                return;
            }

            if (action == "Resolve" && data is Control && data is ICustomItem)
            {
                Form.fpBugList.Controls.Remove((Control)data);
                ResolveEvent.Fire(this, ((ICustomItem)data).Data as BugLogItem);
                return;
            }

            if (action == "AddSubIssue" && data is ICustomItem)
            {
                addIssueForm = new frmSubIssue();
                SetSubIssueEvent.Fire(this, (((ICustomItem)data).Data as BugLogItem).Id);
                addIssueForm.ShowDialog();
                return;
            }
        }

        private void OnClick(object sender)
        {
            if (sender is ICustomItem)
            {
                SelectBug(((ICustomItem)sender).Data as BugLogItem);
                return;
            }

            if (sender is Control)
            {
                OnClick(((Control)sender).Parent);
                return;
            }
        }

        private void SelectBug(BugLogItem item)
        {
            if (item.IsGlobal())
                return;

            new frmProcessing().Show(Form,
               () =>
               {
                   if (item != null)
                       SelectEvent.Fire(this, item);
               }, () =>
               {
                   Form.Close();
               });
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Form.Close();

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
