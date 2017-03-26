using System;
using HouseMadera.DAL;

namespace HouseMadera.Modeles
{
    public class Produit:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Projet Projet { get; set; }
        public Devis Devis { get; set; }
        public Plan Plan { get; set; }
        public StatutProduit StatutProduit { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Produit : {0} , projet :{1}, Devis :{2}, Plan:{3},Statut:{4} ",
                Nom,
                Projet.Nom,
                Devis.PrixTTC,
                Plan.Nom,
                StatutProduit.Nom
                );
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Produit p = (Produit)obj;

            return (Nom == p.Nom) &&(Creation == p.Creation);
        }
        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Produit produit = modele as Produit;
            Nom = produit.Nom;
            MiseAJour = produit.MiseAJour;
            Creation = produit.Creation;
            Suppression = produit.Suppression;
            Projet = produit.Projet;
            Devis = produit.Devis;
            Plan = produit.Plan;
            StatutProduit = produit.StatutProduit;
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
