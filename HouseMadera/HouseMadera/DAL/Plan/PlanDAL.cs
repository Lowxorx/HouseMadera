using HouseMadera.DAL.Interfaces;
using HouseMadera.Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.DAL
{
    public class PlanDAL:DAL,IPlanDAL
    {
        public PlanDAL(string nomBdd) : base(nomBdd)
        {

        }


        #region SYNCHRONISATION
        public int DeleteModele(Plan modele)
        {
            throw new NotImplementedException();
        }

        public List<Plan> GetAllModeles()
        {
            throw new NotImplementedException();
        }

        public int InsertModele(Plan modele)
        {
            throw new NotImplementedException();
        }

        public int UpdateModele(Plan modele1, Plan modele2)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
