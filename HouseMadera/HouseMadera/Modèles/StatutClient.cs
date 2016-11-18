using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class StatutClient
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
    }
}
