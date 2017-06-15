using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class VersionLogicielDAL:DAL
    {
        public VersionLogicielDAL(string nomBdd) : base(nomBdd)
        {
        }

        /// <summary>
        /// Récupère la version courante du logiciel
        /// </summary>
        /// <returns>Un objet VersionLogiciel</returns>
        public VersionLogiciel GetLastVersion()
        {
            VersionLogiciel version = new VersionLogiciel();
            string requete = "SELECT * FROM Version ORDER BY id DESC LIMIT 1";
            //récupèration de la dernière version 
            using (DbDataReader reader = Get(requete, null))
            {
                while (reader.Read())
                {
                    version.Id = reader.GetInt32(0);
                    version.Numero = reader.GetString(1);
                    version.Current = reader.GetBoolean(2);
                    version.Date = reader.GetDateTime(3);
                }
            }

            return version;
        }

        /// <summary>
        ///  compare avec la dernière version connue de l'appli avec la version actuelle
        /// </summary>
        /// <returns>true si la version actuelle est infèrieure à la dernière version disponible sinon false </returns>
        public bool IsLogicielAJour(VersionLogiciel derniereVersion)
        {
            //Récupère les infos de l'assembly
            AppInfo infoLogiciel = new AppInfo(); 
            //Compare avec la version acutelle
            return derniereVersion.Numero == infoLogiciel.Version; 
        }
    }
}
