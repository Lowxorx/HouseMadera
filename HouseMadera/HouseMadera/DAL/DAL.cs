using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;


namespace HouseMadera.DAL
{
    public class DAL : IDisposable
    {
        private string ConnectionStringMySql { get; set; }
        private string ConnectionStringSQLite { get; set; }

        public string Bdd { get; set; }
        protected IDbConnection Connection { get; set; }


        public DAL(string nomBdd)
        {
            ConnectionStringMySql = ConfigurationManager.ConnectionStrings["HouseMaderaDBMySql"].ConnectionString;
            ConnectionStringSQLite = ConfigurationManager.ConnectionStrings["HouseMaderaDBSQlite"].ConnectionString;

            Bdd = nomBdd;
            switch (Bdd)
            {
                case "MYSQL":
                    Connection = new MySqlConnect(ConnectionStringMySql);
                    Connection.Open();
                    break;
                case "SQLITE":
                    Connection = new SQLiteConnect(ConnectionStringSQLite);
                    Connection.Open();
                    break;
                default:
                    throw new Exception("La base de donnée spécifiée n'est pas reconnue");
            }
        }


        private DbCommand GetCommand(string requete, IDictionary<string, object> parameters = null)
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

        public int Insert(string requete, IDictionary<string, object> parameters)
        {
            using (var command = GetCommand(requete, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public int Update(string requete, IDictionary<string, object> parameters)
        {
            using (var command = GetCommand(requete, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

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
        public DbDataReader GetDataReader(DbCommand command)
        {

            return Connection.GetDataReader(command);

        }

        public DbDataReader Get(string requete, IDictionary<string, object> parameters = null)
        {
            using (var command = GetCommand(requete, parameters))
            {
                var dataReader = GetDataReader(command);
                return dataReader;

            }
        }



        public void Dispose()
        {
            if (Connection != null)
                Connection.Dispose();

        }
    }
}

