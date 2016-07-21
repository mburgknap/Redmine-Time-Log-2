using RedmineLog.UI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Utils
{
    class AppStart
    {
        private FileInfo importApp;
        private DirectoryInfo appDir;
        internal bool IsFirstStart()
        {
            appDir = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());

            return appDir.EnumerateDirectories("Database", SearchOption.TopDirectoryOnly).FirstOrDefault() == null;
        }

        internal void ImportData()
        {
            CopyFilesRecursively(new DirectoryInfo(Path.Combine(importApp.Directory.FullName, "Database")), appDir);
            NotifyBox.Show("Import sucessful", "RedmineLog");
        }

        internal bool FindDataToImport()
        {
            var apps = Process.GetProcessesByName("RedmineLog").Select(app => new FileInfo(app.MainModule.FileName));

            var importApps = apps.Where(app => app.Directory.FullName != System.IO.Directory.GetCurrentDirectory()).ToList();

            importApp = importApps.FirstOrDefault();

            return importApp != null && importApp.Directory.GetDirectories("Database").Count() > 0;
        }

        private static DirectoryInfo CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            var newDirectoryInfo = target.CreateSubdirectory(source.Name);
            foreach (var fileInfo in source.GetFiles())
                fileInfo.CopyTo(Path.Combine(newDirectoryInfo.FullName, fileInfo.Name));

            foreach (var childDirectoryInfo in source.GetDirectories())
                CopyFilesRecursively(childDirectoryInfo, newDirectoryInfo);

            return newDirectoryInfo;
        }
    }
}
