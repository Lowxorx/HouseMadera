using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
    public class DevisGenere
    {
        public string Output { get; set; }
        public string PrixHT { get; set; }
        public string PrixTTC { get; set; }
        public string PrixTTCRemise { get; set; }
        public List<string> Modules { get; set; }
        public Client client { get; set; }
    }
}
