using HouseMadera.DAL.Interfaces;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace HouseMadera.DAL
{
    public class PlanDAL : DAL, IPlanDAL
    {
        public PlanDAL(string nomBdd) : base(nomBdd)
        {

        }


        #region SYNCHRONISATION
        public int DeleteModele(Plan modele)
        {
            string sql = @"
                        UPDATE Plan
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

        public List<Plan> GetAllModeles()
        {
            List<Plan> listePlans = new List<Plan>();
            try
            {

                string sql = @"SELECT p.*,g.Id AS Gamme_Id , g.Nom AS Gamme_Nom, c.Id AS CoupePrincipe_Id ,c.Nom AS CoupePrincipe_Nom
                               FROM Plan p
                               LEFT JOIN Gamme g ON p.Gamme_Id = g.Id
                               LEFT JOIN CoupePrincipe c ON p.CoupePrincipe_Id = c.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Plan p = new Plan()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Gamme = new Gamme()
                            {
                                Id = Convert.ToInt32(reader["Gamme_Id"]),
                                Nom = Convert.ToString(reader["Gamme_Nom"])
                            },
                            CoupePrincipe = new CoupePrincipe()
                            {
                                Id = Convert.ToInt32(reader[" CoupePrincipe_Id"]),
                                Nom = Convert.ToString(reader["CoupePrincipe_Nom"])
                            }
                        };
                        listePlans.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listePlans;
        }

        public int InsertModele(Plan modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Gamme == null)
                    throw new Exception("Tentative d'insertion dans la table Plan avec la clé étrangère Gamme nulle");
                if (modele.CoupePrincipe == null)
                    throw new Exception("Tentative d'insertion dans la table Plan avec la clé étrangère CoupePrincipe nulle");

                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                int gammeId;
                if (!Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.TryGetValue(modele.Gamme.Id, out gammeId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    gammeId = Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Gamme.Id).Key;
                }
                int coupePrincipeId;
                if (!Synchronisation<CoupePrincipeDAL, CoupePrincipe>.CorrespondanceModeleId.TryGetValue(modele.CoupePrincipe.Id, out coupePrincipeId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    coupePrincipeId = Synchronisation<CoupePrincipeDAL, CoupePrincipe>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.CoupePrincipe.Id).Key;
                }


                string sql = @"INSERT INTO Gamme (Nom,Gamme_Id,CoupePrincipe_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",gammeId },
                {"@3",coupePrincipeId },
                {"@4", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@5", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@6", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
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

        public int UpdateModele(Plan planLocal, Plan planDistant)
        {

            //Vérification des clés étrangères
            if (planDistant.Gamme == null)
                throw new Exception("Tentative d'insertion dans la table Plan avec la clé étrangère Gamme nulle");
            if (planDistant.CoupePrincipe == null)
                throw new Exception("Tentative d'insertion dans la table Plan avec la clé étrangère CoupePrincipe nulle");

            //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
            int gammeId;
            if (!Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.TryGetValue(planDistant.Gamme.Id, out gammeId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                gammeId = Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == planDistant.Gamme.Id).Key;
            }
            int coupePrincipeId;
            if (!Synchronisation<CoupePrincipeDAL, CoupePrincipe>.CorrespondanceModeleId.TryGetValue(planDistant.CoupePrincipe.Id, out coupePrincipeId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                coupePrincipeId = Synchronisation<CoupePrincipeDAL, CoupePrincipe>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == planDistant.CoupePrincipe.Id).Key;
            }

            //recopie des données du Plan distant dans le Plan local
            planLocal.Copy(planDistant);
            string sql = @"
                        UPDATE plan
                        SET Nom=@1,Gamme_Id=@2,CoupePrincipe_Id=@3,MiseAJour=@4
                        WHERE Id=@5
                      ";

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",planLocal.Nom},
                {"@2",gammeId},
                {"@3",coupePrincipeId},
                {"@4",DateTimeDbAdaptor.FormatDateTime( planLocal.MiseAJour,Bdd) },
                {"@5",planLocal.Id },
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

