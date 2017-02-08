using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class TypeIsolant
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<Isolant> MyProperty { get; set; }
    }
}