using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace HouseMadera.DAL
{
    public class FinitionDAL : DAL, IDAL<Finition>
    {
        public FinitionDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(Finition modele)
        {
            string sql = @"
                        UPDATE Finition
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

        public List<Finition> GetAllModeles()
        {
            List<Finition> listeFinitions = new List<Finition>();
            try
            {

                string sql = @"SELECT f.*,t.Id AS typeFin_Id , t.Nom AS typeFin_Nom
                               FROM Finition f
                               LEFT JOIN TypeFinition t ON f.TypeFinition_Id = t.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Finition f = new Finition()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            TypeFinition = new TypeFinition()
                            {
                                Id = Convert.ToInt32(reader["typeFin_Id"]),
                                Nom = Convert.ToString(reader["typeFin_Nom"]),
                            }

                        };
                        listeFinitions.Add(f);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeFinitions;
        }

        public int InsertModele(Finition modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.TypeFinition == null)
                    throw new Exception("Tentative d'insertion dans la base Finition avec la clé étrangère TypeFinition nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                int typeFinitionId;
                if (!Synchronisation<TypeFinitionDAL, TypeFinition>.CorrespondanceModeleId.TryGetValue(modele.TypeFinition.Id, out typeFinitionId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeFinitionId = Synchronisation<TypeFinitionDAL, TypeFinition>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeFinition.Id).Key;

                }

                string sql = @"INSERT INTO Finition (Nom,TypeFinition_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",typeFinitionId },
                {"@3", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                {"@4", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                {"@5", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) }
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

        public int UpdateModele(Finition finitionLocal, Finition finitionDistant)
        {
            //Vérification des clés étrangères
            if (finitionDistant.TypeFinition == null)
                throw new Exception("Tentative de mise a jour dans la table Isolant avec la clé étrangère TypeIsolant nulle");

            //Valeurs des clés étrangères est modifié avant update via la table de correspondance 
            int typeFinitionId;
            if (!Synchronisation<ClientDAL, Client>.CorrespondanceModeleId.TryGetValue(finitionDistant.TypeFinition.Id, out typeFinitionId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                typeFinitionId = Synchronisation<ClientDAL, Client>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == finitionDistant.TypeFinition.Id).Key;
            }

            //recopie des données de la Finition distante dans la Finition locale
            finitionLocal.Copy(finitionDistant);
            string sql = @"
                        UPDATE Finition
                        SET Nom=@1,TypeFinition_Id=@2,MiseAJour=@3
                        WHERE Id=@4
                      ";

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",finitionLocal.Nom},
                {"@2",typeFinitionId},
                {"@3",DateTimeDbAdaptor.FormatDateTime( finitionLocal.MiseAJour,Bdd) },
                {"@4",finitionLocal.Id },
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
