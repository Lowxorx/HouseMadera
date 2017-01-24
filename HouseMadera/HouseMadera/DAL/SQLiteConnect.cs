using System;
using System.Data.Common;
using System.Data.SQLite;

namespace HouseMadera.DAL
{
    public class SQLiteConnect : IDbConnection
    {
        public SQLiteConnection Connection { get; set; }

        public SQLiteConnect(string connectionString)
        {
            Connection = new SQLiteConnection{ ConnectionString = connectionString };
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
            var command = new SQLiteCommand();
            command.Connection = Connection;
            return command;
        }

        public DbDataAdapter GetAdapter()
        {
            return new SQLiteDataAdapter();
        }

        public DbParameter GetDbParameter(string name, object value)
        {
            return new SQLiteParameter(name, value);
        }

        public DbDataReader GetDataReader(DbCommand command)
        {
           return command.ExecuteReader();
         
        }

    }
}


