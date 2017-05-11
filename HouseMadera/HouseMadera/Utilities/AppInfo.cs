using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HouseMadera.Utilities
{
    public class AppInfo
    {

        /// <summary>
        /// Le chemin vers l'application
        /// </summary>
        public static string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        /// <summary>
        /// Le numéro de version ex 1.0.0.0
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Contient la date de création de l'assembly
        /// </summary>
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
