using Polenter.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog
{
    public class RedmineConfig
    {
        public string Url { get; set; }

        public string ApiKey { get; set; }

        internal void Save()
        {
            new SharpSerializer().Serialize(this, typeof(RedmineConfig).Name + ".xml");
            AppLogger.Log.Info("RedmineConfig saved");
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
        public class TaskTime
        {
            public RedmineData.Issue Issue { get; set; }
            public RedmineData.Issue.Comment Comment { get; set; }
            public DateTime Time { get; set; }
        }

    }

    public class RedmineData : List<RedmineData.Issue>
    {


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
        }

        internal void Save()
        {
            new SharpSerializer().Serialize(this, typeof(RedmineData).Name + ".xml");
            AppLogger.Log.Info("IssueHistory saved");
        }

        internal bool Load()
        {
            if (File.Exists(new Uri(typeof(RedmineData).Name + ".xml", UriKind.Relative).ToString()))
            {
                var obj = new SharpSerializer().Deserialize(typeof(RedmineData).Name + ".xml") as RedmineData;

                if (obj != null)
                {
                    AddRange(obj);
                    return true;
                }
            }

            Add(new RedmineData.Issue(-1));

            return false;
        }

        internal RedmineData.Issue GetIssue(object inObj)
        {
            if (inObj is RedmineData.Issue)
                return this.Where(x => x.Equals(inObj)).FirstOrDefault();

            if (inObj is int)
                return this.Where(x => x.Id == ((int)inObj)).FirstOrDefault();

            return null;
        }
    }
}
