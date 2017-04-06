using HouseMadera.DAL;
using System;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    /// Classe représentant les Devis
    /// </summary>
    public class Devis:ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Id du Devis
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Devis
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Date de création du Devis
        /// </summary>
        public DateTime? DateCreation { get; set; }
        public decimal PrixHT { get; set; }
        public decimal PrixTTC { get; set; }
        public StatutDevis StatutDevis { get; set; }
        public byte[] Pdf { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Devis : {0} \n Prix HT : {1} \n Prix TTC {2}",Nom,PrixHT,PrixTTC);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Devis d = (Devis)obj;

            return (Nom == d.Nom ) && (PrixHT == d.PrixHT) && (PrixTTC == d.PrixTTC)&&(Creation == d.Creation);
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
            Devis devis = modele as Devis;
            Nom = devis.Nom;
            PrixHT = devis.PrixHT;
            PrixTTC = devis.PrixTTC;
            Pdf = devis.Pdf;
            StatutDevis = devis.StatutDevis;
            MiseAJour = devis.MiseAJour;
            Creation = devis.Creation;
            Suppression = devis.Suppression;
        }
    }
}
