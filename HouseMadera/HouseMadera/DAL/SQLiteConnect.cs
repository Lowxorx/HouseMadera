using System;
using System.Data.Common;
using System.Data.SQLite;

namespace HouseMadera.DAL
{
    public class SQLiteConnect : IDbConnection
    {
        public SQLiteConnection Connection { get; set; }

        /// <summary>
        /// Créer la connexion
        /// </summary>
        /// <param name="connectionString"></param>
        public SQLiteConnect(string connectionString)
        {
            Connection = new SQLiteConnection{ ConnectionString = connectionString };
        }
        /// <summary>
        /// Ouvre la connexion
        /// </summary>
        public void Open()
        {
            Connection.Open();
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
            var command = new SQLiteCommand();
            command.Connection = Connection;
            return command;
        }

        /// <summary>
        /// Récupère l'adapter
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter GetAdapter()
        {
            return new SQLiteDataAdapter();
        }

        /// <summary>
        /// Récupère les paramètres de la base de données
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter GetDbParameter(string name, object value)
        {
            return new SQLiteParameter(name, value);
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


