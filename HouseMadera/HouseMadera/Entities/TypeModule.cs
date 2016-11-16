using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeMadera.Entities
{
    public class TypeModule
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Nom { get; set; }

        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<TypeSlot> TypesSlots { get; set; }
    }
}