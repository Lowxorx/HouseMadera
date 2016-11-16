using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMadera.Entities
{
    public class Client
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }
        [StringLength(255)]
        public string Prenom { get; set; }
        [StringLength(255)]
        public string Adresse1 { get; set; }
        [StringLength(255)]
        public string Adresse2 { get; set; }
        [StringLength(255)]
        public string Adresse3 { get; set; }
        [StringLength(20)]
        public string CodePostal { get; set; }
        [StringLength(255)]
        public string Ville { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(15)]
        public string Telephone { get; set; }
        [StringLength(15)]
        public string Mobile { get; set; }

        public StatutClient StatutClient { get; set; }

        public virtual  ICollection<Projet> Projets { get; set; }
    }
}
