using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Mysql.Entities
{
    public class TypeSlot
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Nom { get; set; }

        public virtual ICollection<Slot> Slots { get; set; }
        public virtual ICollection<TypeModule> TypesModules { get; set; }

    }
}