using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;

namespace HouseMadera.DAL
{
    public class DevisDAL : DAL, IDAL<Devis>
    {

        const string NON_RENSEIGNE = "NULL";

        public DevisDAL(string nomBdd) : base(nomBdd)
        {
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

        public List<DataGenerationDevis> GenererDevis(Produit p)
        {
            var sql = @"SELECT mp.*, m.Nom AS module_nom, p.Nom AS nom_produit, c.Prix AS compo_prix, cm.Nombre AS compomod_nbre, c.Nom AS compo_nom, cli.Id AS cli_id, cli.Nom AS cli_nom, cli.Prenom AS cli_prenom, cli.Email AS cli_mail
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
                    NomProduit = Convert.ToString(reader["nom_produit"]),
                    NumModule = Convert.ToInt32(reader["Module_Id"]),
                    NomModule = Convert.ToString(reader["module_nom"]),
                    NomComposant = Convert.ToString(reader["compo_nom"]),
                    NombreComposant = string.IsNullOrEmpty(reader["compomod_nbre"].ToString()) ? 0:Convert.ToInt32(reader["compomod_nbre"]),
                    PrixComposant = Convert.ToString(reader["compo_prix"]),
                    Client = new Client
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
            Devis devis = new Devis();
            var reader = Get(sql, parametres);
            while (reader.Read())
            {
                 devis = new Devis()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nom = Convert.ToString(reader["Nom"]),
                    DateCreation = Convert.ToDateTime(reader["DateCreation"]),
                    PrixHT = Convert.ToDecimal(reader["PrixHT"]),
                    PrixTTC = Convert.ToDecimal(reader["PrixTTC"]),
                    StatutDevis = new StatutDevis() { Id = Convert.ToInt32(reader["StatutDevis_Id"]) },
                    Pdf = (byte[])reader["pdf"]
                };
            }
            return devis;
        }

        /// <summary>
        /// Selectionne le premier devis avec l'ID produit en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet Devis</returns>
        public Devis GetDevisByIdProduit(Produit p)
        {

            string sql = @"SELECT * FROM Devis WHERE Nom = @1";
            var parametres = new Dictionary<string, object>()
            {
                {"@1", p.Nom}
            };
            Devis devis = new Devis();
            var reader = Get(sql, parametres);
            while (reader.Read())
            {
                devis = new Devis()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nom = Convert.ToString(reader["Nom"]),
                    DateCreation = Convert.ToDateTime(reader["DateCreation"]),
                    PrixHT = Convert.ToDecimal(reader["PrixHT"]),
                    PrixTTC = Convert.ToDecimal(reader["PrixTTC"]),
                    StatutDevis = new StatutDevis() { Id = Convert.ToInt32(reader["StatutDevis_Id"]) },
                    Pdf = (byte[])reader["pdf"]
                };
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

            var sql = @"INSERT INTO Devis (Nom,DateCreation,PrixHT,PrixTTC,StatutDevis_Id,Pdf,MiseAJour,Suppression,Creation) VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9)";
            var parameters = new Dictionary<string, object>() {
                {"@1",devis.Nom },
                {"@2",devis.DateCreation },
                {"@3",devis.PrixHT },
                {"@4",devis.PrixTTC },
                {"@5",devis.StatutDevis.Id },
                {"@6",devis.Pdf },
                {"@7", DateTimeDbAdaptor.FormatDateTime( devis.MiseAJour,Bdd) },
                {"@8", DateTimeDbAdaptor.FormatDateTime( devis.Suppression,Bdd) },
                {"@9", DateTimeDbAdaptor.FormatDateTime( devis.Creation,Bdd) }
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

        #region SYNCHRONISATION

        public List<Devis> GetAllModeles()
        {
            string sql = @"
                            SELECT d.*, sd.Id as Statut_Id , sd.Nom as Statut_libelle
                            FROM Devis d
                            LEFT JOIN StatutDevis sd ON d.StatutDevis_Id=sd.Id";
            List<Devis> listeDevis = new List<Devis>();
            try
            {
                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        var devis = new Devis();
                        devis.Id = Convert.ToInt32(reader["Id"]);
                        devis.Nom = Convert.ToString(reader["Nom"]);
                        devis.DateCreation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["DateCreation"]));
                        devis.PrixHT = Convert.ToDecimal(reader["PrixHT"]);
                        devis.PrixTTC = Convert.ToDecimal(reader["PrixTTC"]);
                       
                        devis.StatutDevis = new StatutDevis()
                        {
                            Id = Convert.ToInt32(reader["Statut_Id"]),
                            Nom = Convert.ToString(reader["Statut_Libelle"])
                        };
                        devis.MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"]));
                        devis.Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"]));
                        devis.Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"]));
                        listeDevis.Add(devis);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeDevis;
        }

        public int InsertModele(Devis devis, MouvementSynchronisation sens)
        {
            //Vérification des clés étrangères
            if (devis.StatutDevis == null)
                throw new Exception("Tentative de mise a jour dans la table Devis avec la clé étrangère StatutDevis nulle");
            int statutDevisId = 0;
            if(sens == MouvementSynchronisation.Sortant)
                Synchronisation<StatutDevisDAL, StatutDevis>.CorrespondanceModeleId.TryGetValue(devis.StatutDevis.Id, out statutDevisId);
            else
                statutDevisId = Synchronisation<StatutDevisDAL, StatutDevis>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == devis.StatutDevis.Id).Key;

            string sql = @"INSERT INTO Devis (Nom,PrixHT,PrixTTC,StatutDevis_Id,pdf,MiseAJour,Suppression,Creation,DateCreation)
                        VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",devis.Nom },
                {"@2",devis.PrixHT },
                {"@3",devis.PrixTTC },
                {"@4",statutDevisId},
                {"@5",devis.Pdf },
                {"@6", DateTimeDbAdaptor.FormatDateTime( devis.MiseAJour,Bdd) },
                {"@7", DateTimeDbAdaptor.FormatDateTime( devis.Suppression,Bdd) },
                {"@8", DateTimeDbAdaptor.FormatDateTime( devis.Creation,Bdd) },
                {"@9", DateTimeDbAdaptor.FormatDateTime( devis.DateCreation,Bdd) },

            };
            int result = 0;
            try
            {
                result = Insert(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public int UpdateModele(Devis devisLocal, Devis devisDistant, MouvementSynchronisation sens)
        {

            //Vérification des clés étrangères
            if (devisDistant.StatutDevis == null)
                throw new Exception("Tentative de mise a jour dans la table Devis avec la clé étrangère StatutDevis nulle");
            int statutDevisId = 0;
            if (sens == MouvementSynchronisation.Sortant)
                Synchronisation<StatutDevisDAL, StatutDevis>.CorrespondanceModeleId.TryGetValue(devisDistant.StatutDevis.Id, out statutDevisId);
            else
                statutDevisId = Synchronisation<StatutDevisDAL, StatutDevis>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == devisDistant.StatutDevis.Id).Key;

            //recopie des données du Devis distant dans le Devis local
            if (devisDistant != null)
                devisLocal.Copy(devisDistant);

            string sql = @"
                        UPDATE Devis
                        SET Nom=@1,PrixHT=@2,PrixTTC=@3,StatutDevis_Id=@4,pdf=@5,MiseAJour=@6,DateCreation=@8
                        WHERE Id=@7
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",devisLocal.Nom},
                {"@2",devisLocal.PrixHT},
                {"@3",devisLocal.PrixTTC},
                {"@4",statutDevisId},
                {"@5",devisLocal.Pdf},
                {"@6",devisLocal.DateCreation },
                //{"@6", DateTimeDbAdaptor.FormatDateTime( devisLocal.MiseAJour,Bdd)},
                {"@7",devisLocal.Id },
                {"@8", DateTimeDbAdaptor.FormatDateTime( devisLocal.DateCreation ,Bdd)}
            };
            int result = 0;
            try
            {
                result = Update(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public int DeleteModele(Devis devis)
        {
            string sql = @"
                        UPDATE Devis
                        SET Suppression= @2
                        WHERE Id=@1
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",devis.Id},
                {"@2",DateTimeDbAdaptor.FormatDateTime(devis.Suppression,Bdd)}
            };

            int result = 0;
            try
            {
                result = Update(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        #endregion

    }
}
