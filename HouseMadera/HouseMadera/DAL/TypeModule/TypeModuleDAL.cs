using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace HouseMadera.DAL
{
    class TypeModuleDAL : DAL, IDAL<TypeModule>
    {
        public TypeModuleDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONIZATION
        public int DeleteModele(TypeModule modele)
        {
            string sql = @"
                        UPDATE TypeModule
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

        public List<TypeModule> GetAllModeles()
        {
            List<TypeModule> listeTypeModule = new List<TypeModule>();
            try
            {

                string sql = @"SELECT * FROM TypeModule";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        TypeModule t = new TypeModule()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        listeTypeModule.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeTypeModule;
        }

        public int InsertModele(TypeModule modele)
        {
            string sql = @"INSERT INTO TypeModule (Nom,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@3", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@4", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
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
                //Logger.WriteEx(e);
            }

            return result;
        }

        public int UpdateModele(TypeModule typeModuleLocal, TypeModule typeModuleDistant)
        {
            // recopie des données du TypeModule distant dans le TypeModule local
            typeModuleLocal.Copy(typeModuleDistant);

            string sql = @"
                        UPDATE TypeModule
                        SET Nom=@1,MiseAJour=@2
                        WHERE Id=@3
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",typeModuleLocal.Nom},
                {"@2", DateTimeDbAdaptor.FormatDateTime( typeModuleLocal.MiseAJour,Bdd)},
                {"@3",typeModuleLocal.Id }
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
