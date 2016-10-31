using RedmineLog.Common;
using RedmineLog.Common.Forms;
using RedmineLog.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.UI
{
    public partial class frmAbout : Form, ISetup
    {
        private IAppSettings settings;
        public frmAbout()
        {
            InitializeComponent();
            this.Initialize<About.IView, frmAbout>();

        }
        private void OnAboutLoad(object sender, EventArgs e)
        {
            this.SetupLocation(settings.Display, 50, -130);
        }

        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
        }
    }

    internal class AboutView : About.IView, IView<frmAbout>
    {
        public void Init(frmAbout inView)
        {
            inView.webBrowser1.Navigate(System.Configuration.ConfigurationManager.AppSettings["WebSiteUrl"]);
        }

        public void Load()
        {
        }
    }
}
