using HouseMadera.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ModulePlacePlan : ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public int Id { get; set; }
        public ModulePlace ModulePlace { get; set; }
        public Plan Plan { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }

        #region OVERRIDE
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ModulePlacePlan mp = (ModulePlacePlan)obj;

            return Creation == mp.Creation;

        }

        public override string ToString()
        {
            return string.Format("ModulePlace : {0}, Plan{1}",ModulePlace.Libelle,Plan.Nom);
        }

        #endregion


        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            ModulePlacePlan mpp = modele as ModulePlacePlan;
            ModulePlace = mpp.ModulePlace;
            Plan = mpp.Plan;
            MiseAJour = mpp.MiseAJour;
            Suppression = mpp.Suppression;
            Creation = mpp.Creation;
        }

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.MiseAJour == null)
                return true;
            else
                return MiseAJour == modele.MiseAJour;
        }

        public bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.Suppression != null && !Suppression.HasValue)
            {
                Suppression = modele.Suppression;
                return true;
            }

            return false;
        }
    }
}
