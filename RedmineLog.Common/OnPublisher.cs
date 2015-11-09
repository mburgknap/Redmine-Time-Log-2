using Appccelerate.EventBroker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public class OnPublisher : Appccelerate.EventBroker.Handlers.OnPublisher,
                               Appccelerate.EventBroker.Matchers.IPublicationMatcher
    {
        public void DescribeTo(TextWriter writer)
        {
          
        }

        public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
        {
            return true;
        }
    }
}
