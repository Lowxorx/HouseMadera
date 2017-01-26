

using HouseMadera.Utilites;
using System;
using System.Collections.Generic;
using HouseMadera.Modeles;

namespace HouseMadera.DAL
{
    public class ClientDAL : DAL
    {
        public ClientDAL(string nomBdd) : base(nomBdd)
        {
            
        }

        public List<Client> GetAllClients()
        {
            string sql = "select * from Client_view order by Nom desc";
            var clients = new List<Client>();
            var reader = Get(sql, null);
            while (reader.Read())
            {
                var client = new Client();
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

        public int InsertClient( Client client)
        {

            //statutClient ne doit pas être null
            if (client.StatutClient > 2 ||client.StatutClient==0)
                throw new Exception("Le client n'a pas de statut");
            var utils = new RegexUtilities();
            //Test de validite de l'email
            if (!utils.IsValidEmail(client.Email))
                throw new Exception("L'email n'est pas au bon format");
            //Test de validité du numéro de téléphone
            if (!utils.IsValidTelephoneNumber(client.Telephone))
                throw new Exception("le numero de téléphone devrait être 0xxxxxxxxx");
            if (IsClientExist(client))
                throw new Exception("le client est déjà enregistré");


            var sql = @"
                        INSERT INTO Client (Nom,Prenom,Adresse1,Adresse2,Adresse3,CodePostal,Ville,Email,Telephone,Mobile,StatutClient_Id)
                        VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)";
            var parameters = new Dictionary<string, object>() {
                {"@1",client.Nom },
                {"@2",client.Prenom },
                {"@3",client.Adresse1 },
                {"@4",client.Adresse2 },
                {"@5",client.Adresse3 },
                {"@6",client.CodePostal },
                {"@7",client.Ville },
                {"@8",client.Email },
                {"@9",client.Telephone },
                {"@10",client.Mobile },
                {"@11",client.StatutClient },
            };
            var result = 0;
            try
            {
                result = Insert(sql, parameters);
            }
            catch(Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        /// <summary>
        /// Vérifie en interrogeant la base si un client est déjà enregistré
        /// </summary>
        /// <param name="client"></param>
        /// <returns>"true" si le client existe déjà en base</returns>
        private bool IsClientExist( Client client)
        {
            var result = false;
            string sql = @"SELECT * FROM Client WHERE Nom=@1 AND Prenom=@2 AND Mobile=@3 OR Telephone=@4 AND Email=@5";
            var parameters = new Dictionary<string, object> {
                {"@1",client.Nom },
                {"@2",client.Prenom },
                {"@3",client.Mobile },
                {"@4",client.Telephone },
                {"@5",client.Email }

            };
            var clients = new List<Client>();
            using (var reader = Get(sql, parameters))
            {
                while (reader.Read())
                {
                    result = true;
                }
            }
               
            return result;
        }
    }
}
