using Polenter.Serialization;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedmineLog
{
    public class RedmineConfig
    {
        public string Url { get; set; }

        public string ApiKey { get; set; }

        internal void Save()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                new SharpSerializer().Serialize(this, typeof(RedmineConfig).Name + ".xml");
                AppLogger.Log.Info("RedmineConfig saved");
            }));
        }

        internal bool Load()
        {
            if (File.Exists(new Uri(typeof(RedmineConfig).Name + ".xml", UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(typeof(RedmineConfig).Name + ".xml") as RedmineConfig;

                if (obj != null)
                {
                    Url = obj.Url;
                    ApiKey = obj.ApiKey;
                    return true;
                }
            }

            return false;
        }
    }


    public class TimeLogData : List<TimeLogData.TaskTime>
    {
        public int IdleSeconds { get; set; }

        public class TaskTime
        {
            public TaskTime()
            {
            }

            public TaskTime(int idIssue, Guid? inIdComment, DateTime inWorkTime)
            {
                IdIssue = idIssue;
                WorkTime = inWorkTime;
                IdComment = inIdComment;
            }
            public int IdIssue { get; set; }
            public Guid? IdComment { get; set; }
            public DateTime WorkTime { get; set; }
        }


        internal void Save()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                new SharpSerializer().Serialize(this, typeof(TimeLogData).Name + ".xml");
                AppLogger.Log.Info("TimeLogData saved");
            }));
        }

        internal bool Load()
        {
            if (File.Exists(new Uri(typeof(TimeLogData).Name + ".xml", UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(typeof(TimeLogData).Name + ".xml") as TimeLogData;

                if (obj != null)
                {
                    AddRange(obj);
                    IdleSeconds = obj.IdleSeconds;
                    return true;
                }
            }

            IdleSeconds = 60;

            return false;
        }


        internal TimeLogData.TaskTime Get(int inIdIssue)
        {
            return this.Where(x => x.IdIssue == inIdIssue).FirstOrDefault();
        }

        internal void RemoveId(int inIdIssue)
        {
            Remove(Get(inIdIssue));
        }
    }

    public class RedmineIssues : List<RedmineIssues.Item>
    {

        public class Item
        {
            public Item()
            {
            }

            public Item(Issue issue)
            {
                Id = issue.Id;
                if (issue.ParentIssue != null)
                    IdParent = issue.ParentIssue.Id;
                else
                    IdParent = null;

                Subject = issue.Subject;
            }
            public int Id { get; set; }

            public int? IdParent { get; set; }

            public string Subject { get; set; }

        }

        internal void Save()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                new SharpSerializer().Serialize(this, typeof(RedmineIssues).Name + ".xml");
                AppLogger.Log.Info("RedmineIssuesCache saved");
            }));
        }

        internal bool Load()
        {
            if (File.Exists(new Uri(typeof(RedmineIssues).Name + ".xml", UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(typeof(RedmineIssues).Name + ".xml") as RedmineIssues;

                if (obj != null)
                {
                    AddRange(obj);
                    return true;
                }
            }
            return false;
        }

        internal RedmineIssues.Item GetIssue(int id)
        {
            return this.Where(x => x.Id == id).FirstOrDefault();
        }
    }

    public class LogData : List<LogData.Issue>
    {
        public new bool Contains(LogData.Issue item)
        {
            return this.Where(x => x.Equals(item)).FirstOrDefault() != null;
        }

        public class Issue
        {
            public class Comment
            {
                public Comment()
                {
                    Text = "";
                }

                public Guid Id { get; set; }
                public string Text { get; set; }

                public override string ToString()
                {
                    return Text;
                }

                public override bool Equals(object obj)
                {
                    if (obj is Comment)
                        return Id.Equals(((Comment)obj).Id);

                    return false;
                }

                public override int GetHashCode()
                {
                    return Id.GetHashCode();
                }

                public bool IsGlobal { get; set; }
            }

            public Issue()
            {
                Comments = new List<Comment>();
            }

            public Issue(string id)
                : this()
            {
                int tmp = -1;
                Int32.TryParse(id.Trim(), out tmp);

                Id = tmp;
            }

            public Issue(int id)
                : this()
            {
                Id = id;
            }
            public int Id { get; set; }

            public List<Comment> Comments { get; set; }

            public override string ToString()
            {
                if (Id == -1)
                    return "";

                return Id.ToString();
            }

            public override bool Equals(object obj)
            {
                if (obj is Issue)
                    return Id == ((Issue)obj).Id;

                return false;
            }

            public override int GetHashCode()
            {
                return Id;
            }

            public bool IsValid { get { return Id > 0; } }

            public int UsedCount { get; set; }
        }

        internal void Save()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                new SharpSerializer().Serialize(this, typeof(LogData).Name + ".xml");
                AppLogger.Log.Info("IssueHistory saved");
            }));
        }

        internal bool Load()
        {
            if (File.Exists(new Uri(typeof(LogData).Name + ".xml", UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(typeof(LogData).Name + ".xml") as LogData;

                if (obj != null)
                {
                    AddRange(obj);
                    return true;
                }
            }

            Add(new LogData.Issue(-1));

            return false;
        }

        internal LogData.Issue GetIssue(object inObj)
        {
            if (inObj is LogData.Issue)
                return this.Where(x => x.Equals(inObj)).FirstOrDefault();

            if (inObj is int)
                return this.Where(x => x.Id == ((int)inObj)).FirstOrDefault();

            return null;
        }
    }
}
