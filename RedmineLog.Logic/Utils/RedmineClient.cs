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
    }
}
