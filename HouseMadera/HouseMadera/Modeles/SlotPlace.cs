using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Slots Placés
    /// </summary>
    public class SlotPlace
    {
        /// <summary>
        /// Id du Slot Place
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Abscisse du Slot Place
        /// </summary>
        public int Abscisse { get; set; }

        /// <summary>
        /// Ordonnee du Slot Place
        /// </summary>
        public int Ordonnee { get; set; }

        /// <summary>
        /// Module du Slot Place
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// Slot du Slot Place
        /// </summary>
        public Slot Slot { get; set; }


    }
}