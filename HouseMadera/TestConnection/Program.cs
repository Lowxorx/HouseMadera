
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilites;
using System;
using System.Collections.Generic;

namespace TestConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            //SQLiteConnection dbSqlite = new SQLiteConnection("Data Source=C:/Users/remy/Documents/HouseMaderaDB-sqlite/HouseMaderaDB.db;version=3");
            //dbSqlite.Open();
            //string sql = "select * from Client order by Nom desc";
            //SQLiteCommand commandSQlite = new SQLiteCommand(sql, dbSqlite);
            //SQLiteDataReader readerSQLite = commandSQlite.ExecuteReader();
            //Console.WriteLine("**************************SQLite******************************");
            //while (readerSQLite.Read())
            //    Console.WriteLine("Nom: " + readerSQLite["Nom"] + "\tPrenom: " + readerSQLite["Prenom"] + "\tAdresse: " + readerSQLite["Adresse1"]);
            //dbSqlite.Close();

            //var dbMysql = new MySqlConnection("Server=212.129.41.100;Port=16081;Uid=root;Pwd=Rila2016;Database=HouseMaderaDb;CharSet=utf8;");
            //dbMysql.Open();
            //var query = "select * from Client order by Nom desc";
            //MySqlCommand commandMySql = new MySqlCommand(query, dbMysql);
            //MySqlDataReader readerMySql = commandMySql.ExecuteReader();
            //Console.WriteLine("**************************MYSQL******************************");
            //while (readerMySql.Read())
            //    Console.WriteLine("Nom: " + readerMySql["Nom"] + "\tPrenom: " + readerMySql["Prenom"] + "\tAdresse: " + readerMySql["Adresse1"]);
            //dbMysql.Close();
            //Console.ReadLine();

            //Le nom de la base de donnée utilisée est connue lors du test de connexion en début de programme
            //Il faudra le remplacer par une variable 
            var testConnection = new ConnectivityMonitor();
            var isOnline = testConnection.IsOnline();
            var bdd = isOnline ? "MYSQL" : "SQLITE";
            Console.WriteLine("Etat de la connexion\n En ligne ? : {0}\n Bdd choisie :{1}",isOnline,bdd);
            Console.ReadLine();
           using (var dal = new ClientDAL(bdd))
            {
                //    ////***************************************** GET
                //    var clientId = 1;

                //    try
                //    {
                //        var client = dal.GetClient(clientId);
                //        if (client != null)
                //            Console.WriteLine("Nom: " + client.Nom + "\nPrenom: " + client.Prenom + "\nAdresse: " + client.Adresse1);
                //        else
                //            Console.WriteLine("Aucun client enregistré sous l'ID {0}",clientId);
                //    }
                //    catch(Exception e)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //    Console.ReadLine();

                try
                {
                    var clients = new List<Client>();
                    clients = dal.GetAllClients();
                    if (clients != null)
                    {
                        foreach (var client in clients)
                        {
                            Console.WriteLine("Nom: " + client.Nom + "\nPrenom: " + client.Prenom + "\nAdresse: " + client.Adresse1);
                            Console.WriteLine("##########################");
                        }
                        Console.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.ReadLine();



                
                //*******************************************INSERT
                // const int ACTIF = 1;
                // const int INACTIF = 2;


                //var nouveauClient = new Client()
                //{
                //    Nom = "Delmas",
                //    Prenom = "Daniel",
                //    Adresse1 = "50, chemin de jansau",
                //    CodePostal = "81600",
                //    Ville = "Gaillac",
                //    Email = "md.delmas@free.fr",
                //    Telephone = "0563576351",
                //    StatutClient = INACTIF
                //};

                //var success = 0;
                //try
                //{
                //    success = dal.InsertClient(nouveauClient);
                //    Console.WriteLine("*************************************");
                //    Console.WriteLine("{0} nouveau client enregistré", success);
                //    Console.ReadLine();
                //}
                //catch(Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    Console.ReadLine();
                //}


                //***********************DELETE
                //var success = 0;
                //try
                //{
                //    var clientId = 2;
                //    success = dal.DeleteClient(clientId);
                //    Console.WriteLine("*************************************");
                //    Console.WriteLine("Le client enregistré avec l'ID : {0} a été supprimé de la base", clientId);
                //    Console.ReadLine();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    Console.ReadLine();
                //}

                //*****************************UPDATE
                //var majClient = new Client()
                //{
                //    Id = 3,
                //    Nom = "Bond",
                //    Prenom = "James",
                //    Adresse1 = "7, rue majeste",
                //    CodePostal = "81000",
                //    Ville = "Albi",
                //    Email = "james.bond@free.fr",
                //    Telephone = "0563576351",
                //    StatutClient = ACTIF
                //};

                //var success = 0;
                //try
                //{
                //    success = dal.UpdateClient(majClient);
                //    Console.WriteLine("*************************************");
                //    Console.WriteLine("{0} client mis à jour", success);
                //    Console.ReadLine();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    Console.ReadLine();
                //}

                };



            }
    }
}
