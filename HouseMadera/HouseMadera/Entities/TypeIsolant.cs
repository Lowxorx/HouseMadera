using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class TypeIsolant
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Isolant> MyProperty { get; set; }
    }
}