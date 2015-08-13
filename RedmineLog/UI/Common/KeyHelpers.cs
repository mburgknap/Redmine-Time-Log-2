using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI.Common
{
    public static class KeyHelpers
    {
        public static void BindKey(Control inControl, Action<Object, KeyEventArgs> action)
        {
            if (inControl == null) return;

            inControl.KeyDown += (s, e) => { action(s, e); e.Handled = true; };

            foreach (var item in inControl.Controls)
            {
                KeyHelpers.BindKey(item as Control, action);
            }
        }

        public static void BindMouseClick(Control inControl, Action<Object> action)
        {
            if (inControl == null) return;

            inControl.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    action(s);
                    return;
                }
            };

            foreach (var item in inControl.Controls)
            {
                KeyHelpers.BindMouseClick(item as Control, action);
            }
        }

        public static void BindSpecialClick(Control inControl, Action<string, object> action)
        {
            if (inControl == null) return;

            inControl.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    SpecialAction(s, action);
                }
            };

            foreach (var item in inControl.Controls)
            {
                KeyHelpers.BindSpecialClick(item as Control, action);
            }
        }

        private static void SpecialAction(object s, Action<string, object> action)
        {
            if (s is ISpecialAction)
            {
                ((ISpecialAction)s).Show(action);
                return;
            }

            if (s is Control)
            {
                SpecialAction(((Control)s).Parent, action);
            }

        }

    }
}
