using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Commercial
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nom { get; set; }
        public string Password { get; set; }
        public string Prenom { get; set; }
        public virtual ICollection<Projet> Projets { get; set; }
    }
    public interface ICommercial
    {
        bool Connect(Commercial commercial);
    }

    public class CommercialConnect : ICommercial
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
                command.CommandText = "SELECT * FROM Commercials WHERE Login = '" + commercial.Login + "' AND Password = '" + commercial.Password + "'";
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
}
