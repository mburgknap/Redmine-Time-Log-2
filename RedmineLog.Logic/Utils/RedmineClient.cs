﻿using Ninject;
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

        public Uri IssueUrl(IssueData issue)
        {
            return new Uri(dbRedmine.GetUrl() + "issues/" + issue.Id.ToString());
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

                    list.Add(new WorkLogItem()
                    {
                        Date = item.Date,
                        Id = -1
                    });

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


        public IEnumerable<BugLogItem> GetUserBugs(int idUser)
        {
            var result = new List<BugLogItem>();
            try
            {
                var manager = new RedmineManager(dbRedmine.GetUrl(), dbRedmine.GetApiKey());

                var parameters = new NameValueCollection { };

                var header = new BugLogItem();
                result.Add(header);

                parameters.Add("assigned_to_id", idUser.ToString());
                parameters.Add("tracker_id", 1.ToString());
                parameters.Add("priority_id", 5.ToString());

                var responseList = manager.GetObjectList<Issue>(parameters);

                result.AddRange(responseList.Select(x => ToBugItem(x)));
                header.Subject = result.Last().Priority;

                header = new BugLogItem();
                result.Add(header);
                parameters = new NameValueCollection { };
                parameters.Add("assigned_to_id", idUser.ToString());
                parameters.Add("tracker_id", 1.ToString());
                parameters.Add("priority_id", 4.ToString());

                responseList = manager.GetObjectList<Issue>(parameters);

                result.AddRange(responseList.Select(x => ToBugItem(x)));
                header.Subject = result.Last().Priority;

                header = new BugLogItem();
                result.Add(header);
                parameters = new NameValueCollection { };
                parameters.Add("assigned_to_id", idUser.ToString());
                parameters.Add("tracker_id", 1.ToString());
                parameters.Add("priority_id", 3.ToString());

                responseList = manager.GetObjectList<Issue>(parameters);

                result.AddRange(responseList.Select(x => ToBugItem(x)));
                header.Subject = result.Last().Priority;

                header = new BugLogItem();
                result.Add(header);
                parameters = new NameValueCollection { };
                parameters.Add("assigned_to_id", idUser.ToString());
                parameters.Add("tracker_id", 1.ToString());
                parameters.Add("priority_id", 2.ToString());

                responseList = manager.GetObjectList<Issue>(parameters);

                result.AddRange(responseList.Select(x => ToBugItem(x)));
                header.Subject = result.Last().Priority;
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
    }
}
