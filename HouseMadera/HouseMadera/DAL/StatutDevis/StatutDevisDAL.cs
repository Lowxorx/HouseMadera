using HouseMadera.Modeles;
using System;
using System.Collections.Generic;


namespace HouseMadera.DAL
{
    public class StatutDevisDAL : DAL
    {

        public StatutDevisDAL(string nomBdd) : base(nomBdd)
        {
            // Constructeur par défaut de la classe DevisDAL
        }

        #region READ

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

        #endregion

        #region CREATE

        #endregion

        #region UPDATE

        #endregion

        #region DELETE

        #endregion

    }
}
