using HouseMadera.Modeles;
using HouseMadera.Utilites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HouseMadera.DAL
{
    public class DevisDAL : DAL
    {

        const string NON_RENSEIGNE = "NULL";

        public DevisDAL(string nomBdd) : base(nomBdd)
        {
            // Constructeur par défaut de la classe DevisDAL
        }

        #region READ

        /// <summary>
        /// Selectionne tous les devis enregistrés en base
        /// </summary>
        /// <returns>Une liste d'objets Devis</returns>
        public List<Devis> GetAllDevis()
        {
            string sql = @"
                            SELECT * FROM Devis
                            ORDER BY Nom DESC";
            var listeDevis = new List<Devis>();
            var reader = Get(sql, null);
            while (reader.Read())
            {
                var devis = new Devis();
                devis.Id = Convert.ToInt32(reader["Id"]);
                devis.Nom = Convert.ToString(reader["Nom"]);
                devis.DateCreation = Convert.ToDateTime(reader["DateCreation"]);
                devis.PrixHT = Convert.ToDecimal(reader["PrixHT"]);
                devis.PrixTTC = Convert.ToDecimal(reader["PrixTTC"]);
                listeDevis.Add(devis);
            }
            return listeDevis;
        }

        public List<DataGenerationDevis> GenererDevis(Produit p)
        {
            var sql = @"SELECT mp.*, m.Nom AS module_nom, c.Prix AS compo_prix, cm.Nombre AS compomod_nbre, c.Nom AS compo_nom, cli.Id AS cli_id, cli.Nom AS cli_nom, cli.Prenom AS cli_prenom, cli.Email AS cli_mail
                        FROM ModulePlace mp
                        LEFT JOIN Module m ON mp.Module_Id = m.Id
                        LEFT JOIN Produit p ON p.Id = mp.Produit_Id
                        LEFT JOIN Projet proj ON p.Projet_Id = proj.Id
                        LEFT JOIN Client cli ON proj.Client_Id = cli.Id
                        LEFT JOIN ComposantModule cm ON cm.Module_Id = m.Id
                        LEFT JOIN Composant c ON c.Id = cm.Composant_Id
                        WHERE mp.Produit_Id = @1";
            var parameters = new Dictionary<string, object>()
            {
                {"@1", p.Id }
            };
            var reader = Get(sql, parameters);

            var listeDevis = new List<DataGenerationDevis>();

            while (reader.Read())
            {
                DataGenerationDevis dg = new DataGenerationDevis()
                {
                    NumModule = Convert.ToInt32(reader["Module_Id"]),
                    NomModule = Convert.ToString(reader["module_nom"]),
                    NomComposant = Convert.ToString(reader["compo_nom"]),
                    NombreComposant = Convert.ToInt32(reader["compomod_nbre"]),
                    PrixComposant = Convert.ToString(reader["compo_prix"]),
                    client = new Client
                    {
                        Id = Convert.ToInt32(reader["cli_id"]),
                        Nom = Convert.ToString(reader["cli_nom"]),
                        Prenom = Convert.ToString(reader["cli_prenom"]),
                        Email = Convert.ToString(reader["cli_mail"])
                    }

                };
                listeDevis.Add(dg);
            }
            reader.Close();
            return listeDevis;
        }


        /// <summary>
        /// Selectionne tous les devis associés au projet en paramètre
        /// </summary>
        /// <param name="projet"></param>
        /// <returns>Une liste d'objets Devis</returns>
        public static ObservableCollection<Devis> GetAllDevisByproject(Projet p)
        {
            string sql = @"SELECT * FROM Devis WHERE ";
            ObservableCollection<Devis> listeDevis = new ObservableCollection<Devis>();
            var reader = Get(sql, null);
            while (reader.Read())
            {
                var devis = new Devis()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nom = Convert.ToString(reader["Nom"]),
                    DateCreation = Convert.ToDateTime(reader["DateCreation"]),
                    PrixHT = Convert.ToDecimal(reader["PrixHT"]),
                    PrixTTC = Convert.ToDecimal(reader["PrixTTC"])
                };
                listeDevis.Add(devis);
            }
            return listeDevis;
        }

        /// <summary>
        /// Selectionne le premier devis avec l'ID en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet Devis</returns>
        public Devis GetDevis(int id)
        {

            string sql = @"SELECT * FROM Devis WHERE Id = @1";
            var parametres = new Dictionary<string, object>()
            {
                {"@1", id}
            };
            var reader = Get(sql, parametres);
            var devis = new Devis();
            while (reader.Read())
            {
                devis.Id = Convert.ToInt32(reader["Id"]);
                devis.Nom = Convert.ToString(reader["Nom"]);
                devis.DateCreation = Convert.ToDateTime(reader["DateCreation"]);
                devis.PrixHT = Convert.ToDecimal(reader["PrixHT"]);
                devis.PrixTTC = Convert.ToDecimal(reader["PrixTTC"]);
                devis.StatutDevis = new StatutDevis() { Id = Convert.ToInt32(reader["StatutDevis_Id"]) };
            }
            return devis;
        }

        /// <summary>
        /// Vérifie en interrogeant la base si un devis est déjà enregistré
        /// </summary>
        /// <param name="devis"></param>
        /// <returns>"true" si le devis existe déjà en base</returns>
        private bool IsDevisExist(Devis devis)
        {
            var result = false;
            string sql = @"SELECT * FROM Devis WHERE Nom=@1 AND PrixHT=@2 AND PrixTTC=@3 OR DateCreation=@4";
            var parameters = new Dictionary<string, object> {
                {"@1",devis.Nom },
                {"@2",devis.PrixHT },
                {"@3",devis.PrixTTC },
                {"@4",devis.DateCreation }
            };
            var listeDevis = new List<Devis>();
            using (var reader = Get(sql, parameters))
            {
                while (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Réalise des test sur les propriétés de l'objet Devis
        /// avant insertion en base.
        /// </summary>
        /// <param name="devis"></param>
        /// <returns>Le nombre de ligne affecté en base. -1 si aucune ligne insérée</returns>
        public int InsertDevis(Devis devis)
        {
            if (IsDevisExist(devis))
                throw new Exception("Le devis est déjà enregistré.");

            var sql = @"INSERT INTO Devis (Nom,DateCreation,PrixHT,PrixTTC,StatutDevis_Id) VALUES(@1,@2,@3,@4,@5)";
            var parameters = new Dictionary<string, object>() {
                {"@1",devis.Nom },
                {"@2",devis.DateCreation },
                {"@3",devis.PrixHT },
                {"@4",devis.PrixTTC },
                {"@5",devis.StatutDevis }
            };
            var result = 0;
            try
            {
                result = Insert(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Logger.WriteEx(e);
                Console.WriteLine(e.Message);
            }

            return result;
        }

        #endregion

        #region UPDATE

        #endregion

        #region DELETE

        /// <summary>
        /// Efface en base le devis avec l'Id en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Le nombre de ligne affecté en base. -1 si aucune ligne affectée</returns>
        public int DeleteDevis(int id)
        {
            var sql = @"
                        DELETE FROM Devis 
                        WHERE Id=@1
                      ";
            var parameters = new Dictionary<string, object>() {
                {"@1",id },
            };
            var result = 0;
            try
            {
                result = Delete(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Logger.WriteEx(e);
            }
            return result;
        }

        #endregion

    }
}
