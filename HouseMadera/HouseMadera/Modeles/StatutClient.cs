using HouseMadera.Modeles;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class StatutClient
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
    }
}
