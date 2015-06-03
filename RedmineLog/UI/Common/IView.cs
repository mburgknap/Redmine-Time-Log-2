namespace RedmineLog.UI.Common
{
    internal interface IView<T>
    {
        void Init(T inView);
    }
}