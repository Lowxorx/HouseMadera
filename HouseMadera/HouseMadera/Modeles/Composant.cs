using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    /// Classe représentant les Composants
    /// </summary>
    public class Composant:ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Id du Composant
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Composant
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prix du Composant
        /// </summary>
        public decimal Prix { get; set; }

        /// <summary>
        /// Type du Composant
        /// </summary>
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

            return (Nom == c.Nom) && (Creation == c.Creation) && (Prix == c.Prix);
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