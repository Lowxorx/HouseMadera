using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Gamme
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Finition Finition { get; set; }
        public Isolant Isolant { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
    }
}
