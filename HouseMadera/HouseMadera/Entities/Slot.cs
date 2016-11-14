using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class Slot
    {
        public decimal Hauteur { get; set; }
        public int Id { get; set; }

        public decimal Largeur { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<SlotPlace> SlotsPlaces { get; set; }
        public TypeSlot TypeSlot { get; set; }
    }
}