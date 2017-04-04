using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace HouseMadera.DAL
{
    public class TypeComposantDAL : DAL, IDAL<TypeComposant>
    {
        public TypeComposantDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(TypeComposant modele)
        {
            string sql = @"
                        UPDATE TypeComposant
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

        public List<TypeComposant> GetAllModeles()
        {
            List<TypeComposant> listeTypesComposants = new List<TypeComposant>();
            try
            {

                string sql = @"SELECT t.*,q.Id AS qual_Id , q.Nom AS qual_Nom
                               FROM TypeComposant t
                               LEFT JOIN Qualite q ON t.Qualite_Id = q.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        TypeComposant t = new TypeComposant()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Qualite = new Qualite()
                            {
                                Id = Convert.ToInt32(reader["qual_Id"]),
                                Nom = Convert.ToString(reader["qual_Nom"]),
                            }

                        };
                        listeTypesComposants.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeTypesComposants;
        }

        public int InsertModele(TypeComposant modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.Qualite == null)
                    throw new Exception("Tentative d'insertion dans la table TypeComposant avec la clé étrangère Qualite nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                int qualiteId;
                if (!Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.TryGetValue(modele.Qualite.Id, out qualiteId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    qualiteId = Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.Qualite.Id).Key;

                }

                string sql = @"INSERT INTO TypeComposant (Nom,Qualite_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",qualiteId },
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

        public int UpdateModele(TypeComposant typeComposantLocal, TypeComposant typeComposantDistant)
        {
            //Vérification des clés étrangères
            if (typeComposantDistant.Qualite == null)
                throw new Exception("Tentative d'insertion dans la table TypeComposant avec la clé étrangère Qualite nulle");


            //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
            int qualiteId;
            if (!Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.TryGetValue(typeComposantDistant.Qualite.Id, out qualiteId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                qualiteId = Synchronisation<QualiteDAL, Qualite>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == typeComposantDistant.Qualite.Id).Key;

            }

            //recopie des données du TypeComposant distant dans le TypeComposant local
            typeComposantLocal.Copy(typeComposantDistant);
            string sql = @"
                        UPDATE TypeComposant
                        SET Nom=@1,Qualite_Id=@2,MiseAJour=@3
                        WHERE Id=@4
                      ";

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",typeComposantLocal.Nom},
                {"@2",qualiteId},
                {"@3",DateTimeDbAdaptor.FormatDateTime( typeComposantLocal.MiseAJour,Bdd) },
                {"@4",typeComposantLocal.Id },
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
