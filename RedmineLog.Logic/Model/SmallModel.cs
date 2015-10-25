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
            Sync = new ModelSync<Small.IModel>();
        }
        public IModelSync Sync { get; private set; }
        public TimeSpan WorkTime { get { return Model.WorkTime; } }
        public TimeSpan IdleTime { get { return Model.IdleTime; } }
        public CommentData Comment{ get { return Model.Comment; } }
        public RedmineIssueData IssueParentInfo { get { return Model.IssueParentInfo; } }
        public RedmineIssueData IssueInfo { get { return Model.IssueInfo; } }


        public string IssueUri { get; set; }
    }
}
