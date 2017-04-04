using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class StatutDevisDAL : DAL,IDAL<StatutDevis>
    {
        /// <summary>
        /// Constructeur initial
        /// </summary>
        /// <param name="nomBdd"></param>
        public StatutDevisDAL(string nomBdd) : base(nomBdd)
        {
            // Constructeur par défaut de la classe DevisDAL
        }
        
        /// <summary>
        /// Selectionne le Statut d'un Devis en focntion de l'id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet StatutDevis</returns>
        public StatutDevis GetStatutDevis(int id)
        {

            string sql = @"SELECT * FROM StatutDevis WHERE Id = @1";
            var parametres = new Dictionary<string, object>()
            {
                {"@1", id}
            };
            var reader = Get(sql, parametres);
            var sDevis = new StatutDevis();
            while (reader.Read())
            {
                sDevis.Id = Convert.ToInt32(reader["Id"]);
                sDevis.Nom = Convert.ToString(reader["Nom"]);
            }
            return sDevis;
        }

        #region SYNCHRONISATION
        public int DeleteModele(StatutDevis modele)
        {
            string sql = @"
                        UPDATE StatutDevis
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

        public List<StatutDevis> GetAllModeles()
        {
            List<StatutDevis> statutsDevis = new List<StatutDevis>();
            try
            {

                string sql = @"SELECT * FROM StatutProduit";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        StatutDevis s = new StatutDevis()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        statutsDevis.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return statutsDevis;
        }

        public int InsertModele(StatutDevis modele)
        {
            string sql = @"INSERT INTO StatutDevis (Nom,MiseAJour,Suppression,Creation)
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

        public int UpdateModele(StatutDevis statutDevisLocal, StatutDevis statutDevisDistant)
        {
            //recopie des données du StatutDevis distant dans le StatutDevis local
            statutDevisLocal.Copy(statutDevisDistant);

            string sql = @"
                        UPDATE StatutDevis
                        SET Nom=@1,MiseAJour=@2
                        WHERE Id=@3
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",statutDevisLocal.Nom},
                {"@2", DateTimeDbAdaptor.FormatDateTime( statutDevisLocal.MiseAJour,Bdd)},
                {"@3",statutDevisLocal.Id }
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
