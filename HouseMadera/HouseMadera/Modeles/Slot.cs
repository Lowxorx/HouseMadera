using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Slots
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// Id du Slot
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Slot
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Hauteur du Slot
        /// </summary>
        public decimal Hauteur { get; set; }

        /// <summary>
        /// Largeur du Slot
        /// </summary>
        public decimal Largeur { get; set; }

        /// <summary>
        /// Type de Slot
        /// </summary>
        public TypeSlot TypeSlot { get; set; }

    }
}