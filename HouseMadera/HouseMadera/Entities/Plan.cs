using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class Plan
    {
        public CoupePrincipe CoupePrincipe { get; set; }
        public DateTime CreateDate { get; set; }
        public Gamme Gamme { get; set; }
        public int Id { get; set; }
        public virtual ICollection<ModulePlace> ModulesPlaces { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public virtual ICollection<Produit> Produits { get; set; }
    }
}