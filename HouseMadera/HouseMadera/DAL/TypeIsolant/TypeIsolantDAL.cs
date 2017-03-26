using HouseMadera.DAL.Interfaces;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace HouseMadera.DAL
{
    public class TypeIsolantDAL : DAL, ITypeIsolantDAL
    {
        public TypeIsolantDAL(string nomBdd) : base(nomBdd)
        {
        }


        #region SYNCHRONISATION
        public int DeleteModele(TypeIsolant modele)
        {
            string sql = @"
                        UPDATE TypeIsolant
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

        public List<TypeIsolant> GetAllModeles()
        {
            List<TypeIsolant> listeTypeIsolant = new List<TypeIsolant>();
            try
            {

                string sql = @"SELECT t.*, q.Id AS qualite_Id, q.Nom AS qualite_Nom 
                               FROM TypeIsolant t
                               LEFT JOIN Qualite q ON q.Id = t.Qualite_Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        TypeIsolant t = new TypeIsolant()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            Qualite = new Qualite()
                            {
                                Id = Convert.ToInt32(reader["qualite_Id"]),
                                Nom = Convert.ToString(reader["qualite_Nom"])
                            },
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                        };
                        listeTypeIsolant.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeTypeIsolant;
        }

        public int InsertModele(TypeIsolant modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Qualite == null)
                    throw new Exception("Tentative d'insertion dans la table TypeIsolant avec la clé étrangère Qualite nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                int qualiteId;
                if (!Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.TryGetValue(modele.Qualite.Id, out qualiteId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    qualiteId = Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Qualite.Id).Key;

                }

                string sql = @"INSERT INTO TypeIsolant (Nom,MiseAJour,Suppression,Creation,Qualite_Id)
                        VALUES(@1,@2,@3,@4,@5)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                     {"@1",modele.Nom },
                     {"@2", DateTimeDbAdaptor.FormatDateTime( modele.MiseAJour,Bdd) },
                     {"@3", DateTimeDbAdaptor.FormatDateTime( modele.Suppression,Bdd) },
                     {"@4", DateTimeDbAdaptor.FormatDateTime( modele.Creation,Bdd) },
                     {"@5", qualiteId }
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

        public int UpdateModele(TypeIsolant typeIsolantLocal, TypeIsolant typeIsolantDistant)
        {
            int result = 0;
            try
            {

                //Vérification des clés étrangères
                if (typeIsolantDistant.Qualite == null)
                    throw new Exception("Tentative d'insertion dans la table TypeIsolant avec la clé étrangère Qualite nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                int qualiteId;
                if (!Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.TryGetValue(typeIsolantDistant.Qualite.Id, out qualiteId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    qualiteId = Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == typeIsolantDistant.Qualite.Id).Key;

                }

                // recopie des données du TypeIsolant distant dans le TypeIsolant local
                typeIsolantLocal.Copy<TypeIsolant>(typeIsolantDistant);

                string sql = @"
                        UPDATE TypeIsolant
                        SET Nom=@1,MiseAJour=@2,Qualite_Id=@3 
                        WHERE Id=@4
                      ";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                        {"@1",typeIsolantLocal.Nom},
                        {"@2", DateTimeDbAdaptor.FormatDateTime( typeIsolantLocal.MiseAJour,Bdd)},
                        {"@3",qualiteId  },
                        {"@4",typeIsolantLocal.Id }
                 };


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
