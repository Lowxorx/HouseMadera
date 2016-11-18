using MySql.Data.MySqlClient;
using System;

namespace HouseMadera.Modèles
{
    public class MysqlConnector
    {
        private MySqlConnection MyConnectToBdd()
        {
            try
            {
                Console.WriteLine("Connexion BDD");
                MySqlConnection connexion = new MySqlConnection("Server=212.129.41.100;Port=20;Database=HouseMaderaDb;Uid=root;Pwd=Rila2016");
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

        public MySqlConnection Connect()
        {
            var co = MyConnectToBdd();
            return co;
        }
    }
}