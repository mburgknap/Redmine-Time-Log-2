using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public class EventProperty<T> : IObservable<EventPattern<T>>, IDisposable
        where T : EventArgs
    {
        private List<IDisposable> subscriptions = new List<IDisposable>();
        private IObservable<EventPattern<T>> events;

        public EventProperty<T> Build(IObservable<EventPattern<T>> inEvent)
        {
            Dispose();
            events = inEvent;

            return this;
        }

        public void Dispose()
        {
            if (subscriptions != null)
            {
                subscriptions.ForEach(x => x.Dispose());
                subscriptions.Clear();
            }
        }

        public IDisposable Subscribe(IObserver<EventPattern<T>> observer)
        {
            subscriptions.Add(events.Subscribe(observer));
            return subscriptions.Last();
        }
    }

    public class DataProperty<T> where T : new()
    {
        public T Value { get; private set; }
        public IObservable<T> OnNotify { get { return onViewChanged; } }
        public IObservable<T> OnUpdate { get { return onViewUpdate; } }

        private Subject<T> onViewChanged = new Subject<T>();
        private Subject<T> onViewUpdate = new Subject<T>();

        public DataProperty()
        {
            Value = new T();

            onViewChanged.Subscribe(x =>
            {
                Value = x;
            });

            onViewUpdate.Subscribe(x =>
            {
                Value = x;
            });
        }
        public void Update(bool inNotifyInvoke = true)
        {
            if (inNotifyInvoke)
            {
                onViewUpdate.OnNext(Value);
            }
        }

        public void Update(T inValue, bool inNotifyInvoke = true)
        {
            onViewChanged.OnNext(inValue);

            Update(inNotifyInvoke);
        }

        public void Notify(bool inUpdateInvoke = true)
        {
            if (inUpdateInvoke)
            {
                onViewChanged.OnNext(Value);
            }
        }

        public void Notify(T inValue, bool inUpdateInvoke = true)
        {
            onViewUpdate.OnNext(inValue);

            Notify(inUpdateInvoke);
        }
    }

}
