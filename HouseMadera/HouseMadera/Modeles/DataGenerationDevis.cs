using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
    public class DataGenerationDevis
    {
        public string NomProduit { get; set; }
        public int NumModule { get; set; }
        public string NomModule { get; set; }
        public string NomComposant { get; set; }
        public string PrixComposant { get; set; }
        public int NombreComposant { get; set; }
        public Client Client { get; set; }

    }
}
