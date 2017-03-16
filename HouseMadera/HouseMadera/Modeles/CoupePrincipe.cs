using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class CoupePrincipe:ISynchronizable
    {
       
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        public override string ToString()
        {
            return string.Format("Id : {0} Nom : {1}", Id, Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            CoupePrincipe c = (CoupePrincipe)obj;

            return (Nom == c.Nom) && (Creation == c.Creation);
        }

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            CoupePrincipe coupePrincipe = modele as CoupePrincipe;
            Nom = coupePrincipe.Nom;
            MiseAJour = coupePrincipe.MiseAJour;
            Creation = coupePrincipe.Creation;
            Suppression = coupePrincipe.Suppression;
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