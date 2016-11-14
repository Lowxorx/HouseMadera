namespace HouseMadera.Modèles
{
    public class Produit
    {
        public Devis Devis { get; set; }
        public int Id { get; set; }
        public string Nom { get; set; }

        public Plan Plan { get; set; }
        public Projet Projet { get; set; }
    }
}