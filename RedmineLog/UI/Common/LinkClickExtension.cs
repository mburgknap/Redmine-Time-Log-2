using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI.Common
{
    public static class LinkClickExtension
    {
        public static void SetLinkMouseClick(this LinkLabel inThis, Action inLinkClick, Action<MouseEventArgs> inMouseClick)
        {
            var timer = new Timer();
            timer.Interval = 100;

            bool linkClicked = false;
            MouseEventArgs tmpArg = null;

            inThis.LinkClicked += (s, e) =>
            {
                linkClicked = true;
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();

                if (linkClicked)
                { inLinkClick(); }
                else
                { inMouseClick(tmpArg); }

                linkClicked = false;
            };

            inThis.MouseClick += (s, e) =>
            {
                linkClicked = false;
                tmpArg = e;
                timer.Start();
            };

        }
    }
}
