﻿using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    internal class MainFormLogic : ILogic<Main.IView>
    {
        private Main.IView view;
        private Main.IModel model;
        private IRedmineClient redmine;
        private IDbIssue dbIssue;
        private IDbComment dbComment;
        private IDbRedmineIssue dbRedmineIssue;

        [Inject]
        public MainFormLogic(Main.IView inView, Main.IModel inModel, IEventBroker inEvents, IRedmineClient inClient, IDbIssue inDbIssue, IDbComment inDbComment, IDbRedmineIssue inDbRedmineIssue)
        {
            view = inView;
            model = inModel;
            model.Sync.Bind(SyncTarget.Source, this);
            redmine = inClient;
            dbIssue = inDbIssue;
            dbComment = inDbComment;
            dbRedmineIssue = inDbRedmineIssue;
            inEvents.Register(this);
        }

        [EventSubscription(Main.Events.Load, typeof(Subscribe<Main.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            model.WorkActivities.AddRange(redmine.GetWorkActivityTypes());
            model.Sync.Value(SyncTarget.View, "WorkActivities");

            dbIssue.Init();
            dbComment.Init();
            dbRedmineIssue.Init();

            LoadIssue(dbIssue.Get(0));

        }

        [EventSubscription(Main.Events.Link, typeof(Subscribe<Main.IView>))]
        public void OnLinkEvent(object sender, Args<String> arg)
        {
            if (arg.Data.Equals("Redmine"))
            { view.GoLink(redmine.IssueListUrl()); }
            else if (arg.Data.Equals("Issue"))
            { view.GoLink(redmine.IssueUrl(model.Issue)); }
        }

        [EventSubscription(Main.Events.Exit, typeof(Subscribe<Main.IView>))]
        public void OnExitEvent(object sender, EventArgs arg)
        {
            model.Issue.SetWorkTime(model.WorkTime);
            dbIssue.Update(model.Issue);
        }

        [EventSubscription(Main.Events.AddComment, typeof(Subscribe<Main.IView>))]
        public void OnAddCommentEvent(object sender, Args<string> arg)
        {
            var comment = new CommentData()
            {
                Id = Guid.NewGuid().ToString(),
                Text = arg.Data,
                IsGlobal = model.Issue.Id == 0
            };

            model.Comment = comment;
            dbComment.Update(comment);
            model.Issue.Comments.Add(comment.Id);
            dbIssue.Update(model.Issue);
            model.IssueComments.Add(comment);
            model.Sync.Value(SyncTarget.View, "Comment");
        }

        [EventSubscription(Main.Events.UpdateComment, typeof(Subscribe<Main.IView>))]
        public void OnUpdateCommentEvent(object sender, Args<string> arg)
        {
            if (model.Comment != null)
            {
                model.Comment.Text = arg.Data;
                model.Issue.IdComment = model.Comment.Id;
                dbIssue.Update(model.Issue);
                dbComment.Update(model.Comment);
            }
        }

        [EventSubscription(Main.Events.DelComment, typeof(Subscribe<Main.IView>))]
        public void OnDelCommentEvent(object sender, EventArgs arg)
        {
            if (model.Comment != null)
            {
                model.Issue.IdComment = null;
                model.Issue.Comments.Remove(model.Comment.Id);
                model.IssueComments.Remove(model.Comment);

                dbIssue.Update(model.Issue);
                dbComment.Delete(model.Comment);
                model.Comment = null;
                model.Sync.Value(SyncTarget.View, "Comment");
            }

        }

        [EventSubscription(Main.Events.AddIssue, typeof(Subscribe<Main.IView>))]
        public void OnAddIssueEvent(object sender, Args<string> arg)
        {
            int idIssue = 0;

            if (Int32.TryParse(arg.Data, out idIssue))
            {
                if (ReloadIssueData(idIssue)) return;
            }
            else
                LoadIssue(dbIssue.Get(0));
        }

        private bool ReloadIssueData(int idIssue)
        {
            DownloadIssue(idIssue);

            var tmpIssue = dbRedmineIssue.Get(idIssue);

            var issue = dbIssue.Get(idIssue);

            if (tmpIssue != null && issue == null)
                dbIssue.Update(issue = new IssueData() { Id = tmpIssue.Id, UsedCount = 1 });

            if (issue != null)
            {
                SetupLastIssue(issue);
                LoadIssue(issue);
                return true;
            }

            return false;
        }

        [EventSubscription(Main.Events.Reset, typeof(Subscribe<Main.IView>))]
        public void OnResetEvent(object sender, Args<Main.Actions> arg)
        {
            if (arg.Data == Main.Actions.Issue)
            {
                model.WorkTime = new TimeSpan(0);
                model.Sync.Value(SyncTarget.View, "WorkTime");
                model.Issue.SetWorkTime(model.WorkTime);
                dbIssue.Update(model.Issue);
                LoadIssue(dbIssue.Get(0));
            }
            else
            {
                model.IdleTime = new TimeSpan(0);
                model.Sync.Value(SyncTarget.View, "IdleTime");
            }
        }

        [EventSubscription(Main.Events.Submit, typeof(Subscribe<Main.IView>))]
        public void OnSubmitEvent(object sender, Args<Main.Actions> arg)
        {
            if (model.Comment == null)
            {
                view.Info("Write comment to the task");
                return;
            }

            if (!(model.Issue.Id > 0))
            {
                view.Info("Select issue to the task");
                return;
            }

            var workData = new WorkTimeData()
            {
                IdUssue = model.Issue.Id,
                IdActivityType = model.Activity.Id,
                Time = new TimeSpan(),
                Comment = model.Comment.Text
            };

            if (arg.Data == Main.Actions.Issue || arg.Data == Main.Actions.All)
            {
                workData.Time = workData.Time.Add(model.WorkTime);
                model.WorkTime = new TimeSpan(0);
                model.Sync.Value(SyncTarget.View, "WorkTime");
            }

            if (arg.Data == Main.Actions.Idle || arg.Data == Main.Actions.All)
            {
                workData.Time = workData.Time.Add(model.IdleTime);
                model.IdleTime = new TimeSpan(0);
                model.Sync.Value(SyncTarget.View, "IdleTime");
            }

            model.Issue.Time = null;

            if (!redmine.AddWorkTime(workData))
            { model.Issue.SetWorkTime(workData.Time); }

            dbIssue.Update(model.Issue);
            LoadIssue(dbIssue.Get(0));
        }

        [EventSubscription(Search.Events.Select, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<WorkingIssue> arg)
        {
            SetupLastIssue(arg.Data.Data);
            LoadIssue(arg.Data.Data);
        }

        [EventSubscription(WorkLog.Events.Select, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<WorkLogItem> arg)
        {
            var issue = dbIssue.Get(arg.Data.IdIssue);

            if (issue == null)
            {
                ReloadIssueData(arg.Data.IdIssue);
            }
            else
            {
                SetupLastIssue(issue);
                LoadIssue(issue);
            }
        }

        private void SetupLastIssue(IssueData issue)
        {
            if (model.Issue.Id > 0)
            {
                model.Issue.IdComment = model.Comment != null ? model.Comment.Id : null;
                model.Issue.SetWorkTime(model.WorkTime);
                dbIssue.Update(model.Issue);

                model.WorkTime = new TimeSpan(0);
                model.Sync.Value(SyncTarget.View, "WorkTime");
            }
            else
            {
                issue.AddWorkTime(model.WorkTime);

                if (model.Comment != null
                    && model.Comment.IsGlobal)
                {
                    var comment = new CommentData() { Id = Guid.NewGuid().ToString(), Text = model.Comment.Text };
                    issue.IdComment = comment.Id;
                    issue.Comments.Add(issue.IdComment);
                    dbComment.Update(comment);
                    dbIssue.Update(model.Issue);
                }

            }
        }

        private void DownloadIssue(int idIssue)
        {
            var issue = redmine.GetIssue(idIssue);
            dbRedmineIssue.Update(issue);

            if (issue.IdParent.HasValue)
            {
                var idParent = issue.IdParent.Value;
                issue = dbRedmineIssue.Get(idParent);

                if (issue == null)
                {
                    issue = redmine.GetIssue(idParent);
                    dbRedmineIssue.Update(issue);
                }
            }
        }

        [EventSubscription(Main.Events.DelIssue, typeof(Subscribe<Main.IView>))]
        public void OnDelIssueEvent(object sender, EventArgs arg)
        {
            if (model.Issue.Id > 0)
            {
                dbIssue.Delete(model.Issue);
                LoadIssue(dbIssue.Get(0));
            }
        }

        private void LoadIssue(IssueData inIssue)
        {
            model.Issue = inIssue;

            model.WorkTime = inIssue.GetWorkTime(model.WorkTime);

            model.IssueComments.Clear();
            model.IssueComments.AddRange(dbComment.GetList(inIssue));
            if (inIssue.Id > 0)
                model.IssueComments.AddRange(dbComment.GetList(dbIssue.Get(0)).Select(x => { x.IsGlobal = true; return x; }));

            model.Comment = model.IssueComments.Where(x => x.Id == inIssue.IdComment).FirstOrDefault();

            model.IssueInfo = dbRedmineIssue.Get(inIssue.Id);
            model.IssueParentInfo = dbRedmineIssue.Get(model.IssueInfo.IdParent.GetValueOrDefault(-1));

            model.Sync.Value(SyncTarget.View, "Comment");
            model.Sync.Value(SyncTarget.View, "IssueInfo");
            model.Sync.Value(SyncTarget.View, "IssueParentInfo");
        }
    }
}
