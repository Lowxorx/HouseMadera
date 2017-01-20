using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modèles
{
    public class ConnectivityMonitor
    {
        public bool IsOnline()
        {
            Ping myPing = new Ping();
            String host = "8.8.8.8";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
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
