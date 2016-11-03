using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMadera.Entities
{
    public class Devis
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime DateCreation { get; set; }
        public Decimal PrixHT { get; set; }
        public Decimal PrixTTC { get; set; }

        public StatutDevis StatutDevis { get; set; }
        public virtual ICollection<Produit> Produits { get; set; }
    }
}
