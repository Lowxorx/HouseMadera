using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Collections.Generic;
using HouseMadera.Utilites;
using HouseMadera.Modeles;

namespace HouseMadera.DAL
{
    public class CommercialDAL : DAL
    {
        public CommercialDAL(string nomBdd) : base(nomBdd)
        {

        }

        #region READ
        /// <summary>
        /// Selectionne tous les Commerciaux enregistrés en base
        /// </summary>
        /// <returns>Une collection d'objets Commercial</returns>
        public static ObservableCollection<Commercial> ChargerCommerciaux()
        {
            ObservableCollection<Commercial> listeCommerciaux = new ObservableCollection<Commercial>();
            try
            {
                Console.WriteLine("Connexion BDD");
                string sql = @"SELECT * FROM Commercial";
                var reader = Get(sql, null);
                while (reader.Read())
                {
                    Commercial c = new Commercial() { Nom = reader.GetString(reader.GetOrdinal("Nom")), Prenom = reader.GetString(reader.GetOrdinal("Prenom")) };
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

                var reader = Get(sql, parameters);
                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1}", reader.GetInt32(0),reader.GetString(1));
                }
                reader.Close();
                return "0";
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

        #endregion
    }
}
