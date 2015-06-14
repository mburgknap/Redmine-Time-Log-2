﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class Search
    {
        public static class Events
        {
            public const string Load = "topic://Search/Load";
            public const string Select = "topic://Search/Select";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            IModelSync Sync { get; }

            WorkingIssueList Issues { get; }
        }
    }
}
