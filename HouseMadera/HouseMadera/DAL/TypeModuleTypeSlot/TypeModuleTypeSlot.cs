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
    public class TypeModuleTypeSlotDAL :DAL, IDAL<TypeModuleTypeSlot>
    {
        public TypeModuleTypeSlotDAL(string nomBdd) : base(nomBdd)
        {

        }
        #region SYNCHRONISATION
        public int DeleteModele(TypeModuleTypeSlot modele)
        {
            {
                string sql = @"
                        UPDATE TypeModuleTypeSlot
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

        public List<TypeModuleTypeSlot> GetAllModeles()
        {
            List<TypeModuleTypeSlot> listeTypeModuleTypeSlot = new List<TypeModuleTypeSlot>();
            try
            {

                string sql = @"SELECT t.* , tm.Id AS typeModule_Id, tm.Nom AS typeSlot_Nom , ts.Id AS typeSlot_Id, ts.Nom AS typeSlot_Nom
                               FROM TypeModuleTypeSlot t
                               LEFT JOIN TypeModule tm ON t.TypeModule_Id
                               LEFT JOIN TypeSlot ts ON t.TypeSlot_Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        TypeModuleTypeSlot t = new TypeModuleTypeSlot()
                        {
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            TypeSlot = new TypeSlot()
                            {
                                Id = Convert.ToInt32(reader["typeSlot_Id"]),
                                Nom = Convert.ToString(reader["typeSlot_Nom"]),
                            },
                            TypeModule = new TypeModule()
                            {
                                Id = Convert.ToInt32(reader["typeModule_Id"]),
                                Nom = Convert.ToString(reader["typeModule_Nom"]),
                            }
                        };
                        listeTypeModuleTypeSlot.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeTypeModuleTypeSlot;
        }

        public int InsertModele(TypeModuleTypeSlot modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.TypeModule == null)
                    throw new Exception("Tentative d'insertion dans la table TypeModuleTypeSlot avec la clé étrangère TypeModule nulle");
                if (modele.TypeSlot == null)
                    throw new Exception("Tentative d'insertion dans la table TypeModuleTypeSlot avec la clé étrangère TypeSlot nulle");

                int typeModuleId = 0;
                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.TryGetValue(modele.TypeModule.Id, out  typeModuleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeModuleId = Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeModule.Id).Key;
                }
                int typeSlotId = 0;
                if (!Synchronisation<TypeSlotDAL,TypeSlot>.CorrespondanceModeleId.TryGetValue(modele.TypeSlot.Id, out  typeSlotId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeSlotId = Synchronisation<TypeSlotDAL, TypeSlot>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeSlot.Id).Key;
                }



                string sql = @"INSERT INTO TypeModuleTypeSlot (TypeModule_Id,TypeSlot_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",typeModuleId},
                {"@2",typeSlotId},
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
                
                //Logger.WriteEx(e);

            }

            return result;
        }

        public int UpdateModele(TypeModuleTypeSlot modele1, TypeModuleTypeSlot modele2)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele2.TypeModule == null)
                    throw new Exception("Tentative d'insertion dans la table TypeModuleTypeSlot avec la clé étrangère TypeModule nulle");
                if (modele2.TypeSlot == null)
                    throw new Exception("Tentative d'insertion dans la table TypeModuleTypeSlot avec la clé étrangère TypeSlot nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance
                int typeModuleId = 0;
                if (!Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.TryGetValue(modele2.TypeModule.Id, out  typeModuleId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeModuleId = Synchronisation<TypeModuleDAL, TypeModule>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele2.TypeModule.Id).Key;
                }
                int typeSlotId = 0;
                if (!Synchronisation<TypeSlotDAL, TypeSlot>.CorrespondanceModeleId.TryGetValue(modele2.TypeSlot.Id, out typeSlotId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeSlotId = Synchronisation<TypeSlotDAL, TypeSlot>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele2.TypeSlot.Id).Key;
                }



                string sql = @"
                        UPDATE TypeModuleTypeSlot
                        SET TypeModule_Id=@1,TypeSlot_Id=@2,MiseAJour=@3
                        WHERE TypeModule_Id=@1 AND TypeSlot_Id=@2";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",typeModuleId},
                {"@2",typeSlotId},
                {"@3", DateTimeDbAdaptor.FormatDateTime( modele2.MiseAJour,Bdd) },
                {"@4", DateTimeDbAdaptor.FormatDateTime( modele2.Suppression,Bdd) },
                {"@5", DateTimeDbAdaptor.FormatDateTime( modele2.Creation,Bdd) }
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
        #endregion
    }
}
