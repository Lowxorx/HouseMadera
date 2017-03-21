using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class SlotPlace
    {
        public int Id { get; set; }

        public int Abscisse { get; set; }
        public int Ordonnee { get; set; }

        public  Module Module { get; set; }
        public Slot Slot { get; set; }
    }
}