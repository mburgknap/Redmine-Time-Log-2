using Ninject;
using RedmineLog.Logic.Common;

namespace RedmineLog.UI.Common
{
    internal static class FormExtensions
    {
        //public static void Initialize(this Form inThis, object sender, EventArgs arg = null)
        public static void Initialize<TView, TWinForm>(this TWinForm inThis)
        {
            Program.Kernel.Get<ILogic<TView>>();

            var view = Program.Kernel.Get<TView>() as IView<TWinForm>;
            if (view != null)
                view.Init(inThis);
        }
    }
}