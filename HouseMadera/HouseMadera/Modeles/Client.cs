using HouseMadera.DAL;
using HouseMadera.Modeles;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
    public class Client : ISynchronizable
    /// <summary>
    /// Classe représentant les Clients
    /// </summary>
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
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }
        public int StatutClient { get; set; }

        /// <summary>
        /// Liste des projets du Client
        /// </summary>
        public List<Projet> Projets { get; set; }

        #region OVERRIDE
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Client cl = (Client)obj;

            return ((Nom == cl.Nom) && (Prenom == cl.Prenom) && (Email == cl.Email)) && (Creation == cl.Creation);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Prenom + " " + Nom + " de " + Ville;
        }
        #endregion

        #region SYNCHRONISATION

        /// <summary>
        /// Teste si le modele a été mis à jour
        /// </summary>
        /// <typeparam name="TMODELE"></typeparam>
        /// <param name="modele"></param>
        /// <returns>true si le modele a été modifié sinon false</returns>
        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.MiseAJour == null)
                return true;
            else
                return MiseAJour == modele.MiseAJour;
        }

       

        /// <summary>
        /// Teste si le modele existe en base
        /// </summary>
        /// <typeparam name="TMODELE"></typeparam>
        /// <param name="modele"></param>
        /// <returns>true si le modele a été supprimé sinon false</returns>
        public bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {

            if (modele.Suppression != null && !Suppression.HasValue)
            {
                Suppression = modele.Suppression;
                return true;
            }

            return false;
        }

        /// <summary>
        /// recopie les valeurs des propriétés de l'objet passé en paramètre dans les propriétés de l'instance courante
        /// </summary>
        /// <param name="clientDistant"></param>
        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Client client = modele as Client;
            Nom = client.Nom;
            Prenom = client.Prenom;
            Adresse1 = client.Adresse1;
            Adresse2 = client.Adresse2;
            Adresse3 = client.Adresse3;
            CodePostal = client.CodePostal;
            Ville = client.Ville;
            Email = client.Email;
            Mobile = client.Mobile;
            Telephone = client.Telephone;
            MiseAJour = client.MiseAJour;
            Creation = client.Creation;
            Suppression = client.Suppression;
            StatutClient = client.StatutClient;
        }
        #endregion
    }

}
