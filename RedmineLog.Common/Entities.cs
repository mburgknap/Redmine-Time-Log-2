using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{

    public class RedmineIssueData
    {
        public int Id { get; set; }
        public int? IdParent { get; set; }
        public String Subject { get; set; }
        public String Project { get; set; }
    }

    public class CommentData
    {
        public CommentData()
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
            if (obj is CommentData)
                return Id.Equals(((CommentData)obj).Id);

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class IssueCommentList : List<CommentData>
    {
        public void Init(IEnumerable<CommentData> enumerable)
        {
            Clear();
            AddRange(enumerable);
        }
    }

    public class IssueData
    {
        public IssueData()
        {
            Comments = new List<Guid>();
        }

        public int Id { get; set; }

        public List<Guid> Comments { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is IssueData)
                return Id == ((IssueData)obj).Id;

            return false;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public int UsedCount { get; set; }
    }

    public class WorkActivityType
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }


    public class WorkActivityList : List<WorkActivityType>
    {
        public void Add(int inId, string inName)
        {
            Add(new WorkActivityType() { Id = inId, Name = inName });
        }
    }
}
