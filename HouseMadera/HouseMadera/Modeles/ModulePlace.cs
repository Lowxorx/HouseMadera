using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Modules Placés dans des Slots Placés
    /// </summary>
    public class ModulePlace
    {
        /// <summary>
        /// Id du Module Place
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Module
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// SlotPlace
        /// </summary>
        public SlotPlace SlotPlace { get; set; }

    }
}