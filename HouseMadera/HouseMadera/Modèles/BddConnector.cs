using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System;

namespace HouseMadera.Modèles
{
    public class BddConnector
    {

        public SQLiteConnection SqliteConnectToBdd()
        {
            try
            {
                Console.WriteLine("Connexion BDD");
                SQLiteConnection connexion = new SQLiteConnection("Data Source=Resources/HouseMaderaSqlite.db");
                connexion.Open();
                Console.WriteLine("Connexion ouverte");
                return connexion;
            }
            catch (MySqlException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return null;
            }
        }

        public MySqlConnection MyConnectToBdd()
        {
            try
            {
                Console.WriteLine("Connexion BDD");
                MySqlConnection connexion = new MySqlConnection("Server=212.129.41.100;Port=16081;Database=HouseMaderaDb;Uid=root;Pwd=Rila2016");
                connexion.Open();
                Console.WriteLine("Connexion ouverte");
                return connexion;
            }
            catch (MySqlException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return null;
            }
        }
    }
}
