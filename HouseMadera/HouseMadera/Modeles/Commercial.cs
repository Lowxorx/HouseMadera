using HouseMadera.DAL;
using HouseMadera.Modeles;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class Commercial:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        public override string ToString()
        {
            return string.Format("Commercial :  {0} {1}",Prenom,Nom);
        }

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
            Commercial commercial = modele as Commercial;
            Nom = commercial.Nom;
            Prenom = commercial.Prenom;
            Login = commercial.Login;
            Password = commercial.Password;
            MiseAJour = commercial.MiseAJour;
            Creation = commercial.Creation;
            Suppression = commercial.Suppression;
        }
    }

}
