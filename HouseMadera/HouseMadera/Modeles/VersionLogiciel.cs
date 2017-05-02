using System;


namespace HouseMadera.Modeles
{
   public  class VersionLogiciel
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public bool Current { get; set; }
        public DateTime Date { get; set; }
    }
}
