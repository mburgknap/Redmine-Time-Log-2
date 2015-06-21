﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{


    public static class Main
    {
        public enum Actions
        {
            Issue,
            Idle,
            All
        }

        public static class Events
        {
            public const string Link = "topic://Main/Link";
            public const string Load = "topic://Main/Load";
            public const string Exit = "topic://Main/Exit";
            public const string Reset = "topic://Main/Reset";
            public const string Submit = "topic://Main/Submit";
            public const string AddIssue = "topic://Main/AddIssue";
            public const string DelIssue = "topic://Main/DelIssue";
            public const string AddComment = "topic://Main/AddComment";
            public const string UpdateComment = "topic://Main/UpdateComment";
            public const string DelComment = "topic://Main/DelComment";
        }

        public interface IView
        {
            void Info(string p);

            void GoLink(Uri inUri);
        }

        public interface IModel
        {
            IModelSync Sync { get; }
            TimeSpan WorkTime { get; set; }
            TimeSpan IdleTime { get; set; }
            WorkActivityList WorkActivities { get; }
            WorkActivityType Activity { get; set; }
            CommentData Comment { get; set; }
            IssueData Issue { get; set; }
            IssueCommentList IssueComments { get; }
            RedmineIssueData IssueParentInfo { get; set; }
            RedmineIssueData IssueInfo { get; set; }
        }
    }
}
