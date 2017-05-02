using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Collections.Generic;
using HouseMadera.Utilities;
using HouseMadera.Modeles;
using System.Data.Common;

namespace HouseMadera.DAL
{
    public class CommercialDAL : DAL, IDAL<Commercial>
    {
        public CommercialDAL(string nomBdd) : base(nomBdd)
        {
            // Constructeur par défaut de la classe CommercialDAL
        }

        #region READ

        /// <summary>
        /// Selectionne tous les Commerciaux enregistrés en base
        /// </summary>
        /// <returns>Une collection d'objets Commercial</returns>
        public List<Commercial> ChargerCommerciaux()
        {
            List<Commercial> listeCommerciaux = new List<Commercial>();
            try
            {
                Console.WriteLine("Connexion BDD");
                string sql = @"SELECT * FROM Commercial";
                var reader = Get(sql, null);
                while (reader.Read())
                {
                    Commercial c = new Commercial()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Login = Convert.ToString(reader["Login"]),
                        Nom = Convert.ToString(reader["Nom"]),
                        Prenom = Convert.ToString(reader["Prenom"])
                    };
                    listeCommerciaux.Add(c);
                }
                return listeCommerciaux;
            }
            catch (Exception ex)
            {
                if (ex is MySqlException || ex is SQLiteException)
                {
                    Logger.WriteEx(ex);
                    Logger.WriteTrace("Timeout connexion BDD");
                    return null;
                }
                Logger.WriteEx(ex);
                return null;
            }
        }

        /// <summary>
        /// Vérifie que le mot de passe et le login du commercial sont corrects
        /// </summary>
        /// <param name="Commercial"></param>
        /// <returns>Un chiffre contenant le résultat : 0 pour un succès, 1 pour un utilisateur incorrect, 2 pour un échec de connexion à la bdd. </returns>
        public string Connect(Commercial commercial)
        {
            try
            {
                Console.WriteLine(@"tentative de connexion de " + commercial.Login);
                string result = string.Empty;
                string sql = @"SELECT * FROM Commercial WHERE Login=@1 AND Password=@2";
                var parameters = new Dictionary<string, object>
                {
                    {"@1",commercial.Login },
                    {"@2",commercial.Password }
                };
                string connexionStatut = "1";

                var reader = Get(sql, parameters);
                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                    connexionStatut = "0";
                }
                reader.Close();
                return connexionStatut;
            }
            catch (Exception ex)
            {
                if (ex is MySqlException || ex is SQLiteException)
                {
                    Logger.WriteEx(ex);
                    Logger.WriteTrace("Timeout connexion BDD");
                    return "2";
                }
                Logger.WriteEx(ex);
                return "2";
            }
        }

        /// <summary>
        /// Selectionne le premier comemrcial avec l'ID en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet Commercial</returns>
        public static Commercial GetCommercial(int id)
        {

            string sql = @"
                            SELECT * FROM Commercial
                            WHERE Id = @1";
            var parametres = new Dictionary<string, object>()
            {
                {"@1", id}
            };
            var reader = Get(sql, parametres);
            var commercial = new Commercial();
            while (reader.Read())
            {
                commercial.Id = Convert.ToInt32(reader["Id"]);
                commercial.Nom = Convert.ToString(reader["Nom"]);
                commercial.Prenom = Convert.ToString(reader["Prenom"]);
                commercial.Login = Convert.ToString(reader["Login"]);
                commercial.Password = Convert.ToString(reader["Password"]);
            }
            return commercial;

        }

        /// <summary>
        /// Selectionne le premier comemrcial avec le Login en paramètre
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Un objet Commercial</returns>
        public Commercial GetCommercial(string login)
        {

            string sql = @"
                            SELECT * FROM Commercial
                            WHERE Login = @1";
            var parametres = new Dictionary<string, object>()
            {
                {"@1", login}
            };
            var reader = Get(sql, parametres);
            var commercial = new Commercial();
            while (reader.Read())
            {
                commercial.Id = Convert.ToInt32(reader["Id"]);
                commercial.Nom = Convert.ToString(reader["Nom"]);
                commercial.Prenom = Convert.ToString(reader["Prenom"]);
                commercial.Login = Convert.ToString(reader["Login"]);
                commercial.Password = Convert.ToString(reader["Password"]);
            }
            return commercial;
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Efface en base le commercial avec l'Id en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Le nombre de ligne affecté en base. -1 si aucune ligne affectée</returns>
        public int DeleteCommercial(int id)
        {
            var sql = @"
                        DELETE FROM Commercial 
                        WHERE Id=@1
                      ";
            var parameters = new Dictionary<string, object>() {
                {"@1",id },
            };
            var result = 0;
            try
            {
                result = Delete(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Logger.WriteEx(e);
            }

            return result;
        }



        #endregion

        #region SYNCHRONISATION
        /// <summary>
        /// Methode implémentée de l'interface ICommercialDAL permettant de récupérer tous les commerciaux de la base
        /// </summary>
        /// <returns>Une liste d'objet Commercial</returns>
        public List<Commercial> GetAllModeles()
        {
            List<Commercial> listeCommerciaux = new List<Commercial>();
            try
            {

                string sql = @"SELECT * FROM Commercial";


                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Commercial c = new Commercial()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Login = Convert.ToString(reader["Login"]),
                            Password = Convert.ToString(reader["Password"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            Prenom = Convert.ToString(reader["Prenom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        listeCommerciaux.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeCommerciaux;
        }
        /// <summary>
        /// Methode implémentée de l'interface ICommercialDAL permettant l'insertion d'un Commercial en base
        /// </summary>
        /// <param name="comercial">Le modèle à insérer</param>
        /// <returns>Le nombre de lignes affectées</returns>
        public int InsertModele(Commercial commercial,MouvementSynchronisation sens)
        {
            string sql = @"INSERT INTO Commercial (Nom,Prenom,Login,Password,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6,@7)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",commercial.Nom },
                {"@2",commercial.Prenom },
                {"@3",commercial.Login },
                {"@4",commercial.Password },
                {"@5", DateTimeDbAdaptor.FormatDateTime( commercial.MiseAJour,Bdd) },
                {"@6", DateTimeDbAdaptor.FormatDateTime( commercial.Suppression,Bdd) },
                {"@7", DateTimeDbAdaptor.FormatDateTime( commercial.Creation,Bdd) }
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
        /// <summary>
        /// Methode implémentée de l'interface ICommercialDAL. Elle effectue une copie des valeurs du deuxième paramètre dans le premier
        /// et met à jour le commercial en base.
        /// </summary>
        /// <param name="commercialLocal">Représente l'objet issue de la base locale </param>
        /// <param name="commercialDistant">Représente l'objet issue de la base distante</param>
        /// <returns>Le nombre de lignes affectées</returns>
        public int UpdateModele(Commercial commercialLocal, Commercial commercialDistant, MouvementSynchronisation sens)
        {
            //recopie des données du Commercial distant dans le Commercial local
            commercialLocal.Copy(commercialDistant);

            string sql = @"
                        UPDATE Commercial
                        SET Nom=@1,Prenom=@2,Login=@3,Password=@4,MiseAJour=@5
                        WHERE Id=@6
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",commercialLocal.Nom},
                {"@2",commercialLocal.Prenom},
                {"@3",commercialLocal.Login},
                {"@4",commercialLocal.Password},
                {"@5", DateTimeDbAdaptor.FormatDateTime( commercialLocal.MiseAJour,Bdd)},
                {"@6",commercialLocal.Id }
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
        /// <summary>
        /// Met à jour en base la date de suppression du commercial (suppression logique)
        /// </summary>
        /// <param name="commercial">Représente le commercial à effacer</param>
        /// <returns>Le nombre de lignes affectées</returns>
        public int DeleteModele(Commercial commercial)
        {
            string sql = @"
                        UPDATE Commercial
                        SET Suppression= @2
                        WHERE Id=@1
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",commercial.Id},
                {"@2",DateTimeDbAdaptor.FormatDateTime(commercial.Suppression,Bdd)}

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
