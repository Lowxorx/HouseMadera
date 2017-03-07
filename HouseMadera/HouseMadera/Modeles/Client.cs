using HouseMadera.DAL;
using HouseMadera.Modeles;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class Client : ISynchronizable
    {
        public int Id { get; set; }

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string Adresse3 { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public DateTime ? MiseAJour { get; set; }
        public DateTime ? Suppression { get; set; }
        public DateTime ? Creation { get; set; }
        public int StatutClient { get; set; }

        public List<Projet> Projets { get; set; }

        #region OVERRIDE
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Client cl = (Client)obj;
            return (Nom == cl.Nom) && (Prenom == cl.Prenom) && (Adresse1 == cl.Adresse1) && (Adresse2 == cl.Adresse2) && (Adresse3 == cl.Adresse3) && (CodePostal == cl.CodePostal) && (Ville == cl.Ville) && (Email == cl.Email) && (Telephone == cl.Telephone) && (Mobile == Mobile) && (StatutClient == cl.StatutClient);
        }

        public override string ToString()
        {
            return Prenom + " " + Nom + " de " + Ville;
        }
        #endregion

        #region SYNCHRONISATION

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.MiseAJour == null)
                return true;
            else
                return MiseAJour == modele.MiseAJour;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Teste si le modele existe en base
        /// </summary>
        /// <typeparam name="TMODELE"></typeparam>
        /// <param name="modele"></param>
        /// <returns>true si le modele a été supprimé sinon false</returns>
        public bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if(modele.Suppression != null)
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
        public void Copie(Client clientDistant)
        {
            Nom = clientDistant.Nom;
            Prenom = clientDistant.Prenom;
            Adresse1 = clientDistant.Adresse1;
            Adresse2 = clientDistant.Adresse2;
            Adresse3 = clientDistant.Adresse3;
            CodePostal = clientDistant.CodePostal;
            Ville = clientDistant.Ville;
            Email = clientDistant.Email;
            Mobile = clientDistant.Mobile;
            Telephone = clientDistant.Telephone;
            MiseAJour = clientDistant.MiseAJour;
            Creation = clientDistant.Creation;
            Suppression = clientDistant.Suppression;
            StatutClient = clientDistant.StatutClient;
        }

        #endregion
    }
}
