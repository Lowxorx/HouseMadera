&using HouseMadera.Modeles;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Commerciaux
    /// </summary>
    public class Commercial
    {
        /// <summary>
        /// Id du Commercial
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Commercial
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prenom du Commercial
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Login du Commercial
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Password du Commercial
        /// </summary>
        public string Password { get; set; }
    }
}
