using HouseMadera.DAL.Interfaces;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class TypeIsolantDAL : DAL, ITypeIsolantDAL
    {
        public TypeIsolantDAL(string nomBdd) : base(nomBdd)
        {
        }


        #region SYNCHRONISATION
        public int DeleteModele(TypeIsolant modele)
        {
            string sql = @"
                        UPDATE TypeIsolant
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

        public List<TypeIsolant> GetAllModeles()
        {
            List<TypeIsolant> listeTypeIsolant = new List<TypeIsolant>();
            try
            {

                string sql = @"SELECT * FROM TypeIsolant";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        TypeIsolant t = new TypeIsolant()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        listeTypeIsolant.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeTypeIsolant;
        }

        public int InsertModele(TypeIsolant modele)
        {
            string sql = @"INSERT INTO TypeIsolant (Nom,MiseAJour,Suppression,Creation)
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

        public int UpdateModele(TypeIsolant typeIsolantLocal, TypeIsolant typeIsolantDistant)
        {
            // recopie des données du commercial distant dans le commercial local
            typeIsolantLocal.Copy<TypeIsolant>(typeIsolantDistant);

            string sql = @"
                        UPDATE TypeIsolant
                        SET Nom=@1,MiseAJour=@2
                        WHERE Id=@3
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",typeIsolantLocal.Nom},
                {"@2", DateTimeDbAdaptor.FormatDateTime( typeIsolantLocal.MiseAJour,Bdd)},
                {"@3",typeIsolantLocal.Id }
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
