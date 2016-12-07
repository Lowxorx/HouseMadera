using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Mysql.Entities
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