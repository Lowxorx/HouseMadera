using System;
using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class ModulePlace
    {
        public int Id { get; set; }

        public Module Module { get; set; }
        public SlotPlace SlotPlace { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}