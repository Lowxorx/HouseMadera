using HouseMadera.DAL;
using HouseMadera.Modeles;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class Client:ISynchronizable
    {
        public int Id { get; set; }

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string Adresse3 { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public DateTime MiseAJour { get; set; }
        public int StatutClient { get; set; }

        public List<Projet> Projets { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Client cl = (Client)obj;
            return (Nom == cl.Nom) && (Prenom == cl.Prenom) && (Adresse1 == cl.Adresse1) && (Adresse2 == cl.Adresse2) && (Adresse3 == cl.Adresse3) && (CodePostal == cl.CodePostal) && (Ville == cl.Ville) && (Email == cl.Email)&& (Telephone == cl.Telephone) && (Mobile == Mobile) && (StatutClient == cl.StatutClient);
        }

        public override string ToString()
        {
            return Prenom + " " + Nom + " de " + Ville;
        }

        public bool IsUpToDate<TMODELE>( TMODELE modele) where TMODELE : ISynchronizable
        {
            return MiseAJour == modele.MiseAJour;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
