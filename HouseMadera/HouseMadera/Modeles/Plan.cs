using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class Plan
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime CreateDate { get; set; }
        public Gamme Gamme { get; set; }
        public CoupePrincipe CoupePrincipe { get; set; }
        public virtual ICollection<Produit> Produits { get; set; }
        public virtual ICollection<ModulePlace> ModulesPlaces { get; set; }
    }
}
