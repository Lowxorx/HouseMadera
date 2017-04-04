using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.DAL
{
    public class ModulePlacePlanDAL : DAL, IDAL<ModulePlacePlan>
    {
        public ModulePlacePlanDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(ModulePlacePlan modele)
        {
            string sql = @"
                        UPDATE ModulePlacePlan
                        SET Suppression = @1
                        WHERE ModulePlace_Id = @2 AND Plan_Id = @3";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",DateTimeDbAdaptor.FormatDateTime(modele.Suppression,Bdd)},
                { "@2",modele.ModulePlace.Id},
                {"@3",modele.Plan.Id }
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

        public List<ModulePlacePlan> GetAllModeles()
        {
            List<ModulePlacePlan> listeModulePlacePlan = new List<ModulePlacePlan>();
            try
            {

                string sql = @"SELECT mpp.*,m.Id AS modulePlace_Id , m.Libelle AS modulePlace_libelle,p.Id AS plan_Id,p.Nom AS plan_Nom
                               FROM ModulePlacePlan mpp
                               LEFT JOIN ModulePlace m ON mpp.ModulePlace_Id = m.Id
                               LEFT JOIN Plan p ON mpp.Plan_Id = p.Id";
                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        ModulePlacePlan m = new ModulePlacePlan()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            ModulePlace = new ModulePlace()
                            {
                                Id = Convert.ToInt32(reader["modulePlace_Id"]),
                                Libelle = Convert.ToString(reader["modulePlace_libelle"])
                            },
                            Plan = new Plan()
                            {
                                Id = Convert.ToInt32(reader["plan_Id"]),
                                Nom = Convert.ToString(reader["plan_Nom"])
                            },
                        };
                        listeModulePlacePlan.Add(m);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeModulePlacePlan;
        }

        public int InsertModele(ModulePlacePlan modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.ModulePlace == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlacePlan avec la clé étrangère ModulePlace nulle");
                if (modele.Plan == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlacePlan avec la clé étrangère Plan nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<ModulePlaceDAL, ModulePlace>.CorrespondanceModeleId.TryGetValue(modele.ModulePlace.Id, out int modulePlaceId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    modulePlaceId = Synchronisation<ModulePlaceDAL, ModulePlace>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.ModulePlace.Id).Key;
                }
                if (!Synchronisation<PlanDAL, Plan>.CorrespondanceModeleId.TryGetValue(modele.Plan.Id, out int planId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    planId = Synchronisation<PlanDAL, Plan>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Plan.Id).Key;
                }



                string sql = @"INSERT INTO ModulePlacePlan (ModulePlace_Id,Plan_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modulePlaceId},
                {"@2",planId},
                {"@3", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@4", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@5", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
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

        public int UpdateModele(ModulePlacePlan modele1, ModulePlacePlan modele2)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele2.ModulePlace == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlacePlan avec la clé étrangère ModulePlace nulle");
                if (modele2.Plan == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlacePlan avec la clé étrangère Plan nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<ModulePlaceDAL, ModulePlace>.CorrespondanceModeleId.TryGetValue(modele2.ModulePlace.Id, out int modulePlaceId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    modulePlaceId = Synchronisation<ModulePlaceDAL, ModulePlace>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele2.ModulePlace.Id).Key;
                }
                if (!Synchronisation<PlanDAL, Plan>.CorrespondanceModeleId.TryGetValue(modele2.Plan.Id, out int planId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    planId = Synchronisation<PlanDAL, Plan>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele2.Plan.Id).Key;
                }

                modele1.Copy(modele2);

                string sql = @"
                        UPDATE ModulePlacePlan
                        SET ModulePlace_Id=@1,Plan_Id=@2,MiseAJour=@3
                        WHERE ModulePlace_Id=@1 AND Plan_Id=@2";

                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modulePlaceId},
                {"@2",planId},
                {"@3",DateTimeDbAdaptor.FormatDateTime( modele1.MiseAJour,Bdd) },
                };

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
