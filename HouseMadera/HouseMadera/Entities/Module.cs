using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class Module
    {
        public virtual ICollection<Composant> Composants { get; set; }
        public Gamme Gamme { get; set; }
        public decimal Hauteur { get; set; }
        public int Id { get; set; }
        public decimal Largeur { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public virtual ICollection<SlotPlace> SlotsPlaces { get; set; }
        public TypeModule TypeModule { get; set; }
    }
}