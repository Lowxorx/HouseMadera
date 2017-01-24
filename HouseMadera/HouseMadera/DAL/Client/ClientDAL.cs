

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace HouseMadera.DAL.Client
{
    public class ClientDAL : DAL
    {
        public ClientDAL(string nomBdd) : base(nomBdd)
        {
            
        }

        public List<Modeles.Client> GetAllClients()
        {
            string sql = "select * from Client order by Nom desc";
            var clients = new List<Modeles.Client>();
            var reader = Get(sql, null);
            while (reader.Read())
            {
                var client = new Modeles.Client();
                client.Id = Convert.ToInt32(reader["id"]);
                client.Nom = Convert.ToString(reader["nom"]);
                client.Prenom = Convert.ToString(reader["prenom"]);
                client.Adresse1 = Convert.ToString(reader["adresse1"]);
                client.Adresse2 = Convert.ToString(reader["adresse2"]);
                client.Adresse3 = Convert.ToString(reader["adresse3"]);
                client.Mobile = Convert.ToString(reader["mobile"]);
                client.Telephone = Convert.ToString(reader["telephone"]);
                clients.Add(client);
            }
            //******************************** Sans factorisation **************************
            //if (Bdd == "SQLITE")
            //{
            //    SQLiteCommand commandSQlite = new SQLiteCommand(sql, BddSQLite);
            //    SQLiteDataReader readerSQLite = commandSQlite.ExecuteReader();

            //    while (readerSQLite.Read())
            //    {
            //        var client = new Modeles.Client();
            //        client.Id = Convert.ToInt32(readerSQLite["Id"]);
            //        client.Nom = Convert.ToString(readerSQLite["Nom"]);
            //        client.Prenom = Convert.ToString(readerSQLite["Prenom"]);
            //        client.Adresse1 = Convert.ToString(readerSQLite["Adresse1"]);
            //        client.Adresse2 = Convert.ToString(readerSQLite["Adresse2"]);
            //        client.Adresse3 = Convert.ToString(readerSQLite["Adresse3"]);
            //        client.Mobile = Convert.ToString(readerSQLite["Mobile"]);
            //        client.Telephone = Convert.ToString(readerSQLite["Telephone"]);
            //        clients.Add(client);
            //    }
            //}
            //if(Bdd == "MYSQL")
            //{
            //    MySqlCommand commandMySql = new MySqlCommand(sql, BddMySql);
            //    MySqlDataReader readerMySql = commandMySql.ExecuteReader();
            //    while (readerMySql.Read())
            //    {
            //        var client = new Modeles.Client();
            //        client.Id = Convert.ToInt32(readerMySql["Id"]);
            //        client.Nom = Convert.ToString(readerMySql["Nom"]);
            //        client.Prenom = Convert.ToString(readerMySql["Prenom"]);
            //        client.Adresse1 = Convert.ToString(readerMySql["Adresse1"]);
            //        client.Adresse2 = Convert.ToString(readerMySql["Adresse2"]);
            //        client.Adresse3 = Convert.ToString(readerMySql["Adresse3"]);
            //        client.Mobile = Convert.ToString(readerMySql["Mobile"]);
            //        client.Telephone = Convert.ToString(readerMySql["Telephone"]);
            //        clients.Add(client);
            //    }

            //}
            return clients;
        }
    }
}
