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
            SetupScroll();
            Focus();
        }

        private void SetupScroll()
        {
            HorizontalScroll.Maximum = 0;
            AutoScroll = false;
            VerticalScroll.Visible = false;
            AutoScroll = true;
        }


        public ExFlowLayoutPanel(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            SetupScroll();
            Focus();
        }


        protected override Control.ControlCollection CreateControlsInstance()
        {
            return new ListControls(this);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down) return true;
            if (keyData == Keys.Left || keyData == Keys.Right) return true;
            return base.IsInputKey(keyData);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.Invalidate();
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            this.Invalidate();
            base.OnLeave(e);
        }


        class ListControls : Control.ControlCollection
        {

            public ListControls(ExFlowLayoutPanel owner)
                : base(owner)
            {

                ((ExFlowLayoutPanel)Owner).Focus();
            }

            private void Focus()
            {
                if (!((ExFlowLayoutPanel)Owner).Focused)
                {
                    ((ExFlowLayoutPanel)Owner).Focus();
                }
            }
            void ItemGotFocus(object sender, EventArgs e)
            {
                Focus();
            }


            public override void Add(Control value)
            {
                base.Add(value);
                Focus();
                value.GotFocus += ItemGotFocus;
            }

            public override void Remove(Control value)
            {
                value.GotFocus -= ItemGotFocus;
                base.Remove(value);
                Focus();
            }

            public override void Clear()
            {
                base.Clear(); Focus();
            }
        }
    }
}
