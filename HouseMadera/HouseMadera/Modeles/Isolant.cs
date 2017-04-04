using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Isolants
    /// </summary>
    public class Isolant
    {
        /// <summary>
        /// Id de l'Isolant
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de l'Isolant
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Type de l'Isolant
        /// </summary>
        public TypeIsolant TypeIsolant { get; set; }

    }
}