using System.Collections.Generic;

namespace HouseMadera.DAL
{
    public interface IDAL<TMODELE>
    {
        List<TMODELE> GetAll();
        int InsertNew(TMODELE modele);
    }
}