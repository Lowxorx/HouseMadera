using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Composant
    {
        public int Id { get; set; }

        public virtual ICollection<Module> Modules { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }

        public TypeComposant TypeComposant { get; set; }
    }
}