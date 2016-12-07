using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Mysql.Entities
{
    public class TypeIsolant
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Nom { get; set; }

        public virtual ICollection<Isolant> MyProperty { get; set; }
    }
}