using System;
using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class Projet
    {
        public Client Client { get; set; }
        public Commercial Commercial { get; set; }
        public DateTime CreateDate { get; set; }
        public int Id { get; set; }

        public string Nom { get; set; }
        public virtual ICollection<Produit> Produits { get; set; }
        public string Reference { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}