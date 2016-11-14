using System;
using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Plan
    {
        public CoupePrincipe CoupePrincipe { get; set; }
        public DateTime CreateDate { get; set; }
        public Gamme Gamme { get; set; }
        public int Id { get; set; }

        public virtual ICollection<ModulePlace> ModulesPlaces { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<Produit> Produits { get; set; }
    }
}