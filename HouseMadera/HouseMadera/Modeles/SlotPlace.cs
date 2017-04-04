using HouseMadera.DAL;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class SlotPlace : ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Id du Slot Place
        /// </summary>
        public int Id { get; set; }
        public string Libelle { get; set; }
        public Module Module { get; set; }
        public Slot Slot { get; set; }
        public TypeModulePlacable TypeModulePlacable { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Libelle {0} ", Libelle);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            SlotPlace s = (SlotPlace)obj;

            return (Creation == s.Creation) && (Libelle == s.Libelle);
        }
        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            SlotPlace slotPLace = modele as SlotPlace;
            Module = slotPLace.Module;
            Slot = slotPLace.Slot;
            TypeModulePlacable = slotPLace.TypeModulePlacable;
            Libelle = slotPLace.Libelle;
            MiseAJour = slotPLace.MiseAJour;
            Creation = slotPLace.Creation;
            Suppression = slotPLace.Suppression;
        }

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
    }
}