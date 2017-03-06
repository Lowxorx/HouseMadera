using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Modules génériques
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Id du Module
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Module
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Hauteur du Module
        /// </summary>
        public decimal Hauteur { get; set; }

        /// <summary>
        /// Largeur du Module
        /// </summary>
        public decimal Largeur { get; set; }

        /// <summary>
        /// Gamme associée
        /// </summary>
        public Gamme Gamme { get; set; }

        /// <summary>
        /// Type du Module associé
        /// </summary>
        public TypeModule TypeModule { get; set; }


    }
}