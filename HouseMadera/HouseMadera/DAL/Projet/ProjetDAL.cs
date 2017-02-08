using HouseMadera.Modeles;
using HouseMadera.Utilites;
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
                Console.WriteLine("Connexion BDD");
                string sql = @"SELECT * FROM Projet";
                var reader = Get(sql, null);
                while (reader.Read())
                {
                    Projet p = new Projet()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nom = reader.GetString(reader.GetOrdinal("Nom")),
                        Reference = reader.GetString(reader.GetOrdinal("Reference")),
                        CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                        UpdateDate = reader.GetDateTime(reader.GetOrdinal("UpdateDate"))
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
        public static Projet SelectionnerProjet(string nomProjet)
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
                    p.Client = ClientDAL.GetClient(reader.GetOrdinal("Client_Id"));
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
        public bool CreerProjet(Modeles.Projet projet)
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
