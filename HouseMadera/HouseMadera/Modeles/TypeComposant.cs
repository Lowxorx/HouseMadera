using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class TypeComposant:ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? MiseAJour { get; set; }

        #region OVERRIDE

        public override string ToString()
        {
            return string.Format("Nom {0} , type {1}", Nom, Qualite.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            TypeComposant t = (TypeComposant)obj;

            return (Nom == t.Nom) && (Creation == t.Creation);
        }

        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            TypeComposant typeComposant = modele as TypeComposant;
            Nom = typeComposant.Nom;
            MiseAJour = typeComposant.MiseAJour;
            Creation = typeComposant.Creation;
            Suppression = typeComposant.Suppression;
            Qualite = typeComposant.Qualite;
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
