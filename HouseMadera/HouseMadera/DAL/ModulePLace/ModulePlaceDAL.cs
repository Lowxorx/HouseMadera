using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;


namespace HouseMadera.DAL
{
    public class ModulePlaceDAL : DAL, IDAL<ModulePlace>
    {
        public ModulePlaceDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(ModulePlace modele)
        {
            {
                string sql = @"
                        UPDATE ModulePlace
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

        public List<ModulePlace> GetAllModeles()
        {
            List<ModulePlace> listeModulesPlaces = new List<ModulePlace>();
            try
            {

                string sql = @"SELECT mp.*,m.Id AS module_Id , m.Nom AS module_Nom , s.Id AS slotPlace_Id ,s.Libelle AS slotPlace_Libelle,p.Id AS produit_Id,p.Nom AS produit_Nom
                               FROM ModulePlace mp
                               LEFT JOIN Module m ON mp.Module_Id = m.Id
                               LEFT JOIN SlotPlace s ON mp.SlotPlace_Id = s.Id
                               LEFT JOIN Produit p ON mp.Produit_Id = p.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        ModulePlace m = new ModulePlace()
                        {
                            Id = Convert.ToInt32(reader["Id"]),

                            Horizontal = string.IsNullOrEmpty(reader["Horizontal"].ToString()) ? false : Convert.ToBoolean(reader["Horizontal"]),
                            Vertical = string.IsNullOrEmpty(reader["Vertical"].ToString()) ? false : Convert.ToBoolean(reader["Vertical"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Module = new Module()
                            {
                                Id = Convert.ToInt32(reader["module_Id"]),
                                Nom = Convert.ToString(reader["module_Nom"]),
                            },
                            SlotPlace = new SlotPlace()
                            {
                                Id = Convert.ToInt32(reader["slotPlace_Id"]),
                                Libelle = Convert.ToString(reader["slotPlace_Libelle"])
                            },
                            Produit = new Produit()
                            {
                                Id = Convert.ToInt32(reader["produit_Id"]),
                                Nom = Convert.ToString(reader["produit_Nom"]),
                            }
                        };
                        listeModulesPlaces.Add(m);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeModulesPlaces;
        }

        public int InsertModele(ModulePlace modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Module == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlace avec la clé étrangère Module nulle");
                if (modele.SlotPlace == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlace avec la clé étrangère SlotPlace nulle");
                if (modele.Produit == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlace avec la clé étrangère Produit nulle");

                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.TryGetValue(modele.Module.Id, out int moduleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    moduleId = Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Module.Id).Key;
                }
                if (!Synchronisation<SlotPlaceDAL, SlotPlace>.CorrespondanceModeleId.TryGetValue(modele.SlotPlace.Id, out int slotPlaceId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    slotPlaceId = Synchronisation<SlotPlaceDAL, SlotPlace>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.SlotPlace.Id).Key;
                }
                if (!Synchronisation<ProduitDAL, Produit>.CorrespondanceModeleId.TryGetValue(modele.Produit.Id, out int produitId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    produitId = Synchronisation<ProduitDAL, Produit>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Produit.Id).Key;
                }


                string sql = @"INSERT INTO ModulePlace (Libelle,Horizontal,Vertical,Module_Id,SlotPlace_Id,Produit_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Libelle },
                {"@2",modele.Horizontal },
                {"@3",modele.Vertical },
                {"@4",moduleId },
                {"@5",slotPlaceId },
                {"@6",produitId },
                {"@7", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@8", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@9", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
            };

                result = Insert(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
                
                //Logger.WriteEx(e);

            }

            return result;
        }

        public int UpdateModele(ModulePlace modulePlaceLocal, ModulePlace modulePlaceDistant)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modulePlaceDistant.Module == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlace avec la clé étrangère Module nulle");
                if (modulePlaceDistant.SlotPlace == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlace avec la clé étrangère SlotPlace nulle");
                if (modulePlaceDistant.Produit == null)
                    throw new Exception("Tentative d'insertion dans la table ModulePlace avec la clé étrangère Produit nulle");

                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.TryGetValue(modulePlaceDistant.Module.Id, out int moduleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    moduleId = Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modulePlaceDistant.Module.Id).Key;
                }
                if (!Synchronisation<SlotPlaceDAL, SlotPlace>.CorrespondanceModeleId.TryGetValue(modulePlaceDistant.SlotPlace.Id, out int slotPlaceId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    slotPlaceId = Synchronisation<SlotPlaceDAL, SlotPlace>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modulePlaceDistant.SlotPlace.Id).Key;
                }
                if (!Synchronisation<ProduitDAL, Produit>.CorrespondanceModeleId.TryGetValue(modulePlaceDistant.Produit.Id, out int produitId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    produitId = Synchronisation<ProduitDAL, Produit>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modulePlaceDistant.Produit.Id).Key;
                }

                modulePlaceLocal.Copy(modulePlaceDistant);
                string sql = @"
                        UPDATE ModulePlace
                        SET Libelle=@1,Horizontal = @2,Vertical=@3,Module_Id=@4,SlotPlace_Id=@5,Produit_Id=@6,MiseAJour=@7
                        WHERE Id=@8";

                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modulePlaceLocal.Libelle},
                {"@2",modulePlaceLocal.Horizontal},
                {"@3",modulePlaceLocal.Vertical},
                {"@4",moduleId},
                {"@5",slotPlaceId},
                {"@6",produitId},
                {"@7",DateTimeDbAdaptor.FormatDateTime( modulePlaceLocal.MiseAJour,Bdd) },
                {"@8",modulePlaceLocal.Id },
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
