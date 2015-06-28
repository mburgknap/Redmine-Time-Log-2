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
}