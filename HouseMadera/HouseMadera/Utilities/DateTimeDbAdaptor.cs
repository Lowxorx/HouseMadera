using System;


namespace HouseMadera.Utilities
{

    public class DateTimeDbAdaptor
    {
        public static string FormatDateTime(DateTime ? datetime, string bdd)
        {
            if (datetime == null)
                return bdd == "MYSQL" ? null:string.Empty ;

            DateTime value = datetime.Value ;
            string dateTimeFormat = string.Empty;
            switch (bdd)
            {
                case "SQLITE":
                    dateTimeFormat = "{0: yyyy-MM-dd HH:mm:ss.fff}";
                    break;
                case "MYSQL":
                    dateTimeFormat = "{0: yyyy-MM-dd HH:mm:ss}";
                    break;
            }
            
            //return string.Format(dateTimeFormat, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
            return string.Format(dateTimeFormat, value);
        }

        public static DateTime? InitialiserDate(string value)
        {
            DateTime maj = new DateTime();
            if (DateTime.TryParse(value, out maj))
                return maj;
            else
                return null;
        }
    }

}
