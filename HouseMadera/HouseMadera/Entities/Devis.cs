using System;
using System.Collections.Generic;

namespace HouseMadera.Entities
{
    public class Devis
    {
        public DateTime DateCreation { get; set; }
        public int Id { get; set; }
        public string Nom { get; set; }
        public Decimal PrixHT { get; set; }
        public Decimal PrixTTC { get; set; }

        public virtual ICollection<Produit> Produits { get; set; }
        public StatutDevis StatutDevis { get; set; }
    }
}