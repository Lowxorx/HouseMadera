using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace HouseMadera.DAL
{
    public class SlotPlaceDAL : DAL, IDAL<SlotPlace>
    {
        public SlotPlaceDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(SlotPlace modele)
        {
            {
                string sql = @"
                        UPDATE SlotPlace
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

        public List<SlotPlace> GetAllModeles()
        {
            List<SlotPlace> listeSlotsPlaces = new List<SlotPlace>();
            try
            {

                string sql = @"SELECT sp.*,m.Id AS module_Id , m.Nom AS module_Nom , s.Id AS slot_Id ,s.Nom AS slot_Nom,tmp.Id AS typeModulePlacable_Id,tmp.Nom AS typeModulePlacable_Nom
                               FROM SlotPlace sp
                               LEFT JOIN Module m ON sp.Module_Id = m.Id
                               LEFT JOIN Slot s ON sp.Slot_Id = s.Id
                               LEFT JOIN TypeModulePlacable tmp ON sp.TypeModulePlacable_Id = tmp.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        SlotPlace sp = new SlotPlace()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Libelle = Convert.ToString(reader["Libelle"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Module = new Module()
                            {
                                Id = Convert.ToInt32(reader["module_Id"]),
                                Nom = Convert.ToString(reader["module_Nom"]),
                            },
                            Slot = new Slot()
                            {
                                Id = Convert.ToInt32(reader["slot_Id"]),
                                Nom = Convert.ToString(reader["slot_Nom"])
                            },
                            TypeModulePlacable = new TypeModulePlacable()
                            {
                                Id = reader["typeModulePlacable_Id"] == null ? 0 : Convert.ToInt32(reader["module_Id"]),
                                Nom = Convert.ToString(reader["typeModulePlacable_Nom"]),
                            }
                        };
                        listeSlotsPlaces.Add(sp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeSlotsPlaces;
        }

        public int InsertModele(SlotPlace modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Module == null)
                    throw new Exception("Tentative d'insertion dans la table SlotPlace avec la clé étrangère Module nulle");
                if (modele.Slot == null)
                    throw new Exception("Tentative d'insertion dans la table SlotPlace avec la clé étrangère Slot nulle");
                if (modele.TypeModulePlacable == null)
                    throw new Exception("Tentative d'insertion dans la table SlotPlace avec la clé étrangère TypeModulePlacable nulle");

                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.TryGetValue(modele.Module.Id, out int moduleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    moduleId = Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Module.Id).Key;
                }
                if (!Synchronisation<SlotDAL, Slot>.CorrespondanceModeleId.TryGetValue(modele.Slot.Id, out int slotId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    slotId = Synchronisation<SlotDAL, Slot>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Slot.Id).Key;
                }
                if (!Synchronisation<TypeModulePlacableDAL, TypeModulePlacable>.CorrespondanceModeleId.TryGetValue(modele.TypeModulePlacable.Id, out int typeModulePlacableId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeModulePlacableId = Synchronisation<TypeModulePlacableDAL, TypeModulePlacable>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeModulePlacable.Id).Key;
                }


                string sql = @"INSERT INTO SlotPlace (Libelle,Module_Id,Slot_Id,TypeModulePlacable_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6,@7)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Libelle },
                {"@2",moduleId },
                {"@3",slotId },
                {"@4",typeModulePlacableId },
                {"@5", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@6", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@7", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
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

        public int UpdateModele(SlotPlace slotPlaceLocal, SlotPlace slotPlaceDistant)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (slotPlaceDistant.Module == null)
                    throw new Exception("Tentative d'insertion dans la table SlotPlace avec la clé étrangère Module nulle");
                if (slotPlaceDistant.Slot == null)
                    throw new Exception("Tentative d'insertion dans la table SlotPlace avec la clé étrangère Slot nulle");
                if (slotPlaceDistant.TypeModulePlacable == null)
                    throw new Exception("Tentative d'insertion dans la table SlotPlace avec la clé étrangère TypeModulePlacable nulle");

                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.TryGetValue(slotPlaceDistant.Module.Id, out int moduleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    moduleId = Synchronisation<ModuleDAL, Module>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == slotPlaceDistant.Module.Id).Key;
                }
                if (!Synchronisation<SlotDAL, Slot>.CorrespondanceModeleId.TryGetValue(slotPlaceDistant.Slot.Id, out int slotId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    slotId = Synchronisation<SlotDAL, Slot>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == slotPlaceDistant.Slot.Id).Key;
                }
                if (!Synchronisation<TypeModulePlacableDAL, TypeModulePlacable>.CorrespondanceModeleId.TryGetValue(slotPlaceDistant.TypeModulePlacable.Id, out int typeModulePlacableId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeModulePlacableId = Synchronisation<TypeModulePlacableDAL, TypeModulePlacable>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == slotPlaceDistant.TypeModulePlacable.Id).Key;
                }

                slotPlaceLocal.Copy(slotPlaceDistant);
                string sql = @"
                        UPDATE SlotPlace
                        SET Libelle=@1,Module_Id=@2,Slot_Id=@3,TypeModulePlacable_Id=@4,MiseAJour=@5
                        WHERE Id=@6";

                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",slotPlaceLocal.Libelle},
                {"@2",moduleId},
                {"@3",slotId},
                {"@4",typeModulePlacableId},
                {"@5",DateTimeDbAdaptor.FormatDateTime( slotPlaceLocal.MiseAJour,Bdd) },
                {"@6",slotPlaceLocal.Id },
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
