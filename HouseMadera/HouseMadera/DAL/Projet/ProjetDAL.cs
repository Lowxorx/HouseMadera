using HouseMadera.Modeles;
using HouseMadera.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HouseMadera.DAL
{
    public class ProjetDAL : DAL
    {
        public ProjetDAL(string nomBdd) : base(nomBdd)
        {

        }

        #region READ

        /// <summary>
        /// Selectionne tous les projets enregistrés en base
        /// </summary>
        /// <returns>Une liste d'objets Projet</returns>
        public ObservableCollection<Projet> ChargerProjets()
        {
            ObservableCollection<Projet> listeProjetEnCours = new ObservableCollection<Projet>();
            try
            {
                string sql = @"SELECT p.*, c.Id AS com_id, c.Nom AS com_nom, c.Prenom AS com_prenom, cli.Id AS cli_id, cli.Nom AS cli_nom, cli.Prenom AS cli_prenom
                               FROM Projet p
                               LEFT JOIN Commercial c ON p.Commercial_Id=c.Id
                               LEFT JOIN Client cli ON p.Client_Id=cli.Id";
                var reader = Get(sql, null);
                while (reader.Read())
                {
                    Projet p = new Projet()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nom = Convert.ToString(reader["Nom"]),
                        Reference = Convert.ToString(reader["Reference"]),
                        CreateDate = Convert.ToDateTime(reader["CreateDate"]),
                        UpdateDate = Convert.ToDateTime(reader["UpdateDate"]),
                        Commercial = new Commercial()
                        {
                            Id = Convert.ToInt32(reader["com_id"]),
                            Nom = Convert.ToString(reader["com_nom"]),
                            Prenom = Convert.ToString(reader["com_prenom"])
                        },
                        Client = new Client()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("cli_id")),
                            Nom = Convert.ToString(reader["cli_nom"]),
                            Prenom = Convert.ToString(reader["cli_prenom"])
                        }
                    };
                    listeProjetEnCours.Add(p);
                }
                reader.Close();
                return listeProjetEnCours;
            }
            catch (MySqlException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return null;
            }
        }

        /// <summary>
        /// Selectionne le premier projet avec l'ID du projet en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet Projet</returns>
        public Projet SelectionnerProjet(string nomProjet)
        {
            try
            {
                Modeles.Projet p = new Modeles.Projet();
                Console.WriteLine("Connexion BDD");
                string sql = @"SELECT * FROM Projet WHERE Nom=@1";
                var parameters = new Dictionary<string, object>
                {
                    {"@1", nomProjet }
                };
                var reader = Get(sql, null);
                while (reader.Read())
                {
                    p.Nom = reader.GetString(reader.GetOrdinal("Nom"));
                    p.Reference = reader.GetString(reader.GetOrdinal("Reference"));
                    p.UpdateDate = reader.GetDateTime(reader.GetOrdinal("UpdateDate"));
                    p.CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                    using (ClientDAL cDal = new ClientDAL(Bdd))
                    {
                        p.Client = cDal.GetClient(reader.GetOrdinal("Client_Id"));
                    }
                    p.Commercial = CommercialDAL.GetCommercial(Convert.ToInt32(reader.GetOrdinal("Commercial_Id")));
                }
                reader.Close();
                return p;
            }
            catch (MySqlException)
            {
                Logger.WriteTrace("Timeout connexion bdd");
                return null;
            }
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Réalise des test sur les propriétés de l'objet Client
        /// avant insertion en base.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Le nombre de ligne affecté en base. -1 si aucune ligne insérée</returns>
        public bool CreerProjet(Projet projet)
        {
            try
            {
                Console.WriteLine("Connexion BDD");
                MySqlConnection connexion = new MySqlConnection("Server=212.129.41.100;Port=16081;Database=HouseMaderaDb;Uid=root;Pwd=Rila2016");
                connexion.Open();
                MySqlCommand command = connexion.CreateCommand();
                Console.WriteLine("Requete BDD");
                command.CommandText = "SELECT * FROM Projets";
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

        #endregion

        #region UPDATE

        #endregion

        #region DELETE

        #endregion
    }
}
