using System;

namespace HouseMadera.DAL
{
    public  interface ISynchronizable
    {
        int Id { get; set; }
        DateTime ? MiseAJour { get; set; }
        DateTime ? Suppression { get; set; }
        DateTime ? Creation { get; set; }
        bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable;
        bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable;
        void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable;
    }
}