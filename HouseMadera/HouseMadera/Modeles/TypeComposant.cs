using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class TypeComposant
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }
           public virtual ICollection<Composant> Composants { get; set; }
    }
}
