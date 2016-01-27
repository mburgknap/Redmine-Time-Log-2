using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class EntityExtension
    {
        public static bool IsGlobal(this IssueData inThis)
        {
            return inThis.Id == 0;
        }

        public static bool IsGlobal(this BugLogItem inThis)
        {
            return inThis.Id == 0;
        }

        public static bool IsGlobal(this RedmineIssueData inThis)
        {
            return inThis.Id == 0;
        }

    }

    public class RedmineIssueData
    {
        public int Id { get; set; }
        public int? IdParent { get; set; }
        public String Subject { get; set; }
        public String Project { get; set; }
        public String Tracker { get; set; }
    }

    public class CommentData
    {
        public CommentData()
        {
            Text = "";
            IsGlobal = false;
        }

        public string Id { get; set; }

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

        public bool IsGlobal { get; set; }
    }

    public class IssueCommentList : List<CommentData>
    {
        public void Init(IEnumerable<CommentData> enumerable)
        {
            Clear();
            AddRange(enumerable);
        }
    }

    public class ProjectData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProjectList : List<ProjectData>
    {
        public void Add(int inId, string inName)
        {
            Add(new ProjectData() { Id = inId, Name = inName });
        }
    }

    public class IssueData
    {
        public IssueData()
        {
            Comments = new List<string>();
        }

        public int Id { get; set; }

        public List<string> Comments { get; set; }

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

        public long? Time { get; set; }

        public TimeSpan GetWorkTime(TimeSpan inDefualt)
        {
            if (!Time.HasValue)
                return inDefualt;

            return new TimeSpan(Time.Value);
        }
        public void SetWorkTime(TimeSpan timeSpan)
        {
            if (Time == null)
                Time = 0;

            Time = timeSpan.Ticks;
        }

        public void AddWorkTime(TimeSpan timeSpan)
        {
            if (Time == null)
                Time = 0;

            Time = Time + timeSpan.Ticks;
        }

        public string IdComment { get; set; }

    }

    public class WorkTimeData
    {
        public int IdUssue { get; set; }
        public int IdActivityType { get; set; }
        public TimeSpan Time { get; set; }
        public String Comment { get; set; }


        public decimal ToHours()
        {
            return Convert.ToDecimal(Time.Hours)
                   + Convert.ToDecimal(Time.Minutes) / 60;
        }
    }

    public class WorkingIssue
    {
        public IssueData Data { get; set; }

        public String Comment { get; set; }

        public RedmineIssueData Issue { get; set; }

        public RedmineIssueData Parent { get; set; }

        public String IssueUri { get; set; }

    }


    public class WorkingIssueList : List<WorkingIssue>
    {
        public void Add(IssueData inData, RedmineIssueData inIssue, RedmineIssueData inParent)
        {
            Add(new WorkingIssue()
            {
                Data = inData,
                Issue = inIssue,
                Parent = inParent
            });
        }
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


    public class BugLogItem
    {
        public int Id { get; set; }
        public string Subject { get; set; }

        public string Project { get; set; }

        public string Priority { get; set; }

        public string Uri { get; set; }
    }

    public class BugLogList : List<BugLogItem>
    {
    }

    public class WorkLogItem
    {
        public DateTime Date { get; set; }

        public decimal Hours { get; set; }

        public int IdIssue { get; set; }

        public string ProjectName { get; set; }

        public string ActivityName { get; set; }

        public string Comment { get; set; }

        public int Id { get; set; }

        public int IdActivity { get; set; }

        public string ParentIssue { get; set; }

        public string Issue { get; set; }

        public string IssueUri { get; set; }
    }

    public class WorkLogList : List<WorkLogItem>
    {
    }

    public class UserData
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }

    public class UserDataList : List<UserData>
    {
        public void Add(int inId, string inName)
        {
            Add(new UserData() { Id = inId, Name = inName });
        }
    }

    public class TrackerData
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }

    public class TrackerDataList : List<TrackerData>
    {
        public void Add(int inId, string inName)
        {
            Add(new TrackerData() { Id = inId, Name = inName });
        }
    }

    public class PriorityData
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }

    public class PriorityDataList : List<PriorityData>
    {
        public void Add(int inId, string inName)
        {
            Add(new PriorityData() { Id = inId, Name = inName });
        }
    }

    public class SubIssueData
    {
        public int ParentId { get; set; }
        public String Subject { get; set; }
        public String Description { get; set; }
        public UserData User { get; set; }
        public TrackerData Tracker { get; set; }
        public PriorityData Priority { get; set; }
    }

   
}
