using MySql.Data.MySqlClient;
using System;

namespace HouseMadera.Modèles
{
    public class Commercial
    {
        public string NomUtilisateur { get; set; }
        public string Password { get; set; }
    }

    public interface ICommercialConnect
    {
        bool Connect(Commercial commercial);
    }

    public class CommercialConnect : ICommercialConnect
    {
        public bool Connect(Commercial commercial)
        {
            try
            {
                Console.WriteLine("Connexion BDD");
                MySqlConnection connexion = new MySqlConnection("Server=212.129.41.100;Port=20;Database=HouseMaderaDb;Uid=root;Pwd=Rila2016");
                connexion.Open();
                MySqlCommand command = connexion.CreateCommand();
                Console.WriteLine("Requete BDD");
                command.CommandText = "SELECT * FROM Commercials WHERE Login = '" + commercial.NomUtilisateur + "' AND Password = '" + commercial.Password + "'";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0}", reader[0]));
                }
                reader.Close();
                connexion.Close();
                return true;
            }
            catch (MySqlException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return false;
            }
        }
    }

    public class DesignCommercialConnect : ICommercialConnect
    {
        public bool Connect(Commercial commercial)
        {
            return true;
        }
    }


}
