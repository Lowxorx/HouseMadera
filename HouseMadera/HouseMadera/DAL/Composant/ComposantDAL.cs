using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.DAL
{
    public class ComposantDAL : DAL, IDAL<Composant>
    {
        public ComposantDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(Composant modele)
        {
            string sql = @"
                        UPDATE Composant
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

        public List<Composant> GetAllModeles()
        {
            List<Composant> listeComposants = new List<Composant>();
            try
            {

                string sql = @"SELECT c.*,t.Id AS typeCom_Id , t.Nom AS typeCom_Nom
                               FROM Composant c
                               LEFT JOIN TypeComposant t ON c.TypeComposant_Id = t.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        Composant c = new Composant()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = Convert.ToString(reader["Nom"]),
                            Prix = Convert.ToDecimal(reader["Prix"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            TypeComposant = new TypeComposant()
                            {
                                Id = Convert.ToInt32(reader["typeCom_Id"]),
                                Nom = Convert.ToString(reader["typeCom_Nom"]),
                            }

                        };
                        listeComposants.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeComposants;
        }

        public int InsertModele(Composant modele)
        {
            int result = 0;
            try
            {
                //Vérification des clés étrangères
                if (modele.TypeComposant == null)
                    throw new Exception("Tentative d'insertion dans la table Composant avec la clé étrangère TypeComposant nulle");


                //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
                if (!Synchronisation<TypeComposantDAL, TypeComposant>.CorrespondanceModeleId.TryGetValue(modele.TypeComposant.Id, out int typeComposantId))
                {
                    //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                    typeComposantId = Synchronisation<TypeComposantDAL, TypeComposant>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == modele.TypeComposant.Id).Key;

                }

                string sql = @"INSERT INTO Composant (Nom,Prix,TypeComposant_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Nom },
                {"@2",modele.Prix },
                {"@3",typeComposantId },
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
                
                //Logger.WriteEx(e);

            }

            return result;
        }

        public int UpdateModele(Composant composantLocal, Composant composantDistant)
        {
            //Vérification des clés étrangères
            if (composantDistant.TypeComposant == null)
                throw new Exception("Tentative d'insertion dans la table Composant avec la clé étrangère TypeComposant nulle");

            //Valeurs des clés étrangères est modifié avant insertion via la table de correspondance 
            if (!Synchronisation<TypeComposantDAL, TypeComposant>.CorrespondanceModeleId.TryGetValue(composantDistant.TypeComposant.Id, out int typeComposantId))
            {
                //si aucune clé existe avec l'id passé en paramètre alors on recherche par valeur
                typeComposantId = Synchronisation<TypeComposantDAL, TypeComposant>.CorrespondanceModeleId.FirstOrDefault(c => c.Value == composantDistant.TypeComposant.Id).Key;
            }

            //recopie des données du Composant distant dans le Composant local
            composantLocal.Copy(composantDistant);
            string sql = @"
                        UPDATE Composant
                        SET Nom=@1,Prix=@2,TypeComposant_Id=@3,MiseAJour=@4
                        WHERE Id=@5
                      ";

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",composantLocal.Nom},
                {"@2",composantLocal.Prix},
                {"@3",typeComposantId},
                {"@4",DateTimeDbAdaptor.FormatDateTime( composantLocal.MiseAJour,Bdd) },
                {"@5",composantLocal.Id },
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
