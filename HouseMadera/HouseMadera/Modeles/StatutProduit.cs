using HouseMadera.DAL;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class StatutProduit:ISynchronizable
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

            StatutProduit s = (StatutProduit)obj;

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
            StatutProduit statutProduit = modele as StatutProduit;
            Nom = statutProduit.Nom;
            MiseAJour = statutProduit.MiseAJour;
            Creation = statutProduit.Creation;
            Suppression = statutProduit.Suppression;
        }
    }
}