using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class TypeIsolant
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<Isolant> MyProperty { get; set; }
    }
}