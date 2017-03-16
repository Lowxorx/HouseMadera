using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class CoupePrincipeDAL : DAL, ICoupePrincipeDAL
    {
        public CoupePrincipeDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region
        public int DeleteModele(CoupePrincipe modele)
        {
            string sql = @"
                        UPDATE CoupePrincipe
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
            }

            return result;
        }

        public List<CoupePrincipe> GetAllModeles()
        {
            string sql = @"
                            SELECT * FROM CoupePrincipe
                            ORDER BY Nom DESC";
            List<CoupePrincipe> coupes = new List<CoupePrincipe>();
            using (DbDataReader reader = Get(sql, null))
            {
                while (reader.Read())
                {
                    CoupePrincipe coupe = new CoupePrincipe();
                    coupe.Id = Convert.ToInt32(reader["Id"]);
                    coupe.Nom = Convert.ToString(reader["Nom"]);
                    coupe.MiseAJour = DateTimeDbAdaptor.InitialiserDate(reader["MiseAJour"].ToString());
                    coupe.Suppression = DateTimeDbAdaptor.InitialiserDate(reader["Suppression"].ToString());
                    coupe.Creation = DateTimeDbAdaptor.InitialiserDate(reader["Creation"].ToString());
                    if (coupe != null)
                        coupes.Add(coupe);
                }
            }

            return coupes;
        }

        public int InsertModele(CoupePrincipe coupe)
        {
            string sql = @"INSERT INTO CoupePrincipe (Nom,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",coupe.Nom },
                {"@2", DateTimeDbAdaptor.FormatDateTime( coupe.MiseAJour,Bdd) },
                {"@3", DateTimeDbAdaptor.FormatDateTime( coupe.Suppression,Bdd) },
                {"@4", DateTimeDbAdaptor.FormatDateTime( coupe.Creation,Bdd) }
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
            }

            return result;
        }

        public int UpdateModele(CoupePrincipe coupeLocale, CoupePrincipe coupeDistante)
        {
            //recopie des données du client distant dans le client local
            if (coupeDistante != null)
                coupeLocale.Copy(coupeDistante);

            string sql = @"
                        UPDATE CoupePrincipe
                        SET Nom=@1,MiseAJour=@2
                        WHERE Id=@3
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",coupeLocale.Nom},
                {"@2", DateTimeDbAdaptor.FormatDateTime( coupeLocale.MiseAJour,Bdd)},
                {"@3",coupeLocale.Id }
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
            }

            return result;
        }
        #endregion
    }
}
