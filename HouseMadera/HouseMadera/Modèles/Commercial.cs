using HouseMadera.Modèles;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Commercial
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
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
                var newConnexion = new MysqlConnector();
                var connexion = newConnexion.Connect();
                if (connexion != null)
                {
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
                else
                {
                    return false;
                }

            }
            catch (MySqlException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return false;
            }
        }
    }
}
