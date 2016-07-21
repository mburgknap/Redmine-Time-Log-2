using Ninject;
using RedmineLog.Common;
using RedmineLog.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedmineLog.Utils
{

    class VersionInfo
    {
        public Version AppVersion;
        public string DownloadUrl;
    }

    class AppUpdater : IUpdater
    {
        private ILog log;

        [Inject]
        public AppUpdater(ILog inLog)
        {
            log = inLog;
        }

        public void CheckVersion()
        {

            Task<VersionInfo>.Run(() =>
            {

                try
                {
                    var filename = "version.txt";

                    using (var client = new WebClient())
                    {
                        File.Delete(filename);
                        client.DownloadFile(System.Configuration.ConfigurationManager.AppSettings["UpdateInfo"], filename);

                        var data = File.ReadAllText(filename).Split(';');

                        var onlineVersion = new Version(data[0]);
                        var url = data[1];

                        var appVersion = this.GetType().Assembly.GetName().Version;

                        if (appVersion.CompareTo(onlineVersion) < 0)
                        {
                            return new VersionInfo() { AppVersion = onlineVersion, DownloadUrl = url };
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    log.Error("Check Version", ex);
                }

                return null;

            })
            .ContinueWith(info =>
            {

                if (info == null)
                    return;

                try
                {
                    if (MessageBox.Show("New version availible " + info.Result.AppVersion
                               + Environment.NewLine
                               + "Do you want download it ?", "RedmineLog", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {


                        Task.Run(() =>
                        {
                            try
                            {
                                var file = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RedmineLog.exe"));
                                if (file.Exists)
                                    file.Delete();
                                using (var client = new WebClient())
                                {
                                    client.DownloadFile(info.Result.DownloadUrl, file.FullName);
                                }
                                Process.Start(file.FullName);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                log.Error("Download Setup", ex);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    log.Error("Ask Update", ex);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

    }
}
