namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant les Produits
    /// </summary>
    public class Produit
    {
        /// <summary>
        /// Id du Produit
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Produit
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Projet
        /// </summary>
        public Projet Projet { get; set; }

        /// <summary>
        /// Devis
        /// </summary>
        public Devis Devis { get; set; }

        /// <summary>
        /// Plan
        /// </summary>
        public Plan Plan { get; set; }

        /// <summary>
        /// Statut Produit
        /// </summary>
        public StatutProduit StatutProduit { get; set; }
    }
}
