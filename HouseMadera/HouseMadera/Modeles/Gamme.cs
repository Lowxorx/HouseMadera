using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Gammes
    /// </summary>
    public class Gamme
    {
        /// <summary>
        /// Id de la Gamme
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de la Gamme
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Finition de la Gamme
        /// </summary>
        public Finition Finition { get; set; }

        /// <summary>
        /// Isolant de la Gamme
        /// </summary>
        public Isolant Isolant { get; set; }

    }
}
