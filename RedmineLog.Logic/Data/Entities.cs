using Polenter.Serialization;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace RedmineLog.Logic.Data
{
    public class AppSettings
    {
        public string Url { get; set; }

        public string ApiKey { get; set; }

        public int IdleStateWaitTime { get; set; }

        public int SaveIdleTimeInfo { get; set; }

        public int SnoozeTime { get; set; }

        public int ServiceSleepTime { get; set; }

        public int IdUser { get; set; }

        public void Save()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                try
                {
                    new SharpSerializer().Serialize(this, Path.Combine("Database", typeof(AppSettings).Name + ".xml"));
                    AppLogger.Log.Info("RedmineConfig saved");
                }
                catch (Exception ex)
                { AppLogger.Log.Error("RedmineConfig Save ", ex); }
            }));
        }

        public bool Load()
        {
            if (File.Exists(new Uri(Path.Combine("Database", typeof(AppSettings).Name + ".xml"), UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(Path.Combine("Database", typeof(AppSettings).Name + ".xml")) as AppSettings;

                if (obj != null)
                {
                    Url = obj.Url;
                    ApiKey = obj.ApiKey;
                    IdleStateWaitTime = obj.IdleStateWaitTime;
                    SnoozeTime = obj.SnoozeTime;
                    ServiceSleepTime = obj.ServiceSleepTime;
                    IdUser = obj.IdUser;
                    return true;
                }
            }

            ServiceSleepTime = 60;
            SnoozeTime = 10;
            IdleStateWaitTime = 60;
            SaveIdleTimeInfo = 30;
            IdUser = 0;

            Save();

            return false;
        }
    }

    public class ActivityData : List<ActivityData.TaskActivity>
    {
        public class TaskActivity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public void Add(int inId, string inName)
        {
            Add(new TaskActivity() { Id = inId, Name = inName });
        }
    }

    public class TimeLogData : List<TimeLogData.TaskTime>
    {
        public DateTime IdleTime { get; set; }

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

        public void Save()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                try
                {
                    new SharpSerializer().Serialize(this, Path.Combine("Database", typeof(TimeLogData).Name + ".xml"));
                    AppLogger.Log.Info("TimeLogData saved");
                }
                catch (Exception ex)
                { AppLogger.Log.Error("TimeLogData Save ", ex); }
            }));
        }

        public bool Load()
        {
            if (File.Exists(new Uri(Path.Combine("Database", typeof(TimeLogData).Name + ".xml"), UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(Path.Combine("Database", typeof(TimeLogData).Name + ".xml")) as TimeLogData;

                if (obj != null)
                {
                    AddRange(obj);
                    IdleTime = obj.IdleTime;
                    return true;
                }
            }

            IdleTime = DateTime.MinValue;
            Save();

            return false;
        }

        public TimeLogData.TaskTime Get(int inIdIssue)
        {
            return this.Where(x => x.IdIssue == inIdIssue).FirstOrDefault();
        }

        public void RemoveId(int inIdIssue)
        {
            Remove(Get(inIdIssue));
        }

        public bool IsStarted(int inIdIssue)
        {
            var work = Get(inIdIssue);

            if (work != null)
                return new TimeSpan(work.WorkTime.Ticks).TotalMinutes > 1;

            return false;
        }
    }

    public class RedmineIssues : List<RedmineIssues.Item>
    {
        public class Item
        {
            public Item()
            {
            }

            public Item(Issue issue, Project project)
            {
                Id = issue.Id;
                if (issue.ParentIssue != null)
                    IdParent = issue.ParentIssue.Id;
                else
                    IdParent = null;

                Subject = issue.Subject;

                if (project != null)
                    Project = project.Name;
            }

            public int Id { get; set; }

            public int? IdParent { get; set; }

            public string Subject { get; set; }

            public string Project { get; set; }
        }

        public void Save()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                try
                {
                    new SharpSerializer().Serialize(this, Path.Combine("Database", typeof(RedmineIssues).Name + ".xml"));
                    AppLogger.Log.Info("RedmineIssuesCache saved");
                }
                catch (Exception ex)
                { AppLogger.Log.Error("RedmineIssuesCache Save ", ex); }
            }));
        }

        public bool Load()
        {
            if (File.Exists(new Uri(Path.Combine("Database", typeof(RedmineIssues).Name + ".xml"), UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(Path.Combine("Database", typeof(RedmineIssues).Name + ".xml")) as RedmineIssues;

                if (obj != null)
                {
                    AddRange(obj);
                    return true;
                }
            }

            Add(new Item() { Id = -1, Subject = "" });

            Save();

            return false;
        }

        public RedmineIssues.Item GetIssue(int id)
        {
            return this.Where(x => x.Id == id).FirstOrDefault();
        }

        public void RemoveIssue(int id)
        {
            Remove(GetIssue(id));
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

        public void Save()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                try
                {
                    new SharpSerializer().Serialize(this, Path.Combine("Database", typeof(LogData).Name + ".xml"));
                    AppLogger.Log.Info("IssueHistory saved");
                }
                catch (Exception ex)
                { AppLogger.Log.Error("IssueHistory Save ", ex); }
            }));
        }

        public bool Load()
        {
            if (File.Exists(new Uri(Path.Combine("Database", typeof(LogData).Name + ".xml"), UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(Path.Combine("Database", typeof(LogData).Name + ".xml")) as LogData;

                if (obj != null)
                {
                    AddRange(obj);
                    return true;
                }
            }

            Add(new LogData.Issue(-1));
            Save();

            return false;
        }

        public LogData.Issue GetIssue(object inObj)
        {
            if (inObj is LogData.Issue)
                return this.Where(x => x.Equals(inObj)).FirstOrDefault();

            if (inObj is int)
                return this.Where(x => x.Id == ((int)inObj)).FirstOrDefault();

            return null;
        }
    }


}