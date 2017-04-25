using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace HouseMadera.DAL
{
    public class TypeFinitionDAL : DAL, IDAL<TypeFinition>
    {
        public TypeFinitionDAL(string nomBdd) : base(nomBdd)
        {
        }


        #region SYNCHRONISATION
        public int DeleteModele(TypeFinition modele)
        {
            string sql = @"
                        UPDATE TypeFinition
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

        public List<TypeFinition> GetAllModeles()
        {
            List<TypeFinition> listeTypeFinitions = new List<TypeFinition>();
            try
            {

                string sql = @"SELECT t.*,q.Id AS qualite_Id , q.Nom AS qualite_Nom
                               FROM TypeFinition t
                               LEFT JOIN Qualite q ON t.Qualite_Id = q.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        TypeFinition t = new TypeFinition()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Qualite = new Qualite()
                            {
                                Id = Convert.ToInt32(reader["qualite_Id"]),
                                Nom = Convert.ToString(reader["qualite_Nom"]),
                            }
                        };
                        listeTypeFinitions.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeTypeFinitions;
        }

        public int InsertModele(TypeFinition modele, MouvementSynchronisation sens)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Qualite == null)
                    throw new Exception("Tentative d'insertion dans la base Projet avec la clé étrangère TypeIsolant nulle");

                int qualiteId = 0;
                if (sens == MouvementSynchronisation.Sortant)
                    Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.TryGetValue(modele.Qualite.Id, out qualiteId);
                else
                    qualiteId = Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Qualite.Id).Key;
                ////Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                //if (!Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.TryGetValue(modele.Qualite.Id, out int qualiteId))
                //{
                //    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                //    qualiteId = Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Qualite.Id).Key;

                //}

                string sql = @"INSERT INTO TypeFinition (Nom,Qualite_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",qualiteId},
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
                
                //Logger.WriteEx(e);

            }

            return result;
        }

        public int UpdateModele(TypeFinition typeFinitionLocal, TypeFinition typeFinitionDistant, MouvementSynchronisation sens)
        {
            //Vérification des clés étrangères
            if (typeFinitionLocal.Qualite == null)
                throw new Exception(string.Format("Tentative de mise a jour dans la table {0} avec la clé étrangère Qualite nulle",typeFinitionLocal.GetType()));

            int qualiteId = 0;
            if (sens == MouvementSynchronisation.Sortant)
                Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.TryGetValue(typeFinitionDistant.Qualite.Id, out qualiteId);
            else
                qualiteId = Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == typeFinitionDistant.Qualite.Id).Key;

            ////Valeurs des clés étrangères est modifié avant update via la table de correspondance 
            //if (!Synchronisation<TypeFinitionDAL, TypeFinition>.CorrespondanceModeleId.TryGetValue(typeFinitionDistant.Qualite.Id, out int qualiteId))
            //{
            //    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
            //    qualiteId = Synchronisation<TypeFinitionDAL, TypeFinition>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == typeFinitionDistant.Qualite.Id).Key;
            //}

            //recopie des données du TypeFinition distant dans le TypeFinition local
            typeFinitionLocal.Copy(typeFinitionDistant);
            string sql = @"
                        UPDATE TypeFinition
                        SET Nom=@1,Qualite_Id=@2,MiseAJour=@3
                        WHERE Id=@4
                      ";

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",typeFinitionLocal.Nom},
                {"@2",qualiteId},
                {"@3",DateTimeDbAdaptor.FormatDateTime( typeFinitionLocal.MiseAJour,Bdd) },
                {"@4",typeFinitionLocal.Id },
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
