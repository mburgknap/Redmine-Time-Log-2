using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI.Common
{
    public static class NotifyBox
    {
        public static void Show(string inText, String inTitle)
        {
            var notifyIcon1 = new NotifyIcon();
            notifyIcon1.Icon = SystemIcons.Application;
            notifyIcon1.BalloonTipTitle = inTitle;
            notifyIcon1.BalloonTipText = inText;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(3000);
        }
    }
}
