using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class TypeModulePlacableDAL : DAL, IDAL<TypeModulePlacable>
    {
        public TypeModulePlacableDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(TypeModulePlacable modele)
        {
            string sql = @"
                        UPDATE TypeModulePlacable
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

        public List<TypeModulePlacable> GetAllModeles()
        {
            List<TypeModulePlacable> listeTypeModulePlacable = new List<TypeModulePlacable>();
            try
            {
                
                string sql = string.Format("SELECT * FROM TypeModulePlacable");

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {

                        TypeModulePlacable t = new TypeModulePlacable()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            Icone = string.IsNullOrEmpty(reader["Icone"].ToString()) ? null :(byte[])reader["Icone"],
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        listeTypeModulePlacable.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeTypeModulePlacable;
        }

        public int InsertModele(TypeModulePlacable modele, MouvementSynchronisation sens)
        {
            int result = 0;
            try
            {
                string sql = @"INSERT INTO TypeModulePlacable (Nom,Icone,MiseAJour,Creation,Suppression)
                        VALUES(@1,@2,@3,@4,@5)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    {"@1",modele.Nom },
                    {"@2",modele.Icone },
                    {"@3", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                    {"@4", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) },
                    {"@5", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                };

                result = Insert(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public int UpdateModele(TypeModulePlacable typeModulePlacableLocal, TypeModulePlacable typeModulePlacableDistant, MouvementSynchronisation sens)
        {
            int result = 0;
            try
            {
                if (typeModulePlacableDistant != null)
                    typeModulePlacableLocal.Copy(typeModulePlacableDistant);

                string sql = @"UPDATE Devis
                               SET Nom=@1,Icone=@2,MiseAJour=@3,DateCreation=@4
                               WHERE Id=@5";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    {"@1",typeModulePlacableLocal.Nom },
                    {"@2",typeModulePlacableLocal.Icone },
                    {"@3", DateTimeDbAdaptor.FormatDateTime( typeModulePlacableLocal.MiseAJour,Bdd) },
                    {"@4", DateTimeDbAdaptor.FormatDateTime( typeModulePlacableLocal.Creation,Bdd) },
                    {"@5", typeModulePlacableLocal.Id }
            };

                result = Insert(sql, parameters);
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
