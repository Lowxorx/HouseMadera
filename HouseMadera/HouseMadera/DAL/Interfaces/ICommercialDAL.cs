using HouseMadera.Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.DAL
{
    public interface ICommercialDAL:IDAL<Commercial>
    {
        new List<Commercial> GetAllModeles();
    }
}
