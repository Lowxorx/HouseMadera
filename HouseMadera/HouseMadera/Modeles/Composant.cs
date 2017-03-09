using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Composants
    /// </summary>
    public class Composant
    {
        /// <summary>
        /// Id du Composant
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Composant
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prix du Composant
        /// </summary>
        public decimal Prix { get; set; }

        /// <summary>
        /// Type du Composant
        /// </summary>
        public TypeComposant TypeComposant { get; set; }
    }
}