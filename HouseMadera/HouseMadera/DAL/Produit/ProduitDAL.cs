using System;
using System.Collections.Generic;
using HouseMadera.Modeles;
using System.Collections.ObjectModel;
using HouseMadera.Utilites;
using HouseMadera.Utilities;
using System.Data.Common;
using System.Linq;

namespace HouseMadera.DAL
{
    public class ProduitDAL : DAL, IDAL<Produit>
    {

        public ProduitDAL(string nomBdd) : base(nomBdd)
        {

        }

        /// <summary>
        /// Selectionne tous les produits correspondant à un Projet
        /// </summary>
        /// <returns>Une liste d'objets Produit</returns>
        public ObservableCollection<Produit> GetAllProduitsByProjet(Projet p)
        {
            ObservableCollection<Produit> listeProduit = new ObservableCollection<Produit>();
            try
            {
                string sql = @"SELECT p.*, sp.Nom AS statut_produit,d.Id AS id_devis, d.Nom AS nom_devis, d.PrixTTC AS prixttc_devis, d.PrixHT AS prixht_devis,sd.Nom AS statut_devis , pl.Nom AS nom_plan, pl.CreateDate AS date_plan, pr.Nom AS nom_projet
                               FROM Produit p 
                               LEFT JOIN Devis d ON p.Devis_Id=d.Id
                               LEFT JOIN StatutDevis sd ON d.StatutDevis_Id = sd.Id
                               LEFT JOIN StatutProduit sp ON p.StatutProduit_Id=sp.Id
                               LEFT JOIN Plan pl ON p.Plan_Id=pl.Id 
                               LEFT JOIN Projet pr ON p.Projet_Id=pr.Id 
                               WHERE Projet_Id=@1";

                var parametres = new Dictionary<string, object>()
                {
                    {"@1", p.Id}
                };
                var reader = Get(sql, parametres);
                while (reader.Read())
                {
                    var produit = new Produit();
                    produit.Id = Convert.ToInt32(reader["Id"]);
                    produit.Nom = Convert.ToString(reader["Nom"]);
                    produit.Devis = new Devis()
                    {
                        Nom = Convert.ToString(reader["nom_devis"]),
                        Id = Convert.ToInt32(reader["id_devis"]),
                        PrixTTC = Convert.ToDecimal(reader["prixttc_devis"]),
                        PrixHT = Convert.ToDecimal(reader["prixht_devis"]),
                        StatutDevis = new StatutDevis() { Nom = reader["nom_statutDevis"].ToString() }
                    };
                    produit.Plan = new Plan()
                    {
                        Nom = Convert.ToString(reader["nom_plan"]),
                        CreateDate = Convert.ToDateTime(reader["date_plan"])
                    };
                    produit.Projet = new Projet()
                    {
                        Nom = Convert.ToString(reader["nom_projet"])
                    };
                    produit.StatutProduit = new StatutProduit() { Nom = reader["statut_produit"].ToString() };
                    listeProduit.Add(produit);
                }
                return listeProduit;
            }
            catch (Exception e)
            {
                Logger.WriteEx(e);
                return null;
            }
        }

        /// <summary>
        /// Selectionne le premier client avec l'ID en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet Client</returns>
        public static Client GetClient(int id)
        {

            string sql = @"
                            SELECT * FROM Client
                            WHERE Id = @1";
            var parametres = new Dictionary<string, object>()
            {
                {"@1", id}
            };
            var reader = Get(sql, parametres);
            var client = new Client();
            while (reader.Read())
            {
                client.Id = Convert.ToInt32(reader["id"]);
                client.Nom = Convert.ToString(reader["nom"]);
                client.Prenom = Convert.ToString(reader["prenom"]);
                client.Adresse1 = Convert.ToString(reader["adresse1"]);
                client.Adresse2 = Convert.ToString(reader["adresse2"]);
                client.Adresse3 = Convert.ToString(reader["adresse3"]);
                client.Mobile = Convert.ToString(reader["mobile"]);
                client.Telephone = Convert.ToString(reader["telephone"]);
            }
            return client;

        }

        /// <summary>
        /// Vérifie en interrogeant la base si un client est déjà enregistré
        /// </summary>
        /// <param name="client"></param>
        /// <returns>"true" si le client existe déjà en base</returns>
        private bool IsClientExist(Client client)
        {
            var result = false;
            string sql = @"SELECT * FROM Client WHERE Nom=@1 AND Prenom=@2 AND Mobile=@3 OR Telephone=@4 AND Email=@5";
            var parameters = new Dictionary<string, object> {
                {"@1",client.Nom },
                {"@2",client.Prenom },
                {"@3",client.Mobile },
                {"@4",client.Telephone },
                {"@5",client.Email }

            };
            var clients = new List<Client>();
            using (var reader = Get(sql, parameters))
            {
                while (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        #region SYNCHRONISATION
        public List<Produit> GetAllModeles()
        {
            List<Produit> listeProduits = new List<Produit>();
            try
            {

                string sql = @"SELECT p.*,
                               d.Id AS devis_id, d.Nom AS devis_nom,
                               sp.Id AS statut_produit_id ,sp.Nom AS statut_produit_nom,
                               pl.Id AS plan_id ,pl.Nom AS plan_nom,
                               pr.Id AS projet_id, pr.Nom AS projet_nom
                               FROM Produit p
                               LEFT JOIN Devis d ON p.Devis_Id = d.Id
                               LEFT JOIN StatutProduit sp ON p.StatutProduit_Id = sp.Id
                               LEFT JOIN Plan pl ON p.Plan_Id = pl.Id
                               LEFT JOIN Projet pr ON p.Projet_Id = pr.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Produit p = new Produit()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            Devis = new Devis()
                            {
                                Id = Convert.ToInt32(reader["devis_id"]),
                                Nom = Convert.ToString(reader["devis_nom"]),
                            },
                            Plan = new Plan()
                            {
                                Id = Convert.ToInt32(reader["plan_id"]),
                                Nom = Convert.ToString(reader["plan_nom"]),
                            },
                            Projet = new Projet()
                            {
                                Id = Convert.ToInt32(reader["projet_id"]),
                                Nom = Convert.ToString(reader["projet_nom"])
                            },
                            StatutProduit = new StatutProduit()
                            {
                                Id = Convert.ToInt32(reader["statut_produit_id"]),
                                Nom = Convert.ToString(reader["statut_produit_nom"])
                            },
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        listeProduits.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeProduits;
        }

        public int InsertModele(Produit modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Projet == null)
                    throw new Exception("Tentative d'insertion dans la table Produit avec la clé étrangère Projet nulle");
                if (modele.Devis == null)
                    throw new Exception("Tentative d'insertion dans la table Produit avec la clé étrangère Devis nulle");
                if (modele.Plan == null)
                    throw new Exception("Tentative d'insertion dans la table Produit avec la clé étrangère Plan nulle");
                if (modele.StatutProduit == null)
                    throw new Exception("Tentative d'insertion dans la table Produit avec la clé étrangère StatutProduit nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                int projetId;
                if (!Synchronisation<ProjetDAL, Projet>.CorrespondanceModeleId.TryGetValue(modele.Projet.Id, out projetId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    projetId = Synchronisation<ProjetDAL, Projet>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Projet.Id).Key;
                }
                int devisId;
                if (!Synchronisation<DevisDAL, Devis>.CorrespondanceModeleId.TryGetValue(modele.Devis.Id, out devisId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    devisId = Synchronisation<DevisDAL, Devis>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Devis.Id).Key;
                }
                int planId;
                if (!Synchronisation<PlanDAL, Plan>.CorrespondanceModeleId.TryGetValue(modele.Plan.Id, out planId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    planId = Synchronisation<PlanDAL, Plan>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Plan.Id).Key;
                }
                int statutProduitId;
                if (!Synchronisation<StatutProduitDAL, StatutProduit>.CorrespondanceModeleId.TryGetValue(modele.StatutProduit.Id, out statutProduitId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    statutProduitId = Synchronisation<StatutProduitDAL, StatutProduit>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.StatutProduit.Id).Key;
                }

                string sql = @"INSERT INTO Produit (Nom,Projet_Id,Plan_Id,Devis_Id,StatutProduit_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6,@7,@8)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",projetId },
                {"@3",planId },
                {"@4",devisId },
                {"@5",statutProduitId },
                {"@6", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@7", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@8", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
            };

                result = Insert(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
                //TODO
                //Logger.WriteEx(e);

            }

            return result;
        }

        public int UpdateModele(Produit produitLocal, Produit produitDistant)
        {
            //Vérification des clés étrangères
            if (produitDistant.Projet == null)
                throw new Exception("Tentative d'insertion dans la table Produit avec la clé étrangère Projet nulle");
            if (produitDistant.Devis == null)
                throw new Exception("Tentative d'insertion dans la table Produit avec la clé étrangère Devis nulle");
            if (produitDistant.Plan == null)
                throw new Exception("Tentative d'insertion dans la table Produit avec la clé étrangère Plan nulle");
            if (produitDistant.StatutProduit == null)
                throw new Exception("Tentative d'insertion dans la table Produit avec la clé étrangère StatutProduit nulle");


            //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
            int projetId;
            if (!Synchronisation<ProjetDAL, Projet>.CorrespondanceModeleId.TryGetValue(produitDistant.Projet.Id, out projetId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                projetId = Synchronisation<ProjetDAL, Projet>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == produitDistant.Projet.Id).Key;
            }
            int devisId;
            if (!Synchronisation<DevisDAL, Devis>.CorrespondanceModeleId.TryGetValue(produitDistant.Devis.Id, out devisId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                devisId = Synchronisation<DevisDAL, Devis>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == produitDistant.Devis.Id).Key;
            }
            int planId;
            if (!Synchronisation<PlanDAL, Plan>.CorrespondanceModeleId.TryGetValue(produitDistant.Plan.Id, out planId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                planId = Synchronisation<PlanDAL, Plan>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == produitDistant.Plan.Id).Key;
            }
            int statutProduitId;
            if (!Synchronisation<StatutProduitDAL, StatutProduit>.CorrespondanceModeleId.TryGetValue(produitDistant.StatutProduit.Id, out statutProduitId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                statutProduitId = Synchronisation<StatutProduitDAL, StatutProduit>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == produitDistant.StatutProduit.Id).Key;
            }


            produitLocal.Copy(produitDistant);
            string sql = @"
                        UPDATE Produit
                        SET Nom=@1,Projet_Id=@2,Plan_Id=@3,Devis_Id=@3,StatutProduit_Id=@5,MiseAJour=@6
                        WHERE Id=@7
                      ";

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",produitLocal.Nom},
                {"@2",projetId},
                {"@3",planId},
                {"@4",devisId},
                {"@5",statutProduitId},
                {"@6",DateTimeDbAdaptor.FormatDateTime( produitLocal.MiseAJour,Bdd) },
                {"@7",produitLocal.Id },
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

        public int DeleteModele(Produit modele)
        {
            string sql = @"
                        UPDATE Produit
                        SET Suppression= @2
                        WHERE Id=@1
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Id},
                {"@2",DateTimeDbAdaptor.FormatDateTime(modele.Suppression,Bdd)}

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
                //Logger.WriteEx(e);
            }

            return result;
        }
        #endregion

    }
}
