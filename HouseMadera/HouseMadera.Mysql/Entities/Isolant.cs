using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Mysql.Entities
{
    public class Isolant
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }
        public TypeIsolant TypeIsolant { get; set; }

        public virtual ICollection<Gamme> Gammes { get; set; }
    }
}