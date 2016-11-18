using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class Qualite
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public virtual ICollection<TypeFinition> TypesFinitions { get; set; }
        public virtual ICollection<TypeIsolant> TypesIsolants { get; set; }
    }
}