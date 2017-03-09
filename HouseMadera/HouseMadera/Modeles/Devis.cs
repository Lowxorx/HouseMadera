using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{

    /// <summary>
    /// Classe représentant les Devis
    /// </summary>
    public class Devis
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
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// PrixHT du Devis
        /// </summary>
        public Decimal PrixHT { get; set; }

        /// <summary>
        /// Prix TTC du Devis
        /// </summary>
        public Decimal PrixTTC { get; set; }

        /// <summary>
        /// Statut du Devis
        /// </summary>
        public StatutDevis StatutDevis { get; set; }

        /// <summary>
        /// Facture au format PDF (Blob)
        /// </summary>
        public byte[] Pdf { get; set; }
    }
}
