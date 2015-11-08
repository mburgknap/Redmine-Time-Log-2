using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
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
        private IDbComment dbComment;
        private IDbIssue dbIssue;
        private IDbRedmineIssue dbRedmineIssue;
        private IDbCache dbCache;
        private IRedmineClient redmine;
        private IDbConfig dbConfig;
        private Main.IModel model;
        private Main.IView view;
        private IDbLastIssue dbLastIssue;

        [Inject]
        public MainFormLogic(Main.IView inView, Main.IModel inModel, IRedmineClient inClient, IDbConfig inDbConfig, IDbIssue inDbIssue, IDbComment inDbComment, IDbRedmineIssue inDbRedmineIssue, IDbCache inDbCache, IDbLastIssue inDbLastIssue)
        {
            view = inView;
            model = inModel;
            redmine = inClient;
            dbIssue = inDbIssue;
            dbConfig = inDbConfig;
            dbComment = inDbComment;
            dbCache = inDbCache;
            dbLastIssue = inDbLastIssue;
            dbRedmineIssue = inDbRedmineIssue;

            model.Activity.OnNotify.Subscribe(x =>
            {
                System.Diagnostics.Debug.WriteLine(x.Name);
            });
        }

        [EventSubscription(Main.Events.AddComment, typeof(Subscribe<Main.IView>))]
        public void OnAddCommentEvent(object sender, Args<string> arg)
        {
            var comment = new CommentData()
            {
                Id = Guid.NewGuid().ToString(),
                Text = arg.Data,
                IsGlobal = model.Issue.Value.Id == 0
            };

            model.Comment.Update(comment);
            dbComment.Update(comment);

            model.Issue.Value.Comments.Add(comment.Id);
            if (model.Issue.Value.Id > 0)
                model.Issue.Value.IdComment = comment.Id;

            dbIssue.Update(model.Issue.Value);

            model.IssueComments.Value.Add(comment);
            model.IssueComments.Update();
        }

        [EventSubscription(Main.Events.AddIssue, typeof(Subscribe<Main.IView>))]
        public void OnAddIssueEvent(object sender, Args<string> arg)
        {
            int idIssue = 0;

            if (Int32.TryParse(arg.Data, out idIssue))
            {
                if (!ReloadIssueData(idIssue))
                    LoadIssue(dbIssue.Get(0));
            }
            else
            {
                model.Comment.Update(null, false);
                LoadIssue(dbIssue.Get(0));
            }
        }

        [EventSubscription(Main.Events.DelComment, typeof(Subscribe<Main.IView>))]
        public void OnDelCommentEvent(object sender, EventArgs arg)
        {
            if (model.Comment != null)
            {
                model.Issue.Value.IdComment = null;
                model.Issue.Value.Comments.Remove(model.Comment.Value.Id);
                model.IssueComments.Value.Remove(model.Comment.Value);

                dbIssue.Update(model.Issue.Value);
                dbComment.Delete(model.Comment.Value);

                model.Comment.Update();
                model.IssueComments.Update();
            }

        }

        [EventSubscription(Main.Events.DelIssue, typeof(Subscribe<Main.IView>))]
        public void OnDelIssueEvent(object sender, EventArgs arg)
        {
            if (model.Issue.Value.Id > 0)
            {
                dbIssue.Delete(model.Issue.Value);
                dbLastIssue.Delete(model.Issue.Value.Id);
                LoadIssue(dbIssue.Get(0));
            }
        }

        [EventSubscription(Main.Events.Exit, typeof(Subscribe<Main.IView>))]
        public void OnExitEvent(object sender, EventArgs arg)
        {
            var idleIssue = dbIssue.Get(-1);
            idleIssue.SetWorkTime(model.IdleTime.Value);
            dbIssue.Update(idleIssue);

            model.Issue.Value.SetWorkTime(model.WorkTime.Value);
            dbIssue.Update(model.Issue.Value);
        }

        [EventSubscription(Main.Events.Link, typeof(Subscribe<Main.IView>))]
        public void OnLinkEvent(object sender, Args<String> arg)
        {
            if (arg.Data.Equals("Redmine"))
            { view.GoLink(redmine.IssueListUrl()); }
            else if (arg.Data.Equals("Issue"))
            { view.GoLink(new Uri(redmine.IssueUrl(model.Issue.Value.Id))); }
        }

        [EventSubscription(Main.Events.Load, typeof(Subscribe<Main.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            SetupStartTime();

            if (!dbCache.HasWorkActivities)
                dbCache.InitWorkActivities(redmine.GetWorkActivityTypes());
            else
                model.WorkActivities.Value.AddRange(dbCache.GetWorkActivityTypes());

            model.WorkActivities.Update();

            dbIssue.Init();
            dbLastIssue.Init();
            dbComment.Init();
            dbRedmineIssue.Init();

            LoadIssue(dbIssue.Get(0));
            LoadIdle();
            LoadLastIssues();

        }

        private void LoadLastIssues()
        {
            RedmineIssueData tmpRedmine = null;
            IssueData tmpIssue = null;

            model.LastIssues.Value.Clear();

            foreach (var item in dbLastIssue.GetList())
            {
                tmpRedmine = dbRedmineIssue.Get(item);
                tmpIssue = dbIssue.Get(item);

                if (tmpRedmine.IdParent.HasValue)
                    model.LastIssues.Value.Add(tmpIssue, tmpRedmine, dbRedmineIssue.Get(tmpRedmine.IdParent.Value));
                else
                    model.LastIssues.Value.Add(tmpIssue, tmpRedmine, null);

                var tmpComment = dbComment.Get(tmpIssue);
                if (tmpComment != null)
                    model.LastIssues.Value.Last().Comment = tmpComment.Text;

                model.LastIssues.Value.Last().IssueUri = redmine.IssueUrl(item);
            }

            model.LastIssues.Update();
        }

        private void SetupStartTime()
        {
            var time = dbConfig.GetStartTime();

            if (time.HasValue)
            {
                model.StartTime.Update(time.Value);

                if (time.Value.Date < DateTime.Now.Date)
                {
                    model.StartTime.Update(DateTime.Now);
                    dbConfig.SetStartTime(model.StartTime.Value);
                }
            }
            else
            {
                model.StartTime.Update(DateTime.Now);
                dbConfig.SetStartTime(model.StartTime.Value);
            }
        }

        [EventSubscription(Main.Events.Reset, typeof(Subscribe<Main.IView>))]
        public void OnResetEvent(object sender, Args<Main.Actions> arg)
        {
            if (arg.Data == Main.Actions.Issue)
            {
                model.WorkTime.Update(new TimeSpan(0));
                model.Issue.Value.SetWorkTime(model.WorkTime.Value);
                dbIssue.Update(model.Issue.Value);
                LoadIssue(dbIssue.Get(0));
            }
            else
            {
                model.IdleTime.Update(new TimeSpan(0));
            }
        }

        [EventSubscription(BugLog.Events.Select, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<BugLogItem> arg)
        {
            var issue = dbIssue.Get(arg.Data.Id);

            if (issue == null)
            {
                ReloadIssueData(arg.Data.Id);
            }
            else
            {
                SetupLastIssue(issue);
                LoadIssue(issue);
            }
        }

        [EventSubscription(IssueLog.Events.Select, typeof(OnPublisher))]
        public void OnSelectEvent(object sender, Args<WorkingIssue> arg)
        {
            SetupLastIssue(arg.Data.Data);
            LoadIssue(arg.Data.Data);
        }

        [EventSubscription(IssueLog.Events.Delete, typeof(OnPublisher))]
        public void OnDeleteEvent(object sender, Args<WorkingIssue> arg)
        {
            if (arg.Data.Data.Id > 0)
            {
                if (model.Issue.Value.Id == arg.Data.Data.Id)
                    LoadIssue(dbIssue.Get(0));

                dbIssue.Delete(arg.Data.Data);
                dbLastIssue.Delete(arg.Data.Data.Id);
            }
        }

        [EventSubscription(Main.Events.IssueResolve, typeof(OnPublisher))]
        public void OnResolveEvent(object sender, Args<WorkingIssue> arg)
        {
            redmine.Resolve(arg.Data);

            if (model.Issue.Value.Id == arg.Data.Data.Id)
                LoadIssue(dbIssue.Get(0));

            dbIssue.Delete(arg.Data.Data);
            dbLastIssue.Delete(arg.Data.Data.Id);
        }

        [EventSubscription(BugLog.Events.Resolve, typeof(OnPublisher))]
        public void OnResolveEvent(object sender, Args<BugLogItem> arg)
        {
            redmine.Resolve(arg.Data);

            if (model.Issue.Value.Id == arg.Data.Id)
                LoadIssue(dbIssue.Get(0));

            dbIssue.Delete(arg.Data);
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

        [EventSubscription(Main.Events.AddSubIssue, typeof(OnPublisher))]
        public void OnAddSubIssueEvent(object sender, Args<SubIssueData> arg)
        {
            int newIssueId = redmine.AddSubIssue(arg.Data);
            OnAddIssueEvent(view, new Args<string>(newIssueId.ToString()));
        }

        [EventSubscription(Main.Events.Submit, typeof(Subscribe<Main.IView>))]
        public void OnSubmitEvent(object sender, Args<Main.Actions> arg)
        {
            if (model.Comment.Value == null)
            {
                view.Info("Write comment to the task");
                return;
            }

            if (!(model.Issue.Value.Id > 0))
            {
                view.Info("Select issue to the task");
                return;
            }

            var workData = new WorkTimeData()
            {
                IdUssue = model.Issue.Value.Id,
                IdActivityType = model.Activity.Value.Id,
                Time = new TimeSpan(),
                Comment = model.Comment.Value.Text
            };

            if (arg.Data == Main.Actions.Issue || arg.Data == Main.Actions.All)
            {
                workData.Time = workData.Time.Add(model.WorkTime.Value);
                model.WorkTime.Update(new TimeSpan(0));
            }

            if (arg.Data == Main.Actions.Idle || arg.Data == Main.Actions.All)
            {
                workData.Time = workData.Time.Add(model.IdleTime.Value);
                model.IdleTime.Update(new TimeSpan(0));
            }

            model.Issue.Value.Time = null;

            if (!redmine.AddWorkTime(workData))
            { model.Issue.Value.SetWorkTime(workData.Time); }

            if (model.Resolve.Value)
            {
                redmine.Resolve(model.Issue.Value);
                dbIssue.Delete(model.Issue.Value);
                dbLastIssue.Delete(model.Issue.Value.Id);
                LoadIssue(dbIssue.Get(0));
                model.Resolve.Update(false);
            }
            else
            {

                if (arg.Data == Main.Actions.Issue || arg.Data == Main.Actions.All)
                {
                    dbIssue.Update(model.Issue.Value);
                    LoadIssue(dbIssue.Get(0));
                    LoadLastIssues();
                }

                if (arg.Data == Main.Actions.Idle || arg.Data == Main.Actions.All)
                {
                    var idleIssue = dbIssue.Get(-1);
                    idleIssue.SetWorkTime(new TimeSpan(0));
                    dbIssue.Update(idleIssue);
                }
            }
        }

        [EventSubscription(Main.Events.UpdateComment, typeof(Subscribe<Main.IView>))]
        public void OnUpdateCommentEvent(object sender, Args<string> arg)
        {
            if (model.Comment.Value != null)
            {
                model.Comment.Value.Text = arg.Data;

                if (model.Issue.Value.Id > 0)
                    model.Issue.Value.IdComment = model.Comment.Value.Id;

                dbIssue.Update(model.Issue.Value);
                dbComment.Update(model.Comment.Value);
            }
        }

        [EventSubscription(Main.Events.UpdateIssue, typeof(Subscribe<Main.IView>))]
        public void OnUpdateIssueEvent(object sender, Args<string> arg)
        {
            int idIssue = -1;

            if (Int32.TryParse(arg.Data, out idIssue))
            {
                if (model.Issue.Value.Id == idIssue)
                {
                    model.Issue.Value.SetWorkTime(model.WorkTime.Value);
                    dbIssue.Update(model.Issue.Value);
                }
                else
                {
                    ReloadIssueData(idIssue);
                }

                LoadLastIssues();
            }
        }

        private void DownloadIssue(int idIssue)
        {
            var issue = redmine.GetIssue(idIssue);

            if (issue != null)
            {
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

        }

        private void LoadIdle()
        {
            var idleIssue = dbIssue.Get(-1);
            model.IdleTime.Update(idleIssue.GetWorkTime(new TimeSpan(0)));
        }
        private void LoadIssue(IssueData inIssue)
        {
            model.Issue.Update(inIssue);

            model.WorkTime.Update(inIssue.GetWorkTime(model.WorkTime.Value));

            model.IssueComments.Value.Clear();
            model.IssueComments.Value.AddRange(dbComment.GetList(inIssue));

            if (inIssue.Id > 0)
                model.IssueComments.Value.AddRange(dbComment.GetList(dbIssue.Get(0)).Select(x => { x.IsGlobal = true; return x; }));

            model.IssueComments.Update();

            model.Comment.Update(model.IssueComments.Value.Where(x => x.Id == inIssue.IdComment).FirstOrDefault());

            model.IssueInfo.Update(dbRedmineIssue.Get(inIssue.Id));
            model.IssueParentInfo.Update(dbRedmineIssue.Get(model.IssueInfo.Value.IdParent.GetValueOrDefault(-1)));

            SetupLastIssueList(inIssue.Id);


        }

        private void SetupLastIssueList(int inId)
        {
            var tmp = dbLastIssue.GetList();

            if (inId > 0)
            {
                if (tmp.Count > 4 && !tmp.Contains(inId))
                    tmp = tmp.Take(4).ToList();

                if (tmp.Contains(inId))
                    tmp.Remove(inId);

                tmp.Insert(0, inId);
                dbLastIssue.Update(tmp);
                LoadLastIssues();
            }
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
        private void SetupLastIssue(IssueData issue)
        {
            if (model.Issue.Value.Id > 0)
            {
                model.Issue.Value.IdComment = model.Comment.Value != null ? model.Comment.Value.Id : null;
                model.Issue.Value.SetWorkTime(model.WorkTime.Value);
                dbIssue.Update(model.Issue.Value);

                model.WorkTime.Update(new TimeSpan(0));
            }
            else
            {
                issue.AddWorkTime(model.WorkTime.Value);

                model.Issue.Value.SetWorkTime(new TimeSpan(0));
                dbIssue.Update(model.Issue.Value);

                if (model.Comment.Value != null
                    && model.Comment.Value.IsGlobal)
                {
                    var comment = new CommentData() { Id = Guid.NewGuid().ToString(), Text = model.Comment.Value.Text };
                    issue.IdComment = comment.Id;
                    issue.Comments.Add(issue.IdComment);
                    dbComment.Update(comment);
                    dbIssue.Update(model.Issue.Value);
                    model.Comment.Update();
                }

            }
        }
    }
}
