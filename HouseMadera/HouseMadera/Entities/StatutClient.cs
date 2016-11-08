using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMadera.Entities
{
    public class StatutClient
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
