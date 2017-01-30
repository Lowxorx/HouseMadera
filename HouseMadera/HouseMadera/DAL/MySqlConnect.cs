using MySql.Data.MySqlClient;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class MySqlConnect: IDbConnection
    {
        public MySqlConnection Connection { get; set; }

        public MySqlConnect(string connectionString)
        {
            System.Console.WriteLine(connectionString);
            Connection = new MySqlConnection { ConnectionString = connectionString };
        }

        public void Open()
        {
            Connection.Open();
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


