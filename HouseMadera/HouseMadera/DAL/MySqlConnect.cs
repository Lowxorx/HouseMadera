using HouseMadera.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace HouseMadera.DAL
{

    public class MySqlConnect : IDbConnection
    {
        public MySqlConnection Connection { get; set; }

        /// <summary>
        /// Créer la connexion
        /// </summary>
        /// <param name="connectionString"></param>
        public MySqlConnect(string connectionString)
        {
            Connection = new MySqlConnection { ConnectionString = connectionString };
        }
        /// <summary>
        /// Ouvre la connexion
        /// </summary>
        public void Open()
        {
            try
            {
                Connection.Open();

            }
            catch (MySqlException)
            {
                // Time out connexion BDD
                Logger.WriteTrace("Timeout connexion BDD");
            }
            catch (Exception ex)
            {
                // Erreur inconnue
                Logger.WriteEx(ex);
            }
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Connection.Dispose();
        }

        /// <summary>
        /// Récupère la commande
        /// </summary>
        /// <returns></returns>
        public DbCommand GetCommand()
        {
            var command = new MySqlCommand()
            {
                Connection = Connection
            };
            return command;
        }

        /// <summary>
        /// Récupère l'adapter
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter GetAdapter()
        {
            return new MySqlDataAdapter();
        }

        /// <summary>
        /// Récupère les paramètres de la base de données
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter GetDbParameter(string name, object value)
        {
            return new MySqlParameter(name, value);
        }

        /// <summary>
        /// Executer la lecteur des données
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DbDataReader GetDataReader(DbCommand command)
        {
            return command.ExecuteReader();
        }


    }
}


