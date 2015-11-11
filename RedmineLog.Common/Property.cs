using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{


    public static class PropertyExtension
    {
        public static void Update(this DataProperty<StringBuilder> inThis, String inValue)
        {
            inThis.Value.Init(inValue);
            inThis.Update();
        }

        public static void Notify(this DataProperty<StringBuilder> inThis, String inValue)
        {
            inThis.Value.Init(inValue);
            inThis.Notify();
        }
    }

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
    public enum ActionType
    {
        /// <summary>
        /// Set property value
        /// </summary>
        Set,
        /// <summary>
        /// Notify about value changes by view
        /// </summary>
        Notify,
        /// <summary>
        /// Notify view about value changes
        /// </summary>
        Update
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
        }

        public void Update()
        {
            Invoke(Value, ActionType.Update);
        }

        public void Update(T inValue)
        {
            Invoke(inValue, ActionType.Set, ActionType.Update);
        }

        public void Notify()
        {
            Invoke(Value, ActionType.Notify);
        }

        public void Notify(T inValue)
        {
            Invoke(inValue, ActionType.Set, ActionType.Notify);
        }

        public void Invoke(T inValue, params ActionType[] inActions)
        {
            foreach (var action in inActions)
            {
                switch (action)
                {
                    case ActionType.Update:
                        onViewUpdate.OnNext(Value); break;
                    case ActionType.Notify:
                        onViewChanged.OnNext(Value); break;
                    case ActionType.Set:
                        Value = inValue; break;
                }
            }
        }

    }

}
