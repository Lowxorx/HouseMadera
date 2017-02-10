using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class StatutProduit
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Produit> Produits { get; set; }
    }
}