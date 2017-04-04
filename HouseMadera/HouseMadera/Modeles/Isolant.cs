using HouseMadera.DAL;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    /// Classe représentant les Isolants
    /// </summary>
    public class Isolant:ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Id de l'Isolant
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de l'Isolant
        /// </summary>
        public string Nom { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }
        public TypeIsolant TypeIsolant { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Nom {0} , type {1}", Nom , TypeIsolant.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Isolant i = (Isolant)obj;

            return (Nom == i.Nom) && (Creation == i.Creation);
        }
        #endregion


        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Isolant isolant = modele as Isolant;
            Nom = isolant.Nom;
            MiseAJour = isolant.MiseAJour;
            Creation = isolant.Creation;
            Suppression = isolant.Suppression;
            TypeIsolant = isolant.TypeIsolant;
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