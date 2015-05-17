using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ninject;
using Ninject.Modules;
using RedmineLog.Logic.Common;

namespace RedmineLog.UI.Common
{
    static class FormExtensions
    {
        //public static void Initialize(this Form inThis, object sender, EventArgs arg = null)
        public static void Initialize<TView, TWinForm>(this TWinForm inThis)
        {
            App.Kernel.Get<ILogic<TView>>();

            var view = App.Kernel.Get<TView>() as IView<TWinForm>;
            if (view != null)
                view.Init(inThis);
        }
    }
}
