using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant le Type de Finition
    /// </summary>
    public class TypeFinition
    {

        /// <summary>
        /// Id du Type de Finition
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Type de Finition
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Qualite du Type de Finition
        /// </summary>
        public Qualite Qualite { get; set; }

    }
}