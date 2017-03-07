using System.Collections.Generic;

namespace HouseMadera.DAL
{
    public interface IDAL<TMODELE>
    {
        List<TMODELE> GetAll();
        int InsertModele(TMODELE modele);
        int UpdateModele(TMODELE modele1,TMODELE modele2);
        int DeleteModele(TMODELE modele);
    }
}