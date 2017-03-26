using HouseMadera.DAL;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class Gamme:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Finition Finition { get; set; }
        public Isolant Isolant { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        #region OVERRIDE

        public override string ToString()
        {
            return string.Format("Nom {0} , Finition {1} , Isolant {2}", Nom, Finition.Nom, Isolant.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Gamme g = (Gamme)obj;

            return (Nom == g.Nom) && (Creation == g.Creation);
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
            Gamme gamme = modele as Gamme;
            Nom = gamme.Nom;
            MiseAJour = gamme.MiseAJour;
            Creation = gamme.Creation;
            Suppression = gamme.Suppression;
            Finition = gamme.Finition;
            Isolant = gamme.Isolant;
        }
    }
}
