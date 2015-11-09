using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using RedmineLog.UI.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmIssueLog : Form, ISetup
    {
        private IAppSettings settings;
        public frmIssueLog()
        {
            InitializeComponent();
            this.Initialize<IssueLog.IView, frmIssueLog>();

            cHeader.SetDescription();
        }

        private void OnSearchLoad(object sender, EventArgs e)
        {
            this.SetupLocation(settings.Display, 0, -50);
        }

        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
        }
    }

    internal class IssueLogView : IssueLog.IView, IView<frmIssueLog>
    {
        private frmIssueLog Form;
        private IssueLog.IModel model;
        [Inject]
        public IssueLogView(IssueLog.IModel inModel)
        {
            model = inModel;
            model.Issues.OnUpdate.Subscribe(OnUpdateIssues);
        }

        private void OnUpdateIssues(WorkingIssueList obj)
        {
            var list = new List<Control>();

            foreach (var item in (from p in obj
                                  group p by p.Issue.Project into g
                                  orderby (g.FirstOrDefault().Issue.IsGlobal()
                                           ? Int32.MaxValue
                                           : g.FirstOrDefault().Data.UsedCount) descending
                                  select new { Project = g.Key, Issues = g.ToList() }))
            {

                if (!String.IsNullOrWhiteSpace(item.Project))
                {
                    list.Add(new IssueLogGroupItemView().Set(item.Project));
                    KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);
                }

                foreach (var issueParent in (from i in item.Issues
                                             group i by (i.Parent == null ? "" : i.Parent.Subject) into g
                                             orderby g.FirstOrDefault().Data.UsedCount descending
                                             select new { Parent = g.Key, Issues = g.ToList() }))
                {
                    if (!String.IsNullOrWhiteSpace(issueParent.Parent))
                    {
                        list.Add(new IssueLogGroupItemView().Set(issueParent.Parent));
                        KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);
                    }

                    foreach (var issue in issueParent.Issues.OrderByDescending(x => x.Data.UsedCount))
                    {
                        list.Add(new IssueLogItemView().Set(issue));
                        KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);
                        KeyHelpers.BindMouseClick(list[list.Count - 1], OnClick);
                        KeyHelpers.BindSpecialClick(list[list.Count - 1], OnSpecialClick);
                    }

                }

            }

            Form.fpLogItemList.Set(obj,
              (ui, data) =>
              {
                  ui.Controls.Clear();
                  Form.fpLogItemList.Controls.AddRange(list.ToArray());
              });
        }

        [EventPublication(IssueLog.Events.Load)]
        public event EventHandler LoadEvent;

        [EventPublication(IssueLog.Events.Select)]
        public event EventHandler<Args<WorkingIssue>> SelectEvent;

        [EventPublication(Main.Events.IssueResolve)]
        public event EventHandler<Args<WorkingIssue>> ResolveEvent;

        [EventPublication(IssueLog.Events.Delete)]
        public event EventHandler<Args<WorkingIssue>> DeleteEvent;

        [EventPublication(SubIssue.Events.SetSubIssue)]
        public event EventHandler<Args<int>> SetSubIssueEvent;

        private frmSubIssue addIssueForm;


        public void Init(frmIssueLog inView)
        {
            Form = inView;
            KeyHelpers.BindKey(Form, OnKeyDown);
            Load();
        }

        public void Load()
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    LoadEvent.Fire(this);
                });
        }

        private void OnClick(object sender)
        {
            if (sender is ICustomItem)
            {
                SelectIssue(((ICustomItem)sender).Data as WorkingIssue);
                return;
            }

            if (sender is Control)
            {
                OnClick(((Control)sender).Parent);
                return;
            }
        }

        private void OnSpecialClick(string action, object data)
        {
            if (action == "Select" && data is ICustomItem)
            {
                SelectIssue(((ICustomItem)data).Data as WorkingIssue);
                return;
            }

            if (action == "Resolve" && data is Control && data is ICustomItem)
            {
                Form.fpLogItemList.Controls.Remove((Control)data);
                ResolveEvent.Fire(this, ((ICustomItem)data).Data as WorkingIssue);
                return;
            }

            if (action == "Delete" && data is Control && data is ICustomItem)
            {
                Form.fpLogItemList.Controls.Remove((Control)data);
                DeleteEvent.Fire(this, ((ICustomItem)data).Data as WorkingIssue);
                return;
            }

            if (action == "AddSubIssue" && data is ICustomItem)
            {
                addIssueForm = new frmSubIssue();
                SetSubIssueEvent.Fire(this, (((ICustomItem)data).Data as WorkingIssue).Issue.Id);
                addIssueForm.ShowDialog();
                return;
            }

        }



        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Form.Close();
                return;
            }
        }

        private void SelectIssue(WorkingIssue item)
        {
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
    }
}