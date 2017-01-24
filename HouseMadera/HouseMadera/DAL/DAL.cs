using MySql.Data.MySqlClient;
using System;
using System.Data.SQLite;


namespace HouseMadera.DAL
{
    public class DAL : IDisposable
    {
        private const string CONNEXION_MYSQL = "Server=212.129.41.100;Port=16081;Uid=root;Pwd=Rila2016;Database=HouseMaderaDb;CharSet=utf8;";
        private const string CONNEXION_SQLITE = "Data Source=C:/Users/remy/Documents/HouseMaderaDB-sqlite/HouseMaderaDB.db;version=3";

        public string Bdd { get; set; }
        public MySqlConnection BddMySql { get; set; }
        public SQLiteConnection BddSQLite { get; set; }
        public MySqlCommand CommandeMySql { get; set; }
        public SQLiteCommand CommandeSQLite { get; set; }
        public MySqlDataReader ReaderMySql { get; set; }
        public SQLiteDataReader ReaderSQLite { get; set; }

        public DAL(string nomBdd)
        {
            Bdd = nomBdd;
            BddMySql = null;
            BddSQLite = null;

        }

        public void Initialiser()
        {

            try
            {
                //Suivant la base de donnée utilisée on créé la connexion
                switch (Bdd)
                {
                    case "MYSQL":
                        BddMySql = new MySqlConnection(CONNEXION_MYSQL);
                        BddMySql.Open();
                        break;
                    case "SQLITE":
                        BddSQLite = new SQLiteConnection(CONNEXION_SQLITE);
                        BddSQLite.Open();
                        break;
                    default: break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DAL.Initialiser() :\t" + e.Message);
            }
        }

        public void Command(string requete)
        {
            try
            {
                switch (Bdd)
                {
                    case "MYSQL":
                        CommandeMySql = new MySqlCommand(requete, BddMySql);
                        break;

                    case "SQLITE":
                        CommandeSQLite = new SQLiteCommand(requete, BddSQLite);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DAL.Command() :\t" + e.Message);
            }
        }

        private void Reader()
        {
            try
            {
                switch (Bdd)
                {
                    case "MYSQL":
                        if (CommandeMySql != null)
                           ReaderMySql = CommandeMySql.ExecuteReader();
                        break;
                       
                    case "SQLITE":
                        if (CommandeSQLite != null)
                            ReaderSQLite = CommandeSQLite.ExecuteReader();
                        break;
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine("DAL.Reader() :\t" + e.Message);
            }
           
        }


        

        public void Dispose()
        {
            if (BddMySql != null)
                BddMySql.Close();
            if (BddSQLite != null)
                BddSQLite.Close();
        }
    }
}

