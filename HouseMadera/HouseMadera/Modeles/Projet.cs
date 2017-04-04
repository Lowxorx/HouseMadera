using GalaSoft.MvvmLight;
using HouseMadera.Modeles;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HouseMadera.Modeles
{
    /// <summary>
    /// Classe représentant le Projet
    /// </summary>
    public class Projet
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

    }
}
