using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;


namespace HouseMadera.DAL
{
    public class TypeSlotDAL : DAL, IDAL<TypeSlot>
    {
        public TypeSlotDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(TypeSlot modele)
        {
            string sql = @"
                        UPDATE TypeSlot
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

        public List<TypeSlot> GetAllModeles()
        {
            List<TypeSlot> listeTypeSlot = new List<TypeSlot>();
            try
            {

                string sql = @"SELECT * FROM TypeSlot";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        TypeSlot t = new TypeSlot()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        listeTypeSlot.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeTypeSlot;
        }

        public int InsertModele(TypeSlot modele, MouvementSynchronisation sens)
        {
            string sql = @"INSERT INTO TypeSlot (Nom,MiseAJour,Suppression,Creation)
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

        public int UpdateModele(TypeSlot typeSlotLocal, TypeSlot typeSlotDistant, MouvementSynchronisation sens)
        {
            // recopie des données du TypeSlot distant dans le TypeSlot local
            typeSlotLocal.Copy(typeSlotDistant);

            string sql = @"
                        UPDATE TypeSlot
                        SET Nom=@1,MiseAJour=@2
                        WHERE Id=@3
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",typeSlotLocal.Nom},
                {"@2", DateTimeDbAdaptor.FormatDateTime( typeSlotLocal.MiseAJour,Bdd)},
                {"@3",typeSlotLocal.Id }
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
