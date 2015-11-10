using Ninject;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic.Model
{
    internal class SmallModel : Small.IModel
    {
        private Main.IModel Model;
        [Inject]
        public SmallModel(Main.IModel inModel)
        {
            Model = inModel;
            IssueUri = new DataProperty<StringBuilder>();
        }

        public DataProperty<TimeSpan> WorkTime { get { return Model.WorkTime; } }
        public DataProperty<TimeSpan> IdleTime { get { return Model.IdleTime; } }
        public DataProperty<CommentData> Comment { get { return Model.Comment; } }
        public DataProperty<RedmineIssueData> IssueParentInfo { get { return Model.IssueParentInfo; } }
        public DataProperty<RedmineIssueData> IssueInfo { get { return Model.IssueInfo; } }
        public DataProperty<StringBuilder> IssueUri { get; private set; }
    }
}
