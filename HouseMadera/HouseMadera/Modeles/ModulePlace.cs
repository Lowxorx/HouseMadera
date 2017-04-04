using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ModulePlace : ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Id du Module Place
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Libelle du Module Place
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Horizontal ou non (cloison)
        /// </summary>
        public Boolean Horizontal { get; set; }

        /// <summary>
        /// Vertical ou non (cloison)
        /// </summary>
        public Boolean Vertical { get; set; }

        /// <summary>
        /// Module
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// SlotPlace
        /// </summary>
        public SlotPlace SlotPlace { get; set; }
        public Produit Produit { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }


        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Module {1} , SlotPlace {2}", Module.Nom, SlotPlace.Libelle);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ModulePlace m = (ModulePlace)obj;

            return (Creation == m.Creation) &&
                (Libelle == m.Libelle) &&
                (Horizontal == m.Horizontal) &&
                (Vertical == m.Vertical) ;
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
            ModulePlace modulePLace = modele as ModulePlace;
            Module = modulePLace.Module;
            SlotPlace = modulePLace.SlotPlace;
            Produit = modulePLace.Produit;
            Libelle = modulePLace.Libelle;
            Horizontal = modulePLace.Horizontal;
            Vertical = modulePLace.Vertical;
            MiseAJour = modulePLace.MiseAJour;
            Creation = modulePLace.Creation;
            Suppression = modulePLace.Suppression;
        }

    }
}