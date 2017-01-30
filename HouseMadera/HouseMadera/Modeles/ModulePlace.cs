using System;
using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class ModulePlace
    {
        public int Id { get; set; }

        public Module Module { get; set; }
        public SlotPlace SlotPlace { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}