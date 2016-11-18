using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class TypeFinition
    {
        public virtual ICollection<Finition> Finitions { get; set; }
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public Qualite Qualite { get; set; }
    }
}