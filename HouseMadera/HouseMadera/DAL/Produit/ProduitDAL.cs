using System;
using System.Collections.Generic;
using HouseMadera.Modeles;
using System.Collections.ObjectModel;
using HouseMadera.Utilites;

namespace HouseMadera.DAL
{
    public class ProduitDAL : DAL
    {

        public ProduitDAL(string nomBdd) : base(nomBdd)
        {

        }

        #region READ

        /// <summary>
        /// Selectionne tous les produits correspondant à un Projet
        /// </summary>
        /// <returns>Une liste d'objets Produit</returns>
        public ObservableCollection<Produit> GetAllProduitsByProjet(Projet p)
        {
            ObservableCollection<Produit> listeProduit = new ObservableCollection<Produit>();
            try
            {
                string sql = @"SELECT p.*, d.Id AS id_devis, d.Nom AS nom_devis, sp.Nom AS statut_produit, d.PrixTTC AS prixttc_devis, d.PrixHT AS prixht_devis, pl.Nom AS nom_plan, pl.CreateDate AS date_plan, pr.Nom AS nom_projet, sd.Nom AS statut_devis 
                               FROM Produit p 
                               LEFT JOIN Devis d ON p.Devis_Id = d.Id
                               LEFT JOIN StatutProduit sp ON p.StatutProduit_Id = sp.Id
                               LEFT JOIN StatutDevis sd ON d.StatutDevis_Id = sd.Id
                               LEFT JOIN Plan pl ON p.Plan_Id = pl.Id 
                               LEFT JOIN Projet pr ON p.Projet_Id = pr.Id 
                               WHERE Projet_Id=@1";

                var parametres = new Dictionary<string, object>()
                {
                    {"@1", p.Id}
                };
                var reader = Get(sql, parametres);
                while (reader.Read())
                {
                    var produit = new Produit();
                    produit.Id = Convert.ToInt32(reader["Id"]);
                    produit.Nom = Convert.ToString(reader["Nom"]);
                    produit.Devis = new Devis()
                    {
                        Nom = Convert.ToString(reader["nom_devis"]),
                        Id = Convert.ToInt32(reader["id_devis"]),
                        PrixTTC = Convert.ToDecimal(reader["prixttc_devis"]),
                        PrixHT = Convert.ToDecimal(reader["prixht_devis"]),
                        StatutDevis = new StatutDevis() { Nom = reader["statut_devis"].ToString() }
                    };
                    produit.Plan = new Plan()
                    {
                        Nom = Convert.ToString(reader["nom_plan"]),
                        CreateDate = Convert.ToDateTime(reader["date_plan"])
                    };
                    produit.Projet = new Projet()
                    {
                        Nom = Convert.ToString(reader["nom_projet"])
                    };
                    produit.StatutProduit = new StatutProduit() { Nom = reader["statut_produit"].ToString() };
                    listeProduit.Add(produit);
                }
                return listeProduit;
            }
            catch (Exception e)
            {
                Logger.WriteEx(e);
                return null;
            }
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
