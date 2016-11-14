using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class StatutDevis
    {
        public virtual ICollection<Devis> Devis { get; set; }
        public int Id { get; set; }
        public string Nom { get; set; }
    }
}