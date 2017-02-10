using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Utilities
{
    public class Voie
    {
        public List<string> Libelles { get; set; }
        public Voie(string pays)
        {
            switch (pays)
            {
                case "FR":
                    Libelles = new List<string>() { "lieu-dit", "chemin", "rue", "boulevard", "avenue", "impasse","place","route" };
                    break;
                case "GB":
                    Libelles = new List<string>() { "street", "boulevard", "avenue" };
                    break;

            }

        }
    }
}
