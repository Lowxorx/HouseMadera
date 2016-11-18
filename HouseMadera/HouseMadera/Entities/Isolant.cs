using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class Isolant
    {
        public virtual ICollection<Gamme> Gammes { get; set; }
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public TypeIsolant TypeIsolant { get; set; }
    }
}