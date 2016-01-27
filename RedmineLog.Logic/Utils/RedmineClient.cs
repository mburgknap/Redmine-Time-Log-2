using FluentDateTime;
using Ninject;
using NLog;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{

    internal class RedmineClient : IRedmineClient
    {
        private IDbRedmine dbRedmine;
        private ILog logger;

        [Inject]
        public RedmineClient(IDbRedmine inDbRedmine, ILog inLogger)
        {
            dbRedmine = inDbRedmine;
            logger = inLogger;
        }

        public UserData GetCurrentUser()
        {
            try
            {
                var parameters = new NameValueCollection { };
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());
                var user = manager.GetCurrentUser(parameters);

                return new UserData()
                {
                    Id = user.Id,
                    Name = user.FirstName + " " + user.LastName,
                    IsDefault = true
                };
            }
            catch (Exception ex)
            { logger.Error("GetCurrentUser", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return new UserData() { Id = 0, Name = "Unknown" };
        }






        public RedmineIssueData GetIssue(int idIssue)
        {
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());
                var parameters = new NameValueCollection { };

                var issue = manager.GetObject<Issue>(idIssue.ToString(), parameters);
                var project = manager.GetObject<Project>(issue.Project.Id.ToString(), parameters);

                return new RedmineIssueData()
                {
                    Id = idIssue,
                    IdParent = issue.ParentIssue != null ? issue.ParentIssue.Id : (int?)null,
                    Project = project.Name,
                    Tracker = issue.Tracker.Name,
                    Subject = issue.Subject
                };
            }
            catch (Exception ex)
            { logger.Error("GetIssue", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return null;
        }


        public bool AddWorkTime(WorkTimeData workData)
        {
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());
                var parameters = new NameValueCollection { };

                var response = manager.CreateObject<TimeEntry>(new TimeEntry()
                {
                    Issue = new IdentifiableName() { Id = workData.IdUssue },
                    Activity = new IdentifiableName() { Id = workData.IdActivityType },
                    Comments = workData.Comment,
                    Hours = decimal.Round(workData.ToHours(), 2),
                    SpentOn = DateTime.Now
                });

                if (response != null)
                    return true;
            }
            catch (Exception ex)
            { logger.Error("AddWorkTime", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return false;
        }


        public Uri IssueListUrl()
        {
            return new Uri(dbRedmine.GetUrl() + "issues");
        }


        public IEnumerable<WorkLogItem> GetWorkLogs(int idUser, DateTime workDate)
        {
            var list = new List<WorkLogItem>();

            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var parameters = new NameValueCollection { };
                parameters.Add("user_id", idUser.ToString());
                parameters.Add("spent_on", workDate.ToString("yyyy-MM-dd"));


                foreach (var item in from p in manager.GetObjectList<TimeEntry>(parameters)
                                     orderby p.SpentOn descending
                                     group p by p.SpentOn.Value.Date into g
                                     select new { Date = g.Key, Entities = g.ToList() })
                {
                    foreach (var item2 in item.Entities)
                    {
                        list.Add(new WorkLogItem()
                        {
                            Date = item.Date,
                            Id = item2.Id,
                            IdIssue = item2.Issue.Id,
                            ProjectName = item2.Project.Name,
                            IdActivity = item2.Activity.Id,
                            Hours = item2.Hours,
                            Comment = item2.Comments,
                            ActivityName = item2.Activity.Name,
                        });
                    }


                }

            }
            catch (Exception ex)
            { logger.Error("GetWorkLogs", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return list;
        }


        public void UpdateLog(WorkLogItem workLogItem)
        {
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var timeEntry = new TimeEntry();

                timeEntry.Hours = workLogItem.Hours;
                timeEntry.SpentOn = workLogItem.Date;
                timeEntry.Comments = workLogItem.Comment;
                timeEntry.Activity = new IdentifiableName() { Id = workLogItem.IdActivity };

                manager.UpdateObject<TimeEntry>(workLogItem.Id.ToString(), timeEntry);
            }
            catch (Exception ex)
            { logger.Error("UpdateLog", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }
        }

        public void Resolve(WorkingIssue workingIssue)
        {
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var issue = manager.GetObject<Issue>(workingIssue.Data.Id.ToString(), null);

                issue.Status = new IdentifiableName() { Id = 3 };
                issue.DoneRatio = 100;

                manager.UpdateObject<Issue>(workingIssue.Data.Id.ToString(), issue);
            }
            catch (Exception ex)
            { logger.Error("Resolve IssueData", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }
        }

        public void Resolve(BugLogItem bugLogItem)
        {
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var issue = manager.GetObject<Issue>(bugLogItem.Id.ToString(), null);

                issue.Status = new IdentifiableName() { Id = 3 };
                issue.DoneRatio = 100;

                manager.UpdateObject<Issue>(bugLogItem.Id.ToString(), issue);
            }
            catch (Exception ex)
            { logger.Error("Resolve BugLogItem", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }
        }
        public void Resolve(IssueData issueData)
        {
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var issue = manager.GetObject<Issue>(issueData.Id.ToString(), null);

                issue.Status = new IdentifiableName() { Id = 3 };
                issue.DoneRatio = 100;

                manager.UpdateObject<Issue>(issueData.Id.ToString(), issue);
            }
            catch (Exception ex)
            { logger.Error("Resolve IssueData", ex); }
        }


        public IEnumerable<BugLogItem> GetUserBugs(int idUser)
        {
            var result = new List<BugLogItem>();
            try
            {
                var bugs = new List<Issue>();
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var parameters = new NameValueCollection { };

                parameters.Add("assigned_to_id", idUser.ToString());
                parameters.Add("tracker_id", "1");
                parameters.Add("status_id", 1.ToString());
                bugs.AddRange(manager.GetObjectList<Issue>(parameters));

                parameters = new NameValueCollection { };

                parameters.Add("assigned_to_id", idUser.ToString());
                parameters.Add("tracker_id", "1");
                parameters.Add("status_id", 2.ToString());
                bugs.AddRange(manager.GetObjectList<Issue>(parameters));

                result = bugs.OrderByDescending(x => x.Priority.Id).Select(x => ToBugItem(x)).ToList();

            }
            catch (Exception ex)
            { logger.Error("GetUserBugs", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return result;
        }

        private BugLogItem ToBugItem(Issue x)
        {
            return new BugLogItem()
                 {
                     Id = x.Id,
                     Project = x.Project.Name,
                     Subject = x.Subject,
                     Priority = x.Priority.Name
                 };
        }


        public int AddSubIssue(SubIssueData inIssueData)
        {
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var issue = manager.GetObject<Issue>(inIssueData.ParentId.ToString(), null);

                issue.SpentHours = 0;
                issue.EstimatedHours = 0;
                issue.DoneRatio = 0;

                if (issue.Children != null)
                    issue.Children.Clear();
                if (issue.Attachments != null)
                    issue.Attachments.Clear();
                if (issue.Journals != null)
                    issue.Journals.Clear();
                if (issue.Relations != null)
                    issue.Relations.Clear();
                if (issue.Watchers != null)
                    issue.Watchers.Clear();

                issue.Subject = inIssueData.Subject;
                issue.Description = inIssueData.Description;
                issue.Status = new IdentifiableName() { Id = 1 };
                issue.ParentIssue = new IdentifiableName() { Id = inIssueData.ParentId };
                issue.Tracker = new IdentifiableName() { Id = inIssueData.Tracker.Id };
                issue.Priority = new IdentifiableName() { Id = inIssueData.Priority.Id };
                issue.AssignedTo = new IdentifiableName() { Id = inIssueData.User.Id };

                var newIssue = manager.CreateObject<Issue>(issue);

                return newIssue.Id;
            }
            catch (Exception ex)
            { logger.Error("AddSubIssue ", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return 0;
        }
        public IEnumerable<WorkActivityType> GetWorkActivityTypes()
        {
            try
            {
                var parameters = new NameValueCollection { };

                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var responseList = manager.GetObjectList<TimeEntryActivity>(parameters);

                return responseList.Select(x => new WorkActivityType()
                {
                    Id = x.Id,
                    Name = x.Name
                });
            }
            catch (Exception ex)
            { logger.Error("GetWorkActivityTypes", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return new List<WorkActivityType>(); ;
        }

        public IEnumerable<UserData> GetUsers()
        {
            try
            {
                var parameters = new NameValueCollection { };

                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var responseList = manager.GetObjectList<Redmine.Net.Api.Types.User>(parameters);

                return responseList.Select(x => new UserData()
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName
                });
            }
            catch (Exception ex)
            { logger.Error("GetUsers", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return new List<UserData>(); ;
        }

        public IEnumerable<TrackerData> GetTrackers()
        {
            try
            {
                var parameters = new NameValueCollection { };

                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var responseList = manager.GetObjectList<Tracker>(parameters);

                return responseList.Select(x => new TrackerData()
                {
                    Id = x.Id,
                    Name = x.Name
                });
            }
            catch (Exception ex)
            { logger.Error("GetTrackers", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return new List<TrackerData>(); ;
        }

        public IEnumerable<PriorityData> GetPriorites()
        {
            try
            {
                var parameters = new NameValueCollection { };

                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var responseList = manager.GetObjectList<IssuePriority>(parameters);

                return responseList.Select(x => new PriorityData()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsDefault = x.IsDefault
                });
            }
            catch (Exception ex)
            { logger.Error("GetPriorites", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return new List<PriorityData>(); ;
        }


        public string IssueUrl(int inIdIssue)
        {
            if (!String.IsNullOrWhiteSpace(dbRedmine.GetUrl()))
                return new Uri(dbRedmine.GetUrl() + "issues/" + inIdIssue).ToString();

            return string.Empty;
        }


        public WorkReportData GetWorkReport(int idUser, WorkReportType inMode)
        {
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var parameters = new NameValueCollection { };
                parameters.Add("limit", 100.ToString());
                parameters.Add("user_id", idUser.ToString());

                DateTime from = inMode == WorkReportType.Week ? DateTime.Now.FirstDayOfWeek() : DateTime.Now.AddDays(-7).FirstDayOfWeek();
                DateTime to = inMode == WorkReportType.Week ? DateTime.Now.LastDayOfWeek() : DateTime.Now.AddDays(-7).LastDayOfWeek();

                parameters.Add("spent_on", String.Format("><{0}|{1}", from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd")));

                WorkReportData result = new WorkReportData();

                result.WeekStart = from;
                result.Day1 = new TimeSpan(0);
                result.Day2 = new TimeSpan(0);
                result.Day3 = new TimeSpan(0);
                result.Day4 = new TimeSpan(0);
                result.Day5 = new TimeSpan(0);
                result.Day6 = new TimeSpan(0);
                result.Day7 = new TimeSpan(0);

                TimeSpan tmp = new TimeSpan(0);

                foreach (var item in manager.GetObjectList<TimeEntry>(parameters))
                {
                    int h = (int)item.Hours;
                    int m = (int)Math.Round((item.Hours % 1) * 60, MidpointRounding.AwayFromZero);
                    tmp = new TimeSpan(h, m, 0);

                    switch (item.SpentOn.Value.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            result.Day1 = result.Day1 + tmp; break;
                        case DayOfWeek.Tuesday:
                            result.Day2 = result.Day2 + tmp; break;
                        case DayOfWeek.Wednesday:
                            result.Day3 = result.Day3 + tmp; break;
                        case DayOfWeek.Thursday:
                            result.Day4 = result.Day4 + tmp; break;
                        case DayOfWeek.Friday:
                            result.Day5 = result.Day5 + tmp; break;
                        case DayOfWeek.Saturday:
                            result.Day6 = result.Day6 + tmp; break;
                        case DayOfWeek.Sunday:
                            result.Day7 = result.Day7 + tmp; break;
                    }

                }

                return result;

            }
            catch (Exception ex)
            { logger.Error("GetWorkReport", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return null;
        }


        public string WorkReportUrl(int idUser, DateTime inDate)
        {
            return string.Format("{0}time_entries/report?utf8=%E2%9C%93&criteria[]=project&criteria[]=issue&f[]=spent_on&op[spent_on]=%3D&v[spent_on][]={2}&f[]=user_id&op[user_id]=%3D&v[user_id][]={1}&f[]=&c[]=project&c[]=spent_on&c[]=user&c[]=activity&c[]=issue&c[]=comments&c[]=hours&columns=day&criteria[]=",
                                 dbRedmine.GetUrl(), idUser, inDate.ToString("yyyy-MM-dd"));
        }


        public string WorkReportUrl(int idUser, WorkReportType inReportType)
        {
            return string.Format("{0}time_entries/report?utf8=%E2%9C%93&criteria[]=project&criteria[]=issue&f[]=spent_on&op[spent_on]={2}&f[]=user_id&op[user_id]=%3D&v[user_id][]={1}&f[]=&c[]=project&c[]=spent_on&c[]=user&c[]=activity&c[]=issue&c[]=comments&c[]=hours&columns=day&criteria[]=",
                                dbRedmine.GetUrl(), idUser, inReportType == WorkReportType.Week ? "w" : "lw");
        }


        public IEnumerable<ProjectData> GetProjects()
        {
            try
            {
                var parameters = new NameValueCollection { };

                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var responseList = manager.GetObjectList<Project>(parameters);

                return responseList.OrderBy(x => x.Name)
                                   .Select(x => new ProjectData()
                {
                    Id = x.Id,
                    Name = x.Name
                });
            }
            catch (Exception ex)
            { logger.Error("GetProjects", ex, "Error occured, error detail saved in application logs ", "Warrnig"); }

            return new List<ProjectData>();
        }
    }
}
