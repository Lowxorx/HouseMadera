using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Mysql.Entities
{
    public class Commercial
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Nom { get; set; }
        [StringLength(255)]
        public string Prenom { get; set; }
        [StringLength(255)]
        public string Login { get; set; }
        [StringLength(255)]
        public string Password { get; set; }

        public virtual ICollection<Projet> Projets { get; set; }
    }
}
