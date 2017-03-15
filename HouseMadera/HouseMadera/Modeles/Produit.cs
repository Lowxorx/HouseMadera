using System;
using HouseMadera.DAL;

namespace HouseMadera.Modeles
{
    public class Produit:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Projet Projet { get; set; }
        public Devis Devis { get; set; }
        public Plan Plan { get; set; }
        public StatutProduit StatutProduit { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            throw new NotImplementedException();
        }

        public bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            throw new NotImplementedException();
        }
    }
}
