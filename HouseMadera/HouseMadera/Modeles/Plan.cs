using HouseMadera.DAL;
using System;


namespace HouseMadera.Modeles
{
    public class Plan : ISynchronizable
    {
        /// <summary>
        /// Id du Plan
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Plan
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Date de création du Plan
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gamme du Plan
        /// </summary>
        public Gamme Gamme { get; set; }

        /// <summary>
        /// Coupe de Principe du Plan
        /// </summary>
        public CoupePrincipe CoupePrincipe { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Plan : {0} , Gamme :{1}, CoupePrincipe :{2}", Nom,Gamme.Nom,CoupePrincipe.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Plan p = (Plan)obj;

            return (Nom == p.Nom) && (Creation == p.Creation);
        }
        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Plan plan = modele as Plan;
            Nom = plan.Nom;
            MiseAJour = plan.MiseAJour;
            Creation = plan.Creation;
            Suppression = plan.Suppression;
            Gamme = plan.Gamme;
            CoupePrincipe = plan.CoupePrincipe;
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
