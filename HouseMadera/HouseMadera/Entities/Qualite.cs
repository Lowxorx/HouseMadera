using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class Qualite
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<TypeIsolant> TypesIsolants { get; set; }
        public virtual ICollection<TypeFinition> TypesFinitions { get; set; }
    }
}