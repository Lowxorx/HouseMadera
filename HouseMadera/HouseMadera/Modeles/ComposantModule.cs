using HouseMadera.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ComposantModule : ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {

        public int Id { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Creation { get; set; }
        public DateTime? Suppression { get; set; }
        public Composant Composant { get; set; }
        public Module Module { get; set; }
        public int Nombre { get; set; }

        #region OVERRIDE

        public override string ToString()
        {
            return string.Format("Composant {0} - Module {1}", Composant.Nom, Module.Nom);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ComposantModule c = (ComposantModule)obj;

            return (Nombre == c.Nombre) && (Creation == c.Creation);
        }
        #endregion

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            ComposantModule composantModule = modele as ComposantModule;
            Nombre = composantModule.Nombre;
            MiseAJour = composantModule.MiseAJour;
            Creation = composantModule.Creation;
            Suppression = composantModule.Suppression;
            Composant = composantModule.Composant;
            Module = composantModule.Module;
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
