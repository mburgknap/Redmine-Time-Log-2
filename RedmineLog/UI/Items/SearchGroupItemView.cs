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
    public partial class SearchGroupItemView : UserControl
    {
        public SearchGroupItemView()
        {
            InitializeComponent();
        }

        internal Control Set(string project)
        {
            label1.Text = !string.IsNullOrWhiteSpace(project) ? project : " Global bug ";
            return this;
        }
    }
}
