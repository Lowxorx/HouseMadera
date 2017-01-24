using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
    public class StatutDevis
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Devis> Devis { get; set; }
    }
}
