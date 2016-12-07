using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Mysql.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Nom { get; set; }
        public DateTime CreateDate { get; set; }

        public Gamme Gamme { get; set; }
        public CoupePrincipe CoupePrincipe { get; set; }

        public virtual ICollection<Produit> Produits { get; set; }
        public virtual ICollection<ModulePlace> ModulesPlaces { get; set; }
    }
}
