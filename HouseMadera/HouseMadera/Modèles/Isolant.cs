using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Isolant
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public TypeIsolant TypeIsolant { get; set; }
        public virtual ICollection<Gamme> Gammes { get; set; }
    }
}