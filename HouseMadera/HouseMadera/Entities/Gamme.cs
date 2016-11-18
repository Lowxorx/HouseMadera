using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class Gamme
    {
        public Finition Finition { get; set; }
        public int Id { get; set; }
        public Isolant Isolant { get; set; }

        public virtual ICollection<Module> Modules { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}