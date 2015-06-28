using Ninject;
using RedmineLog.Logic.Common;
using System;
using System.Windows.Forms;

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

        public static void Set<C, D>(this C inUI, D inData, Action<C, D> inAction) where C : Control
        {
            if (inUI.InvokeRequired)
            {
                inUI.Invoke(new MethodInvoker(() => Set(inUI, inData, inAction)));
                return;
            }
            inAction(inUI, inData);
        }
    }
}