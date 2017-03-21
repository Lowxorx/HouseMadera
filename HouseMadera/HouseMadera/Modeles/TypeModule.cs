using HouseMadera.DAL;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class TypeModule : ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
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

            TypeModule t = (TypeModule)obj;

            return (Nom == t.Nom) && (Creation == t.Creation);
        }

        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            TypeModule typeModule = modele as TypeModule;
            Nom = typeModule.Nom;
            MiseAJour = typeModule.MiseAJour;
            Creation = typeModule.Creation;
            Suppression = typeModule.Suppression;
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