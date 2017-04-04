using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Projet:ISynchronizable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {

        /// <summary>
        /// Id du Projet
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du Projet
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Reference du Projet
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Date de mise à jour du Projet
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Date de création du Projet
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Commercial
        /// </summary>
        public Commercial Commercial { get; set; }

        /// <summary>
        /// Client
        /// </summary>
        public Client Client { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Projet p = (Projet)obj;

            return ((Nom == p.Nom) && (Reference == p.Reference)) && (Creation == p.Creation);
          
        }

        public override string ToString()
        {
            return string.Format("Projet : {0} (ref :{1})", Nom, Reference);
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

        public void Copy<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            Projet projet = modele as Projet;
            Nom = projet.Nom;
            Reference = projet.Reference;
            Commercial = projet.Commercial;
            Client = projet.Client;
            MiseAJour = projet.MiseAJour;
            Suppression = projet.Suppression;
            Creation = projet.Creation;
        }
    }
}
