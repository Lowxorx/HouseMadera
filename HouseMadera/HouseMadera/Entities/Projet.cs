using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class Projet
    {
        public Client Client { get; set; }
        public Commercial Commercial { get; set; }
        public DateTime CreateDate { get; set; }
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public virtual ICollection<Produit> Produits { get; set; }
        public string Reference { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}