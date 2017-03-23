using HouseMadera.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
    public class TypeModuleTypeSlot : ISynchronizable
    {
        public int Id { get; set; }
        public TypeModule TypeModule { get; set; }
        public TypeSlot TypeSlot { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }

        #region OVERRIDE
        public override string ToString()
        {
            return string.Format("TypeModule {0} , TypeSlot {1}",TypeModule.Nom,TypeSlot.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            TypeModuleTypeSlot t = (TypeModuleTypeSlot)obj;

            return (Creation == t.Creation) &&
                (TypeSlot.Id == t.TypeSlot.Id) &&
                (TypeModule.Id == t.TypeModule.Id);
        }
        #endregion


        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            TypeModuleTypeSlot typeModuleTypeSlot = modele as TypeModuleTypeSlot;
            TypeSlot = typeModuleTypeSlot.TypeSlot;
            TypeModule = typeModuleTypeSlot.TypeModule;
            MiseAJour = typeModuleTypeSlot.MiseAJour;
            Creation = typeModuleTypeSlot.Creation;
            Suppression = typeModuleTypeSlot.Suppression;
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

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            
            if (modele.MiseAJour == null)
                return true;
            else
                return MiseAJour == modele.MiseAJour;
        }
    }
}
