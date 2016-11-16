using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeMadera.Entities
{
    public class Composant
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }
        public decimal Prix { get; set; }

        public TypeComposant TypeComposant { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
    }
}