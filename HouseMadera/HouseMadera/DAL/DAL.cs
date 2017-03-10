using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;


namespace HouseMadera.DAL
{
    public class DAL : IDisposable
    {
        #region PROPRIETES ET ATTRIBUTS
        private string connectionStringMySql { get; set; }
        private string connectionStringSQLite { get; set; }

        public static string Bdd { get; set; }
        protected static IDbConnection Connection { get; set; }
        #endregion

        public DAL(string nomBdd)
        {
            connectionStringMySql = ConfigurationManager.ConnectionStrings["HouseMaderaDBMySql"].ConnectionString;
            connectionStringSQLite = ConfigurationManager.ConnectionStrings["HouseMaderaDBSQlite"].ConnectionString;
            Bdd = nomBdd;
            switch (Bdd)
            {
                case "MYSQL":
                    Connection = new MySqlConnect(connectionStringMySql);
                    Connection.Open();
                    break;
                case "SQLITE":
                    Connection = new SQLiteConnect(connectionStringSQLite);
                    Connection.Open();
                    break;
                default: break;

            }

        }

        /// <summary>
        /// Instancie un objet DbCommand contenant la requête à éxécuter complétée des paramètres  
        /// </summary>
        /// <param name="requete">Chaine de caractère représentant la requête SQL</param>
        /// <param name="parameters">Dictionnaire contenant les paramètres à substituer à la requête</param>
        /// <returns>Un objet DbCommand</returns>
        private static DbCommand GetCommand(string requete, IDictionary<string, object> parameters = null)
        {
            var command = Connection.GetCommand();
            command.CommandText = requete;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                    command.Parameters.Add(Connection.GetDbParameter(parameter.Key, parameter.Value));
            }

            return command;
        }

        /// <summary>
        /// Instancie un objet DbCommand avec la requête d'insertion passée en paramètre puis l'éxécute
        /// </summary>
        /// <param name="requete">Chaine de caractère représentant la requête SQL de type INSERT</param>
        /// <param name="parameters">Dictionnaire contenant les paramètres à substituer à la requête</param>
        /// <returns>Le nombre de lignes affectées à la base </returns>
        public int Insert(string requete, IDictionary<string, object> parameters)
        {
            using (var command = GetCommand(requete, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Instancie un objet DbCommand avec la requête d'update passée en paramètre puis l'éxécute
        /// </summary>
        /// <param name="requete">Chaine de caractère représentant la requête SQL de type UPDATE</param>
        /// <param name="parameters">Dictionnaire contenant les paramètres à substituer à la requête</param>
        /// <returns>Le nombre de lignes affectées à la base </returns>
        public int Update(string requete, IDictionary<string, object> parameters)
        {
            using (var command = GetCommand(requete, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Instancie un objet DbCommand avec la requête d'insertion passée en paramètre puis l'éxécute
        /// </summary>
        /// <param name="requete">Chaine de caractère représentant la requête SQL de type DELETE</param>
        /// <param name="parameters">Dictionnaire contenant les paramètres à substituer à la requête</param>
        /// <returns>Le nombre de lignes affectées à la base </returns>
        public int Delete(string requete, IDictionary<string, object> parameters)
        {
            using (var command = GetCommand(requete, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        //public DataSet Get(string requete, IDictionary<string, object> parameters = null)
        //{
        //    using (var adptSQL = Connection.GetAdapter())
        //    {
        //        adptSQL.SelectCommand = GetCommand(requete, parameters);

        //        var ds = new DataSet();
        //        adptSQL.Fill(ds);
        //        return ds;
        //    }
        //}
        public static DbDataReader GetDataReader(DbCommand command)
        {
            return Connection.GetDataReader(command);
        }

        /// <summary>
        /// Obtient un objet DbDataReader en instanciant une commande avec la requête de sélection passée en paramètre puis l'éxécute
        /// </summary>
        /// <param name="requete">Chaine de caractère représentant la requête SQL de type INSERT</param>
        /// <param name="parameters">Dictionnaire contenant les paramètres à substituer à la requête</param>
        /// <returns>Retourne un objet DbDataReader</returns>
        public static DbDataReader Get(string requete, IDictionary<string, object> parameters = null)
        {
            using (var command = GetCommand(requete, parameters))
            {
                var dataReader = GetDataReader(command);
                return dataReader;
            }
        }

        /// <summary>
        /// Ferme la connection courante à la base de donnée
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
                Connection.Dispose();

        }
    }
}

