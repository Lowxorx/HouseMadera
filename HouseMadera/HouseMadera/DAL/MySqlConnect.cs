using HouseMadera.Utilites;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class MySqlConnect : IDbConnection
    {
        public MySqlConnection Connection { get; set; }

        public MySqlConnect(string connectionString)
        {
            Connection = new MySqlConnection { ConnectionString = connectionString };
        }

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

        public void Dispose()
        {
            Connection.Dispose();
        }

        public DbCommand GetCommand()
        {
            var command = new MySqlCommand();
            command.Connection = Connection;
            return command;
        }

        public DbDataAdapter GetAdapter()
        {
            return new MySqlDataAdapter();
        }

        public DbParameter GetDbParameter(string name, object value)
        {
            return new MySqlParameter(name, value);
        }

        public DbDataReader GetDataReader(DbCommand command)
        {
            return command.ExecuteReader();
        }


    }
}


