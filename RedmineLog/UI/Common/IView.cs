using System;
namespace RedmineLog.UI.Common
{
    internal interface IView<T> : IView
    {
        void Init(T inView);

    }

    internal interface IView
    {
        void Load();

    }

    interface ICustomItem
    {
        object Data { get; }
    }

    interface ISpecialAction
    {
        void Show(Action<string, object> inData);
    }
}