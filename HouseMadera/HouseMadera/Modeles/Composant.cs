using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class Composant:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public TypeComposant TypeComposant { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? MiseAJour { get; set; }

        #region OVERRIDE

        public override string ToString()
        {
            return string.Format("Nom {0} , type {1}", Nom, TypeComposant.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Composant c = (Composant)obj;

            return (Nom == c.Nom) &&
                (Creation == c.Creation) &&
                (TypeComposant.Id == c.TypeComposant.Id) &&
                (Prix == c.Prix);
        }

        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Composant Composant = modele as Composant;
            Nom = Composant.Nom;
            Prix = Composant.Prix;
            MiseAJour = Composant.MiseAJour;
            Creation = Composant.Creation;
            Suppression = Composant.Suppression;
            TypeComposant = Composant.TypeComposant;
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