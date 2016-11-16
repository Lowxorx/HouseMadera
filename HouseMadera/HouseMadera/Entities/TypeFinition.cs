using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeMadera.Entities
{
    public class TypeFinition
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }

        public virtual ICollection<Finition> Finitions { get; set; }
    }
}