namespace HouseMadera.Modeles
{
    public class Produit
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Projet Projet { get; set; }
        public Devis Devis { get; set; }
        public Plan Plan { get; set; }
        public StatutProduit StatutProduit { get; set; }
    }
}
