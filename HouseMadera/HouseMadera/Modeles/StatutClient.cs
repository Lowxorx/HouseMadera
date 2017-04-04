using HouseMadera.DAL;
using HouseMadera.Modeles;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class StatutClient:ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }

        #region OVERRIDE

        public override string ToString()
        {
            return string.Format("Nom {0} ", Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            StatutClient s = (StatutClient)obj;

            return (Nom == s.Nom) && (Creation == s.Creation);
        }

        #endregion

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.MiseAJour == null)
                return true;
            else
                return MiseAJour == modele.MiseAJour;
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

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            StatutClient statutClient = modele as StatutClient;
            Nom = statutClient.Nom;
            MiseAJour = statutClient.MiseAJour;
            Creation = statutClient.Creation;
            Suppression = statutClient.Suppression;
        }


    }
}
