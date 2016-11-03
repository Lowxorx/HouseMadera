﻿using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class Slot
    {
        public int Id { get; set; }

        public string Nom { get; set; }
        public decimal Hauteur { get; set; }
        public decimal Largeur { get; set; }

        public TypeSlot TypeSlot { get; set; }

        public virtual ICollection<SlotPlace> SlotsPlaces { get; set; }
    }
}