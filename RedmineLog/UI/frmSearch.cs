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
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI
{
    public partial class frmSearch : Form, ISetup
    {
        private IAppSettings settings;

        public frmSearch()
        {
            InitializeComponent();
            this.Initialize<Search.IView, frmSearch>();
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

    internal class SearchView : Search.IView, IView<frmSearch>
    {

        private Search.IModel model;

        private frmSearch Form;

        [Inject]
        public SearchView(Search.IModel inModel)
        {
            model = inModel;
            model.Issues.OnUpdate.Subscribe(OnUpdateIssues);
            model.Project.OnUpdate.Subscribe(OnUpdateProject);
            model.Projects.OnUpdate.Subscribe(OnUpdateProjects);
        }


        [EventPublication(Search.Events.Load)]
        public event EventHandler LoadEvent;

        [EventPublication(Search.Events.Clear)]
        public event EventHandler ClearEvent;

        [EventPublication(IssueLog.Events.Select)]
        public event EventHandler<Args<WorkingIssue>> SelectEvent;

        [EventPublication(Search.Events.Search)]
        public event EventHandler<Args<String>> SearchEvent;

        EventProperty<EventArgs> projectChanged = new EventProperty<EventArgs>();

        public void Init(frmSearch inView)
        {
            Form = inView;
            KeyHelpers.BindKey(Form, OnKeyDown);
            KeyHelpers.BindKey(Form.fpIssueItemList, OnKeyDown);
            Observable.FromEventPattern<EventArgs>(Form.btnSearch, "Click").Subscribe(OnSearchClick);
            Observable.FromEventPattern<EventArgs>(Form.btnClear, "Click").Subscribe(OnClearClick);
            projectChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbProjects, "SelectedIndexChanged"));
            Load();
        }

        private void OnNotifyProject(EventPattern<EventArgs> obj)
        {
            if (model.Projects.Value.Count > Form.cbProjects.SelectedIndex)
            {
                model.Project.Notify(model.Projects.Value[Form.cbProjects.SelectedIndex]);
            };
        }

        private void OnUpdateProject(ProjectData obj)
        {
            Form.cbProjects.Set(obj,
               (ui, data) =>
               {
                   projectChanged.Dispose();
                   ui.SelectedItem = ui.Items[model.Projects.Value.IndexOf(obj)];
                   projectChanged.Subscribe(Observer.Create<EventPattern<EventArgs>>(OnNotifyProject));
               });
        }

        private void OnUpdateProjects(ProjectList value)
        {
            Form.cbProjects.Set(model,
               (ui, data) =>
               {
                   ui.BeginUpdate();

                   ui.Items.Clear();
                   ui.DataSource = data.Projects.Value;
                   ui.DisplayMember = "Name";
                   ui.ValueMember = "Id";

                   if (ui.Items.Count > 0)
                   {
                       projectChanged.Dispose();
                       ui.SelectedItem = ui.Items[0];
                       projectChanged.Subscribe(Observer.Create<EventPattern<EventArgs>>(OnNotifyProject));
                       data.Project.Notify(data.Projects.Value[0]);
                   }
                   ui.EndUpdate();
               });
        }


        private void OnClearClick(EventPattern<EventArgs> obj)
        {
            ClearEvent.Fire(this);
        }

        private void OnSearchClick(EventPattern<EventArgs> obj)
        {
            PerformSearch(Form.tbSearchText.Text);
        }

        private void PerformSearch(string inSearchText)
        {
            Form.lblInfo.Visible = false;
            Form.lblInfo.Height = 0;

            new frmProcessing().Show(Form,
             () =>
             {
                 SearchEvent.Fire(this, inSearchText);
             }, () =>
             {
                 if (Form.fpIssueItemList.Controls.Count == 0)
                 {
                     Form.lblInfo.Visible = true;
                     Form.lblInfo.Height = 23;
                 }
                 Form.tbSearchText.Focus();
             });
        }

        public void Load()
        {
            new frmProcessing().Show(Form,
                () =>
                {
                    LoadEvent.Fire(this);
                }, () =>
                {
                    Form.tbSearchText.SelectAll();
                    Form.tbSearchText.Focus();
                });
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(" key " + e.KeyCode.ToString());

            if (e.KeyCode == Keys.Escape)
            {
                Form.Close();
                return;
            }

            if (e.KeyCode == Keys.Tab || (e.KeyCode == (Keys.LButton | Keys.Back)) || e.KeyCode == Keys.Oemtilde)
            {
                Form.tbSearchText.SelectAll();
                Form.tbSearchText.Focus();
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch(Form.tbSearchText.Text);
                return;
            }
        }

        private void OnUpdateIssues(WorkingIssueList obj)
        {

            var list = new List<Control>();


            foreach (var item in (from p in obj
                                  group p by p.Issue.Project into g
                                  select new { Project = g.Key, Issues = g.ToList() }))
            {
                list.Add(new SearchGroupItemView().Set(item.Project));
                KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);

                foreach (var issue in item.Issues)
                {
                    list.Add(Program.Kernel.Get<SearchItemView>().Set(issue));
                    KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);
                    KeyHelpers.BindMouseClick(list[list.Count - 1], OnClick);
                    // KeyHelpers.BindSpecialClick(list[list.Count - 1], OnSpecialClick);
                }
            }

            Form.fpIssueItemList.Set(obj,
            (ui, data) =>
            {
                ui.Controls.Clear();
                Form.fpIssueItemList.Controls.AddRange(list.ToArray());

                if (list.Count > 0)
                    Form.tbSearchText.Text = string.Empty;

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
