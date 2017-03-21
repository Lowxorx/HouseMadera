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
    public class StatutProduitDAL : DAL, IDAL<StatutProduit>
    {
        public StatutProduitDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(StatutProduit modele)
        {
            string sql = @"
                        UPDATE StatutProduit
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

        public List<StatutProduit> GetAllModeles()
        {
            List<StatutProduit> statutsProduit = new List<StatutProduit>();
            try
            {

                string sql = @"SELECT * FROM StatutProduit";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        StatutProduit s = new StatutProduit()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        statutsProduit.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return statutsProduit;
        }

        public int InsertModele(StatutProduit modele)
        {
            string sql = @"INSERT INTO StatutProduit (Nom,MiseAJour,Suppression,Creation)
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

        public int UpdateModele(StatutProduit statutProduitLocal, StatutProduit statutProduitDistant)
        {
            //recopie des données du StatutProduit distant dans le StatutProduit local
            statutProduitLocal.Copy(statutProduitDistant);

            string sql = @"
                        UPDATE StatutProduit
                        SET Nom=@1,MiseAJour=@2
                        WHERE Id=@3
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",statutProduitLocal.Nom},
                {"@2", DateTimeDbAdaptor.FormatDateTime( statutProduitLocal.MiseAJour,Bdd)},
                {"@3",statutProduitLocal.Id }
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
