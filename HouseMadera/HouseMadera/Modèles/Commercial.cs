using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;

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
    public class Commerciaux
    {
        public static ObservableCollection<Commercial> ChargerCommerciaux()
        {
            ObservableCollection<Commercial> listeCommerciaux = new ObservableCollection<Commercial>();
            try
            {
                Console.WriteLine("Connexion BDD");
                BddConnector co = new BddConnector();
                if (ConnectivityMonitor.ConnectivityStatus)
                {
                    var connexion = co.MyConnectToBdd();
                    if (connexion == null)
                    {
                        Console.WriteLine("connect bdd fail");
                        return null;
                    }
                    else
                    {
                        MySqlCommand command = connexion.CreateCommand();
                        Console.WriteLine("Requete BDD");
                        command.CommandText = "SELECT * FROM Commercial";
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Commercial c = new Commercial() { Nom = reader.GetString(reader.GetOrdinal("Nom")), Prenom = reader.GetString(reader.GetOrdinal("Prenom")) };
                            listeCommerciaux.Add(c);
                        }
                        reader.Close();
                        connexion.Close();
                        return listeCommerciaux;
                    }
                }
                else
                {
                    var connexion = co.SqliteConnectToBdd();
                    connexion.Close();
                    return listeCommerciaux;
                }            
            }
            catch (Exception ex)
            {
                if (ex is MySqlException || ex is SQLiteException)
                {
                    Console.WriteLine("Timeout connexion bdd");
                    return null;
                }

                return null;
            }
        }
    }

    public class CommercialConnect
    {

        public string Connect(Commercial commercial)
        {
            // Retourne un chiffre contenant le résultat : 0 pour un succès, 1 pour un utilisateur incorrect, 2 pour un échec de connexion à la bdd.
            try
            {
                Console.WriteLine("Connexion BDD pour login commercial");
                BddConnector co = new BddConnector();
                Console.WriteLine("Vérification de la connexion");
                ConnectivityMonitor checkCo = new ConnectivityMonitor();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DataSet dataset = new DataSet();

                if (checkCo.IsOnline())
                {
                    Console.WriteLine("Connecté au réseau");
                    var connexion = co.MyConnectToBdd();
                    if (connexion == null)
                    {
                        return "2";
                    }
                    else
                    {
                        Console.WriteLine("connecté à la bdd");
                        Console.WriteLine("Requete BDD");
                        
                        adapter.SelectCommand = new MySqlCommand("SELECT * FROM Commercial WHERE Login = ? AND Password = ?", connexion);
                        adapter.SelectCommand.Parameters.AddWithValue("@Username", commercial.Login);
                        adapter.SelectCommand.Parameters.AddWithValue("@Password", commercial.Password);

                        adapter.Fill(dataset);
                        bool loginResult = dataset.Tables.Cast<DataTable>()
                               .Any(table => table.Rows.Count != 0);

                        if (loginResult)
                        {
                            return "0";
                        }
                        else
                        {
                            return "1";
                        }

                    }
                }
                else
                {
                    var connexion = co.SqliteConnectToBdd();
                    // sqlite connect

                    connexion.Close();
                    return "0";
                }
            }
            catch (MySqlException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return "2";
            }
            catch (SQLiteException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return "2";
            }
        }
    }
}
