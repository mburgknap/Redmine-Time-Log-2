using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Appccelerate.EventBroker.Matchers;
using System;

namespace RedmineLog.Common
{

    public class TopicMatcher : IPublicationMatcher
    {

        public void DescribeTo(System.IO.TextWriter writer)
        {
            throw new NotImplementedException();
        }

        public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(publication.ToString());
            System.Diagnostics.Debug.WriteLine(subscription.ToString());

            return false;
        }
    }


    public class TopicPublisher : OnPublisher
    {

        public override void Handle(IEventTopicInfo eventTopic, object subscriber, object sender, EventArgs e, Appccelerate.EventBroker.Internals.Subscriptions.IDelegateWrapper delegateWrapper)
        {
            base.Handle(eventTopic, subscriber, sender, e, delegateWrapper);
        }

    }

    public abstract class MyPublisher : IPublicationMatcher
    {
        public abstract Type View { get; }

        public void DescribeTo(System.IO.TextWriter writer)
        {
        }

        public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
        {
            if (subscription.EventArgsType.IsAssignableFrom(e.GetType())
                && subscription.Handler is MySubscriber)
            {
                foreach (var item in publication.PublicationMatchers)
                {
                    if (item is MyPublisher)
                    {
                        var val = ((MyPublisher)item).View == (((MySubscriber)subscription.Handler).View);
                        return val;
                    }
                }
            }
            return false;
        }
    }

    public class Publish<T> : MyPublisher
    {
        public override Type View { get { return typeof(T); } }
    }

    public abstract class MySubscriber : OnPublisher
    {
        public abstract Type View { get; }
    }

    public class Subscribe<T> : MySubscriber
    {
        public override Type View { get { return typeof(T); } }
    }
}