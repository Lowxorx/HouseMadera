using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class StatutDevis
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<Devis> Devis { get; set; }
    }
}
