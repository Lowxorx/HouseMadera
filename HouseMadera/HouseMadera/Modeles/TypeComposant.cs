using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant le Type d'un Composant
    /// </summary>
    public class TypeComposant
    {

        /// <summary>
        /// Id du Type de Composant
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Type de Composant
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Qualité du Type de Composant
        /// </summary>
        public Qualite Qualite { get; set; }

    }
}
