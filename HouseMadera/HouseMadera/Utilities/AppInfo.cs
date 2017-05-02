using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HouseMadera.Utilities
{
    public class AppInfo
    {
        public static string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string Version { get; set; }
        public DateTime Date { get; set; }

        public AppInfo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Date = File.GetCreationTime(assembly.Location);
            Version = fvi.FileVersion;
        }

        public override string ToString()
        {
            return string.Format("Version {0} - {1:d}", Version, Date);
        }
    }
}
