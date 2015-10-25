using Ninject;
using NLog;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{

    internal class RedmineClient : IRedmineClient
    {
        private IDbRedmine dbRedmine;
        private Logger logger;

        [Inject]
        public RedmineClient(IDbRedmine inDbRedmine, Logger inLogger)
        {
            dbRedmine = inDbRedmine;
            logger = inLogger;
        }

        public int GetCurrentUser()
        {
            try
            {
                var parameters = new NameValueCollection { };
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());
                var user = manager.GetCurrentUser(parameters);

                return user.Id;
            }
            catch (Exception ex)
            { logger.Error("GetCurrentUser", ex); }

            return 0;
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
            { logger.Error("GetIssue", ex); }

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
            { logger.Error("AddWorkTime", ex); }

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
                            Hours = item2.Hours,
                            Comment = item2.Comments,
                            ActivityName = item2.Activity.Name,
                        });
                    }


                }

            }
            catch (Exception ex)
            { logger.Error("GetWorkLogs", ex); }

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
            { logger.Error("UpdateLog", ex); }
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
            { logger.Error("Resolve IssueData", ex); }
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
            { logger.Error("Resolve BugLogItem", ex); }
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
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                foreach (var priority in new int[] { 5, 4, 3, 2, 1 })
                {
                    foreach (var status in new int[] { 2, 1 })
                    {
                        var parameters = new NameValueCollection { };

                        parameters.Add("assigned_to_id", idUser.ToString());
                        parameters.Add("tracker_id", "1");
                        parameters.Add("priority_id", priority.ToString());
                        parameters.Add("status_id", status.ToString());

                        result.AddRange(manager.GetObjectList<Issue>(parameters).Select(x => ToBugItem(x)));
                    }
                }


            }
            catch (Exception ex)
            { logger.Error("GetUserBugs", ex); }

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
            { logger.Error("AddSubIssue ", ex); }

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
            { logger.Error("GetWorkActivityTypes", ex); }

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
            { logger.Error("GetUsers", ex); }

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
            { logger.Error("GetTrackers", ex); }

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
            { logger.Error("GetPriorites", ex); }

            return new List<PriorityData>(); ;
        }


        public string IssueUrl(int inIdIssue)
        {
            return new Uri(dbRedmine.GetUrl() + "issues/" + inIdIssue).ToString();
        }
    }
}
