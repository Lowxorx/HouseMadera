using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class Produit
    {
        public Devis Devis { get; set; }
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public Plan Plan { get; set; }
        public Projet Projet { get; set; }
    }
}