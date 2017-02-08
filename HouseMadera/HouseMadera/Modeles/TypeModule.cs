using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class TypeModule
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<TypeSlot> TypesSlots { get; set; }
    }
}