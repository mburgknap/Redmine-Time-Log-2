using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RedmineLog.Common
{

    public enum SyncTarget
    {
        View,
        Source,
        All
    }
    public class ModelSync<TType> : IModelSync
    {
        private Dictionary<String, Action> viewSync = new Dictionary<string, Action>();
        private Dictionary<String, Action> sourceSync = new Dictionary<string, Action>();



        public void Value(SyncTarget mode, string property)
        {
            if (mode == SyncTarget.View)
                Invoke(viewSync, property, mode);

            if (mode == SyncTarget.Source)
                Invoke(sourceSync, property, mode);

            if (mode == SyncTarget.All)
            {
                Invoke(viewSync, property, mode);
                Invoke(sourceSync, property, mode);
            }
        }

        private void Invoke(Dictionary<string, Action> propSync, string property, SyncTarget mode)
        {
            Action action = null;

            propSync.TryGetValue(property, out action);

            if (action != null)
                action();
            else
                Debug.WriteLine("Property: " + property + "fo Mode: " + mode + " is not defined");
        }

        public void Bind(SyncTarget mode, object inObject)
        {
            var methods = inObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var prop in typeof(TType).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var method = methods.Where(x => x.Name == "On" + prop.Name + "Change").FirstOrDefault();

                if (method != null)
                {
                    Action action = (Action)Delegate.CreateDelegate(typeof(Action), inObject, method);

                    if (mode == SyncTarget.View)
                        viewSync.Add(prop.Name, action);
                    if (mode == SyncTarget.Source)
                        sourceSync.Add(prop.Name, action);
                    if (mode == SyncTarget.All)
                    {
                        viewSync.Add(prop.Name, action);
                        sourceSync.Add(prop.Name, action);
                    }
                }
            }
        }
    }

    public interface IModelSync
    {
        void Bind(SyncTarget mode, object inObject);
        void Value(SyncTarget mode, string property);
    }
}
