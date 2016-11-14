using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Gamme
    {
        public Finition Finition { get; set; }
        public int Id { get; set; }
        public Isolant Isolant { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
    }
}