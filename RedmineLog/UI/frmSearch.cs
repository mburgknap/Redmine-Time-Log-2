﻿using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic;
using RedmineLog.Logic.Data;
using RedmineLog.UI.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSearch : Form
    {
        public Action<int> OnSelect;
        private Point appLocation;

        public frmSearch()
        {
            InitializeComponent();
            this.Initialize<Search.IView, frmSearch>();
        }

        private void OnSearchDeactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnSearchLoad(object sender, EventArgs e)
        {
            this.Location = appLocation;

            RedmineIssues.Item main = null;
            RedmineIssues.Item parent = null;

            int row;

            foreach (var item in App.Context.History.OrderByDescending(x =>
            {
                if (x.Id == -1) return int.MaxValue;
                return x.UsedCount;
            }))
            {
                main = App.Context.IssuesCache.GetIssue(item.Id);

                if (main != null)
                {
                    if (main.IdParent.HasValue)
                        parent = App.Context.IssuesCache.GetIssue(main.IdParent.Value);
                    else
                        parent = null;

                    if (item.Id > 0)
                        row = dataGridView1.Rows.Add(new Object[] { item.Id, main.Project, String.Format("{0}{1}", GetSubject(parent), GetSubject(main)) });
                    else
                        row = dataGridView1.Rows.Add(new Object[] { "", "", "" });

                    if (App.Context.Work.IsStarted(item.Id))
                        dataGridView1.Rows[row].Cells[0].Style.BackColor = Color.Red;
                    else
                        dataGridView1.Rows[row].Cells[0].Style.BackColor = Color.White;

                    dataGridView1.Rows[row].Tag = item;
                }
            }
        }

        private string GetSubject(RedmineIssues.Item parent)
        {
            if (parent != null)
                return parent.Subject + Environment.NewLine + "   ";
            return "";
        }

        private void OnSearchMouseLeave(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
                SelectIssue(dataGridView1.Rows[e.RowIndex].Tag as LogData.Issue);
        }

        private void SelectIssue(LogData.Issue item)
        {
            if (item != null && OnSelect != null)
            {
                var data = App.Context.History.Where(x => x.Id == item.Id).First();
                data.UsedCount += 1;
                App.Context.History.Save();
                OnSelect(item.Id);
            }
            this.Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (dataGridView1.SelectedRows.Count == 0)
                return;

            if (e.KeyCode == Keys.Enter)
                SelectIssue(dataGridView1.SelectedRows[0].Tag as LogData.Issue);
        }

        internal void Init(Point point)
        {
            appLocation = point;
        }
    }

    internal class SearchView : Search.IView, IView<frmSearch>
    {
        private Search.IModel Model;

        private frmSearch Form;

        [Inject]
        public SearchView(Search.IModel inModel, IEventBroker inGlobalEvent)
        {
            Model = inModel;
            inGlobalEvent.Register(this);
        }

        [EventPublication(Search.Events.Load, typeof(Publish<Search.IView>))]
        public event EventHandler LoadEvent;

        public void Init(frmSearch inView)
        {
            Form = inView;
            LoadEvent.Fire(this);
        }
    }
}