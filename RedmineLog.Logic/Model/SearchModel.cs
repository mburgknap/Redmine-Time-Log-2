using Ninject;
using RedmineLog.Common;
using RedmineLog.Common.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    internal class SearchModel : Search.IModel
    {
        [Inject]
        public SearchModel()
        {
            Project = new DataProperty<ProjectData>();
            Projects = new DataProperty<ProjectList>();
            Issues = new DataProperty<WorkingIssueList>();
        }

        public DataProperty<WorkingIssueList> Issues { get; set; }
        public DataProperty<ProjectData> Project { get; private set; }

        public DataProperty<ProjectList> Projects { get; private set; }
    }
}
