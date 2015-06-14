using Appccelerate.EventBroker;
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


        [EventSubscription(Main.Events.AddComment, typeof(Subscribe<Main.IView>))]
        public void OnAddCommentEvent(object sender, Args<string> arg)
        {
            var comment = new CommentData() { Id = Guid.NewGuid().ToString(), Text = arg.Data };
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
            model.Comment.Text = arg.Data;
            dbComment.Update(model.Comment);
        }

        [EventSubscription(Main.Events.DelComment, typeof(Subscribe<Main.IView>))]
        public void OnDelCommentEvent(object sender, EventArgs arg)
        {
        }

        [EventSubscription(Main.Events.AddIssue, typeof(Subscribe<Main.IView>))]
        public void OnAddIssueEvent(object sender, Args<string> arg)
        {
            int idIssue = 0;

            if (Int32.TryParse(arg.Data, out idIssue))
            {
                DownloadIssue(idIssue);

                var tmpIssue = dbRedmineIssue.Get(idIssue);

                var issue = dbIssue.Get(idIssue);

                if (tmpIssue != null && issue == null)
                    dbIssue.Update(issue = new IssueData() { Id = tmpIssue.Id, UsedCount = 1 });

                if (issue != null)
                    LoadIssue(issue);
                else
                    LoadIssue(dbIssue.Get(0));
            }
            else
                LoadIssue(dbIssue.Get(0));
        }

        private void DownloadIssue(int idIssue)
        {
            var issue = redmine.GetIssue(idIssue);
            dbRedmineIssue.Update(issue);


            if (issue.IdParent.HasValue)
            {
                issue = dbRedmineIssue.Get(issue.IdParent.Value);

                if (issue == null)
                {
                    issue = redmine.GetIssue(idIssue);
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
            model.Comment = null;

            model.IssueComments.Clear();
            model.IssueComments.AddRange(dbComment.GetList(inIssue));
            if (inIssue.Id > 0)
                model.IssueComments.AddRange(dbComment.GetList(dbIssue.Get(0)).Select(x => { x.IsGlobal = true; return x; }));

            model.IssueInfo = dbRedmineIssue.Get(inIssue.Id);
            model.IssueParentInfo = dbRedmineIssue.Get(model.IssueInfo.IdParent.GetValueOrDefault(-1));

            model.Sync.Value(SyncTarget.View, "Comment");
            model.Sync.Value(SyncTarget.View, "IssueInfo");
            model.Sync.Value(SyncTarget.View, "IssueParentInfo");
        }
    }
}
