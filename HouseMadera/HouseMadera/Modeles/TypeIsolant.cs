using HouseMadera.DAL;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class TypeIsolant:ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        
        public int Id { get; set; }
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? Suppression { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Nom {0} Qualite {1}", Nom, Qualite.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            TypeIsolant t = (TypeIsolant)obj;

            return (Nom == t.Nom) && (Creation == t.Creation);
        }

        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            TypeIsolant typeIsolant = modele as TypeIsolant;
            Nom = typeIsolant.Nom;
            Qualite = typeIsolant.Qualite;
            MiseAJour = typeIsolant.MiseAJour;
            Creation = typeIsolant.Creation;
            Suppression = typeIsolant.Suppression;
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