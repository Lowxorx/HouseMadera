using HouseMadera.Modeles;
using System.Collections.Generic;

namespace HouseMadera.DAL
{
    public interface IClientDAL : IDAL<Client>
    {
        List<Client> GetAll();
    }
}