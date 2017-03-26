using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class Slot:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public decimal Hauteur { get; set; }
        public decimal Largeur { get; set; }
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

            return (Nom == s.Nom) &&
                (Creation == s.Creation) &&
                (Largeur == s.Largeur) &&
                (Hauteur == s.Hauteur);
        }
        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Slot slot = modele as Slot;
            Nom = slot.Nom;
            Hauteur = slot.Hauteur;
            Largeur = slot.Largeur;
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