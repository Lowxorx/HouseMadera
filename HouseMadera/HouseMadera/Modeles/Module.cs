using HouseMadera.DAL;
using System;

namespace HouseMadera.Modeles
{
    public class Module:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public decimal Hauteur { get; set; }
        public decimal Largeur { get; set; }
        public Gamme Gamme { get; set; }
        public TypeModule TypeModule { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }


        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("Nom {0} , Gamme {1} , Type {2}", Nom,Gamme.Nom,TypeModule.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Module m = (Module)obj;

            return (Nom == m.Nom) &&
                (Creation == m.Creation) &&
                (Largeur == m.Largeur) &&
                (Hauteur == m.Hauteur) &&
                (Gamme.Id == m.Gamme.Id) &&
                (TypeModule.Id == m.TypeModule.Id);
        }
        #endregion

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.MiseAJour == null)
                return true;
            else
                return MiseAJour == modele.MiseAJour;
        }

        public bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.Suppression != null && !Suppression.HasValue)
            {
                Suppression = modele.Suppression;
                return true;
            }

            return false;
        }

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Module module = modele as Module;
            Nom = module.Nom;
            Hauteur = module.Hauteur;
            Largeur = module.Largeur;
            Gamme = module.Gamme;
            TypeModule = module.TypeModule;
            MiseAJour = module.MiseAJour;
            Creation = module.Creation;
            Suppression = module.Suppression;
        }
        
    }
}