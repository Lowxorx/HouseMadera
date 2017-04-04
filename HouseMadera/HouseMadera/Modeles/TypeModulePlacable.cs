using HouseMadera.DAL;
using System;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class TypeModulePlacable : ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public byte[] Icone { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }


        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Nom {1}", Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            TypeModulePlacable t = (TypeModulePlacable)obj;

            return (Creation == t.Creation) && (Nom == t.Nom);
        }
        #endregion


        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            TypeModulePlacable typeModulePlacable = modele as TypeModulePlacable;
            Nom = typeModulePlacable.Nom;
            Icone = typeModulePlacable.Icone;
            MiseAJour = typeModulePlacable.MiseAJour;
            Creation = typeModulePlacable.Creation;
            Suppression = typeModulePlacable.Suppression;
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
