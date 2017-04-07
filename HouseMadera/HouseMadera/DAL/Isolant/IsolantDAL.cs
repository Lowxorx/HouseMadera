using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;


namespace HouseMadera.DAL
{
    public class IsolantDAL : DAL, IDAL<Isolant>
    {
        public IsolantDAL(string nomBdd) : base(nomBdd)
        {
        }

        public int DeleteModele(Isolant modele)
        {
            string sql = @"
                        UPDATE Isolant
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

        public List<Isolant> GetAllModeles()
        {
            List<Isolant> listeIsolants = new List<Isolant>();
            try
            {

                string sql = @"SELECT i.*,t.Id AS typeIso_Id , t.Nom AS typeIso_Nom
                               FROM Isolant i
                               LEFT JOIN TypeIsolant t ON i.TypeIsolant_Id = t.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Isolant i = new Isolant()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            TypeIsolant = new TypeIsolant()
                            {
                                Id = Convert.ToInt32(reader["typeIso_Id"]),
                                Nom = Convert.ToString(reader["typeIso_Nom"]),
                            } 

                        };
                        listeIsolants.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeIsolants;
        }

        public int InsertModele(Isolant modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.TypeIsolant == null)
                    throw new Exception("Tentative d'insertion dans la table Isolant avec la clé étrangère TypeIsolant nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<TypeIsolantDAL, TypeIsolant>.CorrespondanceModeleId.TryGetValue(modele.TypeIsolant.Id, out int typeIsolantId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeIsolantId = Synchronisation<TypeIsolantDAL, TypeIsolant>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeIsolant.Id).Key;

                }

                string sql = @"INSERT INTO Isolant (Nom,TypeIsolant_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",typeIsolantId },
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

        public int UpdateModele(Isolant isolantLocal, Isolant isolantDistant)
        {
            //Vérification des clés étrangères
            if (isolantDistant.TypeIsolant == null)
                throw new Exception("Tentative de mise a jour dans la table Isolant avec la clé étrangère TypeIsolant nulle");

            //Valeurs des clés étrangères est modifié avant update via la table de correspondance 
            if (!Synchronisation<ClientDAL, Client>.CorrespondanceModeleId.TryGetValue(isolantDistant.TypeIsolant.Id, out int typeIsolantId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                typeIsolantId = Synchronisation<ClientDAL, Client>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == isolantDistant.TypeIsolant.Id).Key;
            }

            //recopie des données du Isolant distant dans le Isolant local
            isolantLocal.Copy(isolantDistant);
            string sql = @"
                        UPDATE Isolant
                        SET Nom=@1,TypeIsolant_Id=@2,MiseAJour=@3
                        WHERE Id=@4
                      ";

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",isolantLocal.Nom},
                {"@2",typeIsolantId},
                {"@3",DateTimeDbAdaptor.FormatDateTime( isolantLocal.MiseAJour,Bdd) },
                {"@4",isolantLocal.Id },
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
    }
}
