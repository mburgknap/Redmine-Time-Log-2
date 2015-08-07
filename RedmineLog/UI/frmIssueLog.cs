using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
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
    public partial class frmIssueLog : Form
    {
        public frmIssueLog()
        {
            InitializeComponent();
            this.Initialize<IssueLog.IView, frmIssueLog>();

            cHeader.SetDescription();
            fpLogItemList.AutoScroll = true;
        }

        private void OnSearchLoad(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }
    }

    internal class IssueLogView : IssueLog.IView, IView<frmIssueLog>
    {
        private frmIssueLog Form;
        private IssueLog.IModel model;
        [Inject]
        public IssueLogView(IssueLog.IModel inModel, IEventBroker inGlobalEvent)
        {
            model = inModel;
            model.Sync.Bind(SyncTarget.View, this);
            inGlobalEvent.Register(this);
        }

        [EventPublication(IssueLog.Events.Load, typeof(Publish<IssueLog.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(IssueLog.Events.Select)]
        public event EventHandler<Args<WorkingIssue>> SelectEvent;

        [EventPublication(IssueLog.Events.Resolve)]
        public event EventHandler<Args<WorkingIssue>> ResolveEvent;

        [EventPublication(IssueLog.Events.Delete)]
        public event EventHandler<Args<WorkingIssue>> DeleteEvent;

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
        }

        private void OnIssuesChange()
        {
            var list = new List<Control>();

            foreach (var item in (from p in model.Issues
                                  group p by p.Issue.Project into g
                                  select new { Project = g.Key, Issues = g.ToList() }))
            {
                list.Add(new IssueLogGroupItem().Set(item.Project));
                KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);

                foreach (var issue in item.Issues)
                {
                    list.Add(new IssueLogItem().Set(issue));
                    KeyHelpers.BindKey(list[list.Count - 1], OnKeyDown);
                    KeyHelpers.BindMouseClick(list[list.Count - 1], OnClick);
                    KeyHelpers.BindSpecialClick(list[list.Count - 1], OnSpecialClick);
                }

            }

            Form.fpLogItemList.Set(model,
              (ui, data) =>
              {
                  ui.Controls.Clear();

                  Form.fpLogItemList.Controls.AddRange(list.ToArray());
              });
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