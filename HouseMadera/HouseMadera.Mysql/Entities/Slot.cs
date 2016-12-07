using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Mysql.Entities
{
    public class Slot
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Nom { get; set; }
        public decimal Hauteur { get; set; }
        public decimal Largeur { get; set; }

        public TypeSlot TypeSlot { get; set; }

        public virtual ICollection<SlotPlace> SlotsPlaces { get; set; }
    }
}