using HouseMadera.Modeles;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Clients
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Id du Client
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Client
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prenom du Client
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Adresse1 du Client
        /// </summary>
        public string Adresse1 { get; set; }

        /// <summary>
        /// Adresse2 du Client
        /// </summary>
        public string Adresse2 { get; set; }

        /// <summary>
        /// Adresse3 du Client
        /// </summary>
        public string Adresse3 { get; set; }

        /// <summary>
        /// Code postal du Client
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Ville  du Client
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Adresse email du Client
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Telephone du Client
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Mobile du Client
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Statut du Client
        /// </summary>
        public int StatutClient { get; set; }

        /// <summary>
        /// Liste des projets du Client
        /// </summary>
        public List<Projet> Projets { get; set; }
    }
}
