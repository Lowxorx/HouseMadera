using HouseMadera.DAL;
using HouseMadera.Modeles;
using System.Collections.Generic;
using System;

namespace HouseMadera.Modeles
{
    public class Commercial:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
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
