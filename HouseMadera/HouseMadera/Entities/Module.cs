using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeMadera.Entities
{
    public class Module
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Nom { get; set; }
        public decimal Hauteur { get; set; }
        public decimal Largeur { get; set; }

        public Gamme Gamme { get; set; }
        public TypeModule TypeModule { get; set; }

        public virtual ICollection<SlotPlace> SlotsPlaces { get; set; }
        public virtual ICollection<Composant> Composants { get; set; }

    }
}