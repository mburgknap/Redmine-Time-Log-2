namespace RedmineLog.Logic.Common
{
    public interface ILogic<TView>
    {
        void Apply(string inCmd);
    }
}