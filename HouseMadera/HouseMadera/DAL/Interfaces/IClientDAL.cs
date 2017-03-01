using HouseMadera.Modeles;
using System.Collections.Generic;

namespace HouseMadera.DAL
{
    public interface IClientDAL : IDAL<Client>
    {
        new List<Client> GetAll();
    }
}