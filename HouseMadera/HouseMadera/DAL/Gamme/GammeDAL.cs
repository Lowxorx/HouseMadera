using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;


namespace HouseMadera.DAL
{
    public class GammeDAL : DAL, IDAL<Gamme>
    {
        public GammeDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(Gamme modele)
        {
            string sql = @"
                        UPDATE Gamme
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

        public List<Gamme> GetAllModeles()
        {
            List<Gamme> listeGammes = new List<Gamme>();
            try
            {

                string sql = @"SELECT g.*,f.Id AS Finition_Id , f.Nom AS Finition_Nom, i.Id AS Isolant_Id ,i.Nom AS Isolant_Nom
                               FROM Gamme g
                               LEFT JOIN Finition f ON g.Finition_Id = f.Id
                               LEFT JOIN Isolant  i ON g.Isolant_Id = i.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Gamme g = new Gamme()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Finition = new Finition()
                            {
                                Id = Convert.ToInt32(reader["Finition_Id"]),
                                Nom = Convert.ToString(reader["Finition_Nom"])
                            },
                            Isolant = new Isolant()
                            {
                                Id = Convert.ToInt32(reader["Isolant_Id"]),
                                Nom = Convert.ToString(reader["Isolant_Nom"])
                            }
                        };
                        listeGammes.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeGammes;
        }

        public int InsertModele(Gamme modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Finition == null)
                    throw new Exception("Tentative d'insertion dans la table Gamme avec la clé étrangère Finition nulle");
                if (modele.Isolant == null)
                    throw new Exception("Tentative d'insertion dans la table Gamme avec la clé étrangère Isolant nulle");

                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<FinitionDAL, Finition>.CorrespondanceModeleId.TryGetValue(modele.Finition.Id, out int finitionId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    finitionId = Synchronisation<FinitionDAL, Finition>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Finition.Id).Key;
                }
                if (!Synchronisation<IsolantDAL, Isolant>.CorrespondanceModeleId.TryGetValue(modele.Isolant.Id, out int isolantId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    isolantId = Synchronisation<IsolantDAL, Isolant>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Isolant.Id).Key;
                }


                string sql = @"INSERT INTO Gamme (Nom,Finition_Id,Isolant_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",finitionId },
                {"@3",isolantId },
                {"@4", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@5", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@6", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
            };

                result = Insert(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
                //TODO
                //Logger.WriteEx(e);

            }

            return result;
        }

        public int UpdateModele(Gamme gammeLocal, Gamme gammeDistant)
        {
            //Vérification des clés étrangères
            if (gammeDistant.Finition == null)
                throw new Exception("Tentative d'insertion dans la table Gamme avec la clé étrangère Finition nulle");
            if (gammeDistant.Isolant == null)
                throw new Exception("Tentative d'insertion dans la table Gamme avec la clé étrangère Isolant nulle");

            //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
            if (!Synchronisation<FinitionDAL, Finition>.CorrespondanceModeleId.TryGetValue(gammeDistant.Finition.Id, out int finitionId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                finitionId = Synchronisation<FinitionDAL, Finition>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == gammeDistant.Finition.Id).Key;
            }
            if (!Synchronisation<IsolantDAL, Isolant>.CorrespondanceModeleId.TryGetValue(gammeDistant.Isolant.Id, out int isolantId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                isolantId = Synchronisation<IsolantDAL, Isolant>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == gammeDistant.Isolant.Id).Key;
            }
            //recopie des données de la Gamme distante dans la Gamme locale
            gammeLocal.Copy(gammeDistant);
            string sql = @"
                        UPDATE Gamme
                        SET Nom=@1,Finition_Id=@2,Isolant_Id=@3,MiseAJour=@4
                        WHERE Id=@5
                      ";

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",gammeLocal.Nom},
                {"@2",finitionId},
                {"@3",isolantId},
                {"@4",DateTimeDbAdaptor.FormatDateTime( gammeLocal.MiseAJour,Bdd) },
                {"@5",gammeLocal.Id },
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
