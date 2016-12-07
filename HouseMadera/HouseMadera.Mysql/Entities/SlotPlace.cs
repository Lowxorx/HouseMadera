using System.Collections.Generic;

namespace HouseMadera.Mysql.Entities
{
    public class SlotPlace
    {
        public int Id { get; set; }

        public int Abscisse { get; set; }
        public int Ordonnee { get; set; }

        public  Module Module { get; set; }
        public Slot Slot { get; set; }

        public virtual ICollection<ModulePlace> ModulesPlaces { get; set; }

    }
}