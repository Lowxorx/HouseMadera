using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
    public class Produit
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public Projet Projet { get; set; }
        public Devis Devis { get; set; }
        public Plan Plan { get; set; }
    }
}
