using HouseMadera.DAL;
using System;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Slot:ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Id du Slot
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Slot
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Hauteur du Slot
        /// </summary>
        public decimal Hauteur { get; set; }

        /// <summary>
        /// Largeur du Slot
        /// </summary>
        public decimal Largeur { get; set; }

        /// <summary>
        /// Type de Slot
        /// </summary>
        public TypeSlot TypeSlot { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Nom {0}", Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Slot s = (Slot)obj;

            return (Nom == s.Nom) && (Creation == s.Creation);
        }
        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Slot slot = modele as Slot;
            Nom = slot.Nom;
            TypeSlot = slot.TypeSlot;
            MiseAJour = slot.MiseAJour;
            Creation = slot.Creation;
            Suppression = slot.Suppression;
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