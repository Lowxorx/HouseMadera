using System;
using System.IO;
using System.Reflection;

namespace HouseMadera.Utilities
{
    public class AppInfo
    {
        public static String AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
