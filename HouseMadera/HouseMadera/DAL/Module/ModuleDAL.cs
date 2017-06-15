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
    public class ModuleDAL : DAL, IDAL<Module>
    {
        public ModuleDAL(string nomBdd) : base(nomBdd)
        {
        }


        #region SYNCHRONISATION
        public int DeleteModele(Module modele)
        {
            {
                string sql = @"
                        UPDATE Module
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
        }

        public List<Module> GetAllModeles()
        {
            List<Module> listeModules = new List<Module>();
            try
            {

                string sql = @"SELECT m.*,g.Id AS gamme_Id , g.Nom AS gamme_Nom , t.Id AS typeModule_Id , t.Nom AS typeModule_Nom
                               FROM Module m
                               LEFT JOIN Gamme g ON m.Gamme_Id = g.Id
                               LEFT JOIN TypeModule t ON m.TypeModule_Id = t.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Module m = new Module()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Gamme = new Gamme()
                            {
                                Id = Convert.ToInt32(reader["gamme_Id"]),
                                Nom = Convert.ToString(reader["gamme_Nom"]),
                            },
                            TypeModule = new TypeModule()
                            {
                                Id = Convert.ToInt32(reader["typeModule_Id"]),
                                Nom = Convert.ToString(reader["typeModule_Nom"]),
                            }
                        };
                        listeModules.Add(m);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEx(ex);
            }

            return listeModules;
        }

        public int InsertModele(Module modele,MouvementSynchronisation sens)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.TypeModule == null)
                    throw new Exception("Tentative d'insertion dans la table Module avec la clé étrangère TypeModule nulle");
                if (modele.Gamme == null)
                    throw new Exception("Tentative d'insertion dans la table Module avec la clé étrangère Gamme nulle");




                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                int typeModuleId = 0;
                int gammeId = 0;

                if(sens == MouvementSynchronisation.Sortant)
                {
                    Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.TryGetValue(modele.TypeModule.Id, out typeModuleId);
                    Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.TryGetValue(modele.Gamme.Id, out gammeId);
                }
                else
                {
                    typeModuleId = Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeModule.Id).Key;
                    gammeId = Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Gamme.Id).Key;
                }
                //if (!Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.TryGetValue(modele.TypeModule.Id, out typeModuleId))
                //{
                //    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                //    typeModuleId = Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeModule.Id).Key;
                //}
                
                //if (!Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.TryGetValue(modele.Gamme.Id, out gammeId))
                //{
                //    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                //    gammeId = Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Gamme.Id).Key;
                //}

                string sql = @"INSERT INTO Module (Nom,Gamme_Id,TypeModule_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",gammeId },
                {"@3",typeModuleId },
                {"@4", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@5", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@6", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
            };

                result = Insert(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                
                Logger.WriteEx(e);

            }

            return result;
        }

        public int UpdateModele(Module moduleLocal, Module moduleDistant, MouvementSynchronisation sens)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (moduleDistant.TypeModule == null)
                    throw new Exception("Tentative d'insertion dans la table Module avec la clé étrangère TypeModule nulle");
                if (moduleDistant.Gamme == null)
                    throw new Exception("Tentative d'insertion dans la table Module avec la clé étrangère Gamme nulle");

                int typeModuleId = 0;
                int gammeId = 0;

                if (sens == MouvementSynchronisation.Sortant)
                {
                    Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.TryGetValue(moduleDistant.TypeModule.Id, out typeModuleId);
                    Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.TryGetValue(moduleDistant.Gamme.Id, out gammeId);
                }
                else
                {
                    typeModuleId = Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == moduleDistant.TypeModule.Id).Key;
                    gammeId = Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == moduleDistant.Gamme.Id).Key;
                }



                ////Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                //int typeModuleId = 0;
                //if (!Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.TryGetValue(moduleDistant.TypeModule.Id, out typeModuleId))
                //{
                //    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                //    typeModuleId = Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == moduleDistant.TypeModule.Id).Key;
                //}
                //int gammeId = 0;
                //if (!Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.TryGetValue(moduleDistant.Gamme.Id, out gammeId))
                //{
                //    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                //    gammeId = Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == moduleDistant.Gamme.Id).Key;
                //}
                moduleLocal.Copy(moduleDistant);
                string sql = @"
                        UPDATE Module
                        SET Nom=@1,Gamme_Id=@2,TypeModule_Id=@3,MiseAJour=@4
                        WHERE Id=@5";

                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",moduleLocal.Nom},
                {"@2",gammeId},
                {"@3",typeModuleId},
                {"@4",DateTimeDbAdaptor.FormatDateTime( moduleLocal.MiseAJour,Bdd) },
                {"@5",moduleLocal.Id },
                };

                result = Update(sql, parameters);
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
