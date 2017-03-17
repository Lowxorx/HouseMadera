using HouseMadera.DAL.Interfaces;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class QualiteDAL : DAL, IQualiteDAL
    {
        public QualiteDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region
        public int DeleteModele(Qualite qualite)
        {
            string sql = @"
                        UPDATE Qualite
                        SET Suppression= @2
                        WHERE Id=@1
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",qualite.Id},
                {"@2",DateTimeDbAdaptor.FormatDateTime(qualite.Suppression,Bdd)}

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

        public List<Qualite> GetAllModeles()
        {
            List<Qualite> listeQualites = new List<Qualite>();
            try
            {

                string sql = @"SELECT * FROM Qualite";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Qualite q = new Qualite()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        listeQualites.Add(q);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeQualites;
        }

        public int InsertModele(Qualite qualite)
        {
            string sql = @"INSERT INTO Qualite (Nom,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",qualite.Nom },
                {"@2", DateTimeDbAdaptor.FormatDateTime( qualite.MiseAJour,Bdd) },
                {"@3", DateTimeDbAdaptor.FormatDateTime( qualite.Suppression,Bdd) },
                {"@4", DateTimeDbAdaptor.FormatDateTime( qualite.Creation,Bdd) }
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

        public int UpdateModele(Qualite qualiteLocal, Qualite qualiteDistant)
        {
            //recopie des données du commercial distant dans le commercial local
            qualiteLocal.Copy<Qualite>(qualiteDistant);

            string sql = @"
                        UPDATE Qualite
                        SET Nom=@1,MiseAJour=@2
                        WHERE Id=@3
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",qualiteLocal.Nom},
                {"@2", DateTimeDbAdaptor.FormatDateTime( qualiteLocal.MiseAJour,Bdd)},
                {"@3",qualiteLocal.Id }
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
