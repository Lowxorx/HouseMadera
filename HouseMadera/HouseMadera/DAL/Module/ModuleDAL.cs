﻿using HouseMadera.Modeles;
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


        #region synchronisation
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
                            Hauteur = Convert.ToDecimal(reader["Hauteur"]),
                            Largeur = Convert.ToDecimal(reader["Largeur"]),
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
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeModules;
        }

        public int InsertModele(Module modele)
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
                int typeModuleId;
                if (!Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.TryGetValue(modele.TypeModule.Id, out typeModuleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeModuleId = Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeModule.Id).Key;
                }
                int gammeId;
                if (!Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.TryGetValue(modele.Gamme.Id, out gammeId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeModuleId = Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeModule.Id).Key;
                }

                string sql = @"INSERT INTO Module (Nom,Hauteur,Largeur,Gamme_Id,TypeModule_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6,@7,@8)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",modele.Hauteur },
                {"@3",modele.Largeur },
                {"@4",gammeId },
                {"@5",typeModuleId },
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

        public int UpdateModele(Module moduleLocal, Module moduleDistant)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (moduleDistant.TypeModule == null)
                    throw new Exception("Tentative d'insertion dans la table Module avec la clé étrangère TypeModule nulle");
                if (moduleDistant.Gamme == null)
                    throw new Exception("Tentative d'insertion dans la table Module avec la clé étrangère Gamme nulle");

                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                int typeModuleId;
                if (!Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.TryGetValue(moduleDistant.TypeModule.Id, out typeModuleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeModuleId = Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == moduleDistant.TypeModule.Id).Key;
                }
                int gammeId;
                if (!Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.TryGetValue(moduleDistant.Gamme.Id, out gammeId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeModuleId = Synchronisation<GammeDAL, Gamme>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == moduleDistant.TypeModule.Id).Key;
                }
                moduleLocal.Copy(moduleDistant);
                string sql = @"
                        UPDATE Module
                        SET Nom=@1,Hauteur = @2,Largeur=@3,Gamme_Id=@4,TypeModule_Id=@5,MiseAJour=@6
                        WHERE Id=@7";

                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",moduleLocal.Nom},
                {"@2",moduleLocal.Hauteur},
                {"@3",moduleLocal.Largeur},
                {"@4",gammeId},
                {"@5",typeModuleId},
                {"@6",DateTimeDbAdaptor.FormatDateTime( moduleLocal.MiseAJour,Bdd) },
                {"@7",moduleLocal.Id },
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
