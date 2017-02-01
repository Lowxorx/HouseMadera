using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Utilites
{
    public class ConnectivityMonitor
    {
        public bool IsOnline()
        {
            Ping myPing = new Ping();
            var host = "8.8.4.4";
            int timeout = 1000;
            PingReply reply = myPing.Send(host, timeout);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("online");
                ConnectivityStatus = true;
                return true;
            }
            else
            {
                Console.WriteLine("offline");
                ConnectivityStatus = false;
                return false;
            }
        }

        public static bool ConnectivityStatus { get; set; }

    }
}
