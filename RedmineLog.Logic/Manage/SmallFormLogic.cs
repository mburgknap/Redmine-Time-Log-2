using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Logic
{
    internal class SmallFormLogic : ILogic<Small.IView>
    {
        private Small.IModel model;
        private Small.IView view;
        private IRedmineClient redmine;

        [Inject]
        public SmallFormLogic(Small.IView inView, Small.IModel inModel, IRedmineClient inClient)
        {
            view = inView;
            model = inModel;
            redmine = inClient;
        }


        [EventSubscription(Small.Events.Load, typeof(Subscribe<Small.IView>))]
        public void OnLoadEvent(object sender, EventArgs arg)
        {
            if (!String.IsNullOrWhiteSpace(model.IssueInfo.Value.ToString()))
            {
                model.IssueUri.Value.Clear();
                model.IssueUri.Value.Append(redmine.IssueUrl(model.IssueInfo.Value.Id));
            }

            model.WorkTime.Update();
            model.IdleTime.Update();
            model.Comment.Update();
            model.IssueInfo.Update();
            model.IssueParentInfo.Update();
        }
    }
}
