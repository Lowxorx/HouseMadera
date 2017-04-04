using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Plans
    /// </summary>
    public class Plan
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


    }
}
