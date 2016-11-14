using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modèles
{
    public class Projet
    {
        public Client Client { get; set; }
        public Commercial Commercial { get; set; }
        public DateTime CreateDate { get; set; }
        public int Id { get; set; }

        public string Nom { get; set; }
        public virtual ICollection<Produit> Produits { get; set; }
        public string Reference { get; set; }
        public DateTime UpdateDate { get; set; }
    }
    public interface IProjet
    {
        bool CreerProjet(Projet projet);
        Projet SelectionnerProjet(string nomProjet);
        List<Projet> ChargerProjets();
    }

    public class Projets : IProjet
    {
        public List<Projet> ChargerProjets()
        {
            try
            {
                var listeProjetEnCours = new List<Projet>();
                Console.WriteLine("Connexion BDD");
                MySqlConnection connexion = new MySqlConnection("Server=212.129.41.100;Port=20;Database=HouseMaderaDb;Uid=root;Pwd=Rila2016");
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
                return listeProjetEnCours;
            }
            catch (MySqlException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return null;
            }
        }

        public bool CreerProjet(Projet projet)
        {
            try
            {
                Console.WriteLine("Connexion BDD");
                MySqlConnection connexion = new MySqlConnection("Server=212.129.41.100;Port=20;Database=HouseMaderaDb;Uid=root;Pwd=Rila2016");
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

        public Projet SelectionnerProjet(string nomProjet)
        {
            throw new NotImplementedException();
            // TODO
        }
    }

    public class DesignNouveauProjet : IProjet
    {
        public List<Projet> ChargerProjets()
        {
            throw new NotImplementedException();
        }

        public bool CreerProjet(Projet projet)
        {
            try
            {
                return true;
            }
            catch (MySqlException)
            {
                Console.WriteLine("Timeout connexion bdd");
                return false;
            }
        }

        public Projet selectionnerProjet(string nomProjet)
        {
            throw new NotImplementedException();
        }

        public Projet SelectionnerProjet(string nomProjet)
        {
            Projet projetDesign = new Projet();
            projetDesign.Nom = "projet test";
            projetDesign.Reference = "test";
            return projetDesign;
        }
    }

}