using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class TypeFinition
    {
        public virtual ICollection<Finition> Finitions { get; set; }
        public int Id { get; set; }
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }
    }
}