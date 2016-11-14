using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class SlotPlace
    {
        public int Abscisse { get; set; }
        public int Id { get; set; }
        public Module Module { get; set; }
        public virtual ICollection<ModulePlace> ModulesPlaces { get; set; }
        public int Ordonnee { get; set; }
        public Slot Slot { get; set; }
    }
}