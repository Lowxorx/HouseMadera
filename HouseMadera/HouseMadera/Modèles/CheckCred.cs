using MySql.Data.MySqlClient;
using System;

namespace HouseMadera.Modèles
{
    public class CheckCred
    {
        public bool VerifLogin(Commercial commercial)
        {
            MySqlConnection connexion = new MySqlConnection("Server=212.129.41.100;Port=16081;Database=HouseMaderaDb;Uid=root;Pwd=Rila2016");
            connexion.Open();
            MySqlCommand command = connexion.CreateCommand();
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
    }
}
