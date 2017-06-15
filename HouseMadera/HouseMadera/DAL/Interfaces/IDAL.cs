using System.Collections.Generic;

namespace HouseMadera.DAL
{
    public interface IDAL<TMODELE>
    {
        List<TMODELE> GetAllModeles();
        int InsertModele(TMODELE modele,MouvementSynchronisation sens);
        int UpdateModele(TMODELE modele1,TMODELE modele2, MouvementSynchronisation sens);
        int DeleteModele(TMODELE modele);
    }
}