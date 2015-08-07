using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI.Common
{
    public partial class ExFlowLayoutPanel : FlowLayoutPanel
    {
        public ExFlowLayoutPanel()
        {
            InitializeComponent();
            AutoScroll = true;
        }

        public ExFlowLayoutPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
