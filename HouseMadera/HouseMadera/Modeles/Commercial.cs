using HouseMadera.Modèles;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class Commercial
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Projet> Projets { get; set; }
    }

}
