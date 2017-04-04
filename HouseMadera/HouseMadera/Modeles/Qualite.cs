using HouseMadera.DAL;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class Qualite:ISynchronizable
    {
        /// <summary>
        /// Id de la Qualite
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de la Qualite
        /// </summary>
        public string Nom { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        public override string ToString()
        {
            return string.Format("Nom {0}", Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Qualite q = (Qualite)obj;

            return (Nom == q.Nom) && (Creation == q.Creation);
        }

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Qualite qualite = modele as Qualite;
            Nom = qualite.Nom;
            MiseAJour = qualite.MiseAJour;
            Creation = qualite.Creation;
            Suppression = qualite.Suppression;
        }

        public bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.Suppression != null && !Suppression.HasValue)
            {
                Suppression = modele.Suppression;
                return true;
            }

            return false;
        }

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.MiseAJour == null)
                return true;
            else
                return MiseAJour == modele.MiseAJour;
        }
    }
}