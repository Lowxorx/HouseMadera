using HouseMadera.DAL;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class TypeFinition:ISynchronizable
    {

        public int Id { get; set; }
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? MiseAJour { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Nom {0} Qualite {1}", Nom, Qualite.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            TypeFinition t = (TypeFinition)obj;

            return (Nom == t.Nom) && (Creation == t.Creation) && (Qualite.Id == t.Qualite.Id);
        }
        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            TypeFinition typeFinition = modele as TypeFinition;
            Nom = typeFinition.Nom;
            Qualite = typeFinition.Qualite;
            MiseAJour = typeFinition.MiseAJour;
            Creation = typeFinition.Creation;
            Suppression = typeFinition.Suppression;
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