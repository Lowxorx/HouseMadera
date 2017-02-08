using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class TypeFinition
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }
        public virtual ICollection<Finition> Finitions { get; set; }
    }
}