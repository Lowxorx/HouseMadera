using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Module
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public decimal Hauteur { get; set; }
        public decimal Largeur { get; set; }
        public Gamme Gamme { get; set; }
        public TypeModule TypeModule { get; set; }
        public virtual ICollection<SlotPlace> SlotsPlaces { get; set; }
        public virtual ICollection<Composant> Composants { get; set; }

    }
}