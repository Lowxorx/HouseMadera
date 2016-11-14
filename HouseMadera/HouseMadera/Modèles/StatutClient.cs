using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class StatutClient
    {
        public virtual ICollection<Client> Clients { get; set; }
        public int Id { get; set; }
        public string Nom { get; set; }
    }
}