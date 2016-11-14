using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class Commercial
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nom { get; set; }
        public string Password { get; set; }
        public string Prenom { get; set; }
        public virtual ICollection<Projet> Projets { get; set; }
    }
}