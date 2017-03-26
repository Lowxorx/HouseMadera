using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class ModulePlace : ISynchronizable
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public bool Horizontal { get; set; }
        public bool Vertical { get; set; }
        public Module Module { get; set; }
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