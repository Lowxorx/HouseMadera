using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class Plan:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime CreateDate { get; set; }
        public Gamme Gamme { get; set; }
        public CoupePrincipe CoupePrincipe { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        public override string ToString()
        {
            return string.Format("plan : {0}", Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Plan d = (Plan)obj;

            return (Nom == d.Nom) && (Creation == d.Creation);
        }

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            throw new NotImplementedException();
        }

        public bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            throw new NotImplementedException();
        }

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            throw new NotImplementedException();
        }
    }
}
