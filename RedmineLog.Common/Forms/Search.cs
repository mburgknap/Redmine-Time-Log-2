using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common.Forms
{
    public static class Search
    {
        public static class Events
        {
            public const string Load = "topic://Search/Load";
            public const string Search = "topic://Search/Search";
            public const string Clear = "topic://Search/Clear";
        }

        public interface IView
        {
        }

        public interface IModel
        {
            DataProperty<WorkingIssueList> Issues { get; }
            DataProperty<ProjectData> Project { get; }
            DataProperty<ProjectList> Projects { get; }
        }
    }
}
