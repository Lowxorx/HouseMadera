using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
    public class TypeComposant
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }

        public virtual ICollection<Composant> Composants { get; set; }
    }
}
