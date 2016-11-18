using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class StatutClient
    {
        public virtual ICollection<Client> Clients { get; set; }
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }
    }
}