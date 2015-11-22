using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog
{
    public class WebRedmine
    {
        private IRedmineClient redmine;

        [Inject]
        public WebRedmine(IRedmineClient inClient)
        {
            redmine = inClient;
        }


        [EventSubscription(WwwRedmineEvent.IssueAdd, typeof(OnPublisher))]
        public void OnIssueAddEvent(object sender, Args<int> arg)
        {

        }

        [EventSubscription(WwwRedmineEvent.IssueEdit, typeof(OnPublisher))]
        public void OnIssueEditEvent(object sender, Args<int> arg)
        {
            try
            {
                System.Diagnostics.Process.Start(redmine.IssueUrl(arg.Data) + "/edit");
            }
            catch (Exception ex)
            {
                Program.Kernel.Get<ILog>().Error("GoLink", ex, "Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

        [EventSubscription(WwwRedmineEvent.IssueShow, typeof(OnPublisher))]
        public void OnIssueShowEvent(object sender, Args<int> arg)
        {
            try
            {
                System.Diagnostics.Process.Start(redmine.IssueUrl(arg.Data));
            }
            catch (Exception ex)
            {
                Program.Kernel.Get<ILog>().Error("GoLink", ex, "Error occured, error detail saved in application logs ", "Warrnig");
            }
        }

    }
}
