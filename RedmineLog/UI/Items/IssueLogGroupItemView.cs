using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI.Items
{
    public partial class IssueLogGroupItemView : UserControl
    {
        public IssueLogGroupItemView()
        {
            InitializeComponent();
        }

        internal Control Set(string project)
        {
            label1.Text = !string.IsNullOrWhiteSpace(project) ? project : " Global issue ";
            return this;
        }
    }
}
