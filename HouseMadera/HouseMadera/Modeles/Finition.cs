
using System;
using HouseMadera.DAL;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Finition : ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Id de la finission
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de la finission
        /// </summary>
        public string Nom { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? MiseAJour { get; set; }
        public TypeFinition TypeFinition { get; set; }

        #region OVERRIDE

        public override string ToString()
        {
            return string.Format("Nom {0} , type {1}", Nom, TypeFinition.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Finition f = (Finition)obj;

            return (Nom == f.Nom) && (Creation == f.Creation);
        }

        #endregion


        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Finition finition = modele as Finition;
            Nom = finition.Nom;
            MiseAJour = finition.MiseAJour;
            Creation = finition.Creation;
            Suppression = finition.Suppression;
            TypeFinition = finition.TypeFinition;
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