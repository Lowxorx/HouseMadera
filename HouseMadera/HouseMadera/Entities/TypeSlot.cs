﻿using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class TypeSlot
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Slot> Slots { get; set; }
        public virtual ICollection<TypeModule> TypesModules { get; set; }

    }
}