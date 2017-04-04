using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace HouseMadera.Utilities
{
    internal class Logger
    {
        private static bool IsEnabled = true;
        private static string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string logPath = appPath + @"\ex.log";

        private static string EncodeIso(string s)
        {
            // Encodage de la chaine de caractères en iso
            byte[] bytes = Encoding.Default.GetBytes(s);
            s = Encoding.GetEncoding("iso-8859-1").GetString(bytes);
            return s;
        }

        public static void WriteTrace(string sE)
        {
            if (IsEnabled)
            {
                string s = EncodeIso(sE);
                try
                {
                    if (File.Exists(logPath))
                        using (
                            StreamWriter sw = new StreamWriter(File.Open(logPath, FileMode.Append),
                                Encoding.GetEncoding("iso-8859-1")))
                        {
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + s);
                        }
                    else
                        using (
                            StreamWriter sw = new StreamWriter(File.Open(logPath, FileMode.Create),
                                Encoding.GetEncoding("iso-8859-1")))
                        {
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + s);
                        }
                }
                catch (Exception)
                {
                    // Impossible de log l'erreur
                }
            }
            else
            {
                Console.WriteLine(@"log désactivé");
            }
        }

        public static void WriteEx(Exception e)
        {
            if (IsEnabled)
            {
                try
                {
                    if (File.Exists(logPath))
                        using (
                            StreamWriter sw = new StreamWriter(File.Open(logPath, FileMode.Append),
                                Encoding.GetEncoding("iso-8859-1")))
                        {
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " +
                                         "-----------------------------------------------------------------------------------");
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.Message);
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.Source);
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.StackTrace);
                            if (e.InnerException == null) return;
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.InnerException.Message);
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.InnerException.Source);
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.InnerException.StackTrace);
                        }
                    else
                        using (
                            StreamWriter sw = new StreamWriter(File.Open(logPath, FileMode.Create),
                                Encoding.GetEncoding("iso-8859-1")))
                        {
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " +
                                         "-----------------------------------------------------------------------------------");
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.Message);
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.Source);
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.StackTrace);
                            if (e.InnerException == null) return;
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.InnerException.Message);
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.InnerException.Source);
                            sw.WriteLine(DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() +
                                         " - " + e.InnerException.StackTrace);
                        }
                }
                catch (Exception)
                {
                    // Impossible de log l'erreur
                }
            }
            else
            {
                Console.WriteLine(@"log désactivé");
            }
        }
    }
}