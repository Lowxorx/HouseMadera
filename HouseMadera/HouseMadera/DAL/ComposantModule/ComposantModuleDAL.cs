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
    public class ComposantModuleDAL : DAL, IDAL<ComposantModule>
    {
        public ComposantModuleDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(ComposantModule modele)
        {
            string sql = @"
                        UPDATE ComposantModule
                        SET Suppression= @2
                        WHERE Module_Id = @1  AND Composant_Id = @3
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Module.Id},
                {"@2",DateTimeDbAdaptor.FormatDateTime(modele.Suppression,Bdd)},
                {"@3",modele.Composant.Id}

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

        public List<ComposantModule> GetAllModeles()
        {
            List<ComposantModule> listeComposantModule = new List<ComposantModule>();
            try
            {

                string sql = @"SELECT cm.*,c.Id AS composant_Id , c.Nom AS composant_Nom, m.Id AS module_Id , m.Nom AS module_Nom
                               FROM ComposantModule cm
                               LEFT JOIN Composant c ON cm.Composant_Id = c.Id
                               LEFT JOIN Module m ON cm.Module_Id = m.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        ComposantModule c = new ComposantModule()
                        {
                            Id = 0, //La table de liaison ne contient pas d'ID
                            Nombre = Convert.ToInt32(reader["Nombre"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Composant = new Composant()
                            {
                                Id = Convert.ToInt32(reader["composant_Id"]),
                                Nom = Convert.ToString(reader["composant_Nom"]),
                            },
                            Module = new Module()
                            {
                                Id = Convert.ToInt32(reader["module_Id"]),
                                Nom = Convert.ToString(reader["module_Nom"]),
                            }
                        };
                        listeComposantModule.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeComposantModule;
        }

        public int InsertModele(ComposantModule modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Composant == null)
                    throw new Exception("Tentative d'insertion dans la table ComposantModule avec la clé étrangère Composant nulle");
                if (modele.Module == null)
                    throw new Exception("Tentative d'insertion dans la table ComposantModule avec la clé étrangère Module nulle");

                if (!Synchronisation<ComposantDAL, Composant>.CorrespondanceModeleId.TryGetValue(modele.Composant.Id, out int composantId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    composantId = Synchronisation<ComposantDAL, Composant>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Composant.Id).Key;
                }

                if (!Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.TryGetValue(modele.Module.Id, out int moduleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    moduleId = Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Module.Id).Key;
                }



                string sql = @"INSERT INTO ComposantModule (Nombre,Composant_Id,Module_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nombre },
                {"@2",composantId },
                {"@3",moduleId },
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

        public int UpdateModele(ComposantModule modeleLocal, ComposantModule modeleDistant)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modeleDistant.Composant == null)
                    throw new Exception("Tentative d'insertion dans la table ComposantModule avec la clé étrangère Composant nulle");
                if (modeleDistant.Module == null)
                    throw new Exception("Tentative d'insertion dans la table ComposantModule avec la clé étrangère Module nulle");

                if (!Synchronisation<ComposantDAL, Composant>.CorrespondanceModeleId.TryGetValue(modeleDistant.Composant.Id, out int composantId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    composantId = Synchronisation<ComposantDAL, Composant>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modeleDistant.Composant.Id).Key;
                }

                if (!Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.TryGetValue(modeleDistant.Module.Id, out int moduleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    moduleId = Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modeleDistant.Module.Id).Key;
                }


                //recopie des données du Isolant distant dans le Isolant local
                modeleLocal.Copy(modeleDistant);
                string sql = @"UPDATE ComposantModule 
                               SET Nombre = @1 ,Composant_Id = @2 ,Module_Id = @3 ,MiseAJour = @4 )
                        VALUES(@1,@2,@3,@4)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modeleLocal.Nombre },
                {"@2",composantId },
                {"@3",moduleId },
                {"@4", DateTimeDbAdaptor.FormatDateTime( modeleLocal.MiseAJour,Bdd) },
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

        #endregion
    }
}
