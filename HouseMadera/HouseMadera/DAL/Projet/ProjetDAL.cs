using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;

namespace HouseMadera.DAL.Projet
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
        public static ObservableCollection<Modeles.Projet> ChargerProjets()
        {
            ObservableCollection<Modeles.Projet> listeProjetEnCours = new ObservableCollection<Modeles.Projet>();
            try
            {
                Console.WriteLine("Connexion BDD");
                string sql = @"SELECT * FROM Projet";
                var reader = Get(sql, null);
                while (reader.Read())
                {
                    Modeles.Projet p = new Modeles.Projet() { Nom = reader.GetString(reader.GetOrdinal("Nom")) };
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

        public Modeles.Projet SelectionnerProjet(string nomProjet)
        {
            throw new NotImplementedException();
            // TODO
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
