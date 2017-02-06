using System;
using System.Collections.Generic;
using HouseMadera.Utilites;
using HouseMadera.Modeles;

namespace HouseMadera.DAL
{
    public class ClientDAL : DAL
    {
        private string erreur;
        const string NON_RENSEIGNE = "NULL";

        public ClientDAL(string nomBdd) : base(nomBdd)
        {

        }

        #region READ

        /// <summary>
        /// Selectionne tous les clients enregistrés en base
        /// </summary>
        /// <returns>Une liste d'objets Client</returns>
        public List<Client> GetAllClients()
        {
            string sql = @"
                            SELECT * FROM Client
                            ORDER BY Nom DESC";
            var clients = new List<Client>();
            var reader = Get(sql, null);
            while (reader.Read())
            {
                var client = new Client();
                client.Id = Convert.ToInt32(reader["id"]);
                client.Nom = Convert.ToString(reader["nom"]);
                client.Prenom = Convert.ToString(reader["prenom"]);
                client.Adresse1 = Convert.ToString(reader["adresse1"]);
                client.Adresse2 = Convert.ToString(reader["adresse2"]);
                client.Adresse3 = Convert.ToString(reader["adresse3"]);
                client.Mobile = Convert.ToString(reader["mobile"]);
                client.Telephone = Convert.ToString(reader["telephone"]);
                clients.Add(client);
            }
            //******************************** Sans factorisation **************************
            //if (Bdd == "SQLITE")
            //{
            //    SQLiteCommand commandSQlite = new SQLiteCommand(sql, BddSQLite);
            //    SQLiteDataReader readerSQLite = commandSQlite.ExecuteReader();

            //    while (readerSQLite.Read())
            //    {
            //        var client = new Modeles.Client();
            //        client.Id = Convert.ToInt32(readerSQLite["Id"]);
            //        client.Nom = Convert.ToString(readerSQLite["Nom"]);
            //        client.Prenom = Convert.ToString(readerSQLite["Prenom"]);
            //        client.Adresse1 = Convert.ToString(readerSQLite["Adresse1"]);
            //        client.Adresse2 = Convert.ToString(readerSQLite["Adresse2"]);
            //        client.Adresse3 = Convert.ToString(readerSQLite["Adresse3"]);
            //        client.Mobile = Convert.ToString(readerSQLite["Mobile"]);
            //        client.Telephone = Convert.ToString(readerSQLite["Telephone"]);
            //        clients.Add(client);
            //    }
            //}
            //if(Bdd == "MYSQL")
            //{
            //    MySqlCommand commandMySql = new MySqlCommand(sql, BddMySql);
            //    MySqlDataReader readerMySql = commandMySql.ExecuteReader();
            //    while (readerMySql.Read())
            //    {
            //        var client = new Modeles.Client();
            //        client.Id = Convert.ToInt32(readerMySql["Id"]);
            //        client.Nom = Convert.ToString(readerMySql["Nom"]);
            //        client.Prenom = Convert.ToString(readerMySql["Prenom"]);
            //        client.Adresse1 = Convert.ToString(readerMySql["Adresse1"]);
            //        client.Adresse2 = Convert.ToString(readerMySql["Adresse2"]);
            //        client.Adresse3 = Convert.ToString(readerMySql["Adresse3"]);
            //        client.Mobile = Convert.ToString(readerMySql["Mobile"]);
            //        client.Telephone = Convert.ToString(readerMySql["Telephone"]);
            //        clients.Add(client);
            //    }

            //}
            return clients;
        }

        /// <summary>
        /// Selectionne le premier client avec l'ID en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet Client</returns>
        public static Client GetClient(int id)
        {

            string sql = @"SELECT * FROM Client WHERE Id = @1";
            var parametres = new Dictionary<string, object>()
            {
                {"@1", id}
            };
            var reader = Get(sql, parametres);
            var client = new Client();
            while (reader.Read())
            {
                client.Id = Convert.ToInt32(reader["id"]);
                client.Nom = Convert.ToString(reader["nom"]);
                client.Prenom = Convert.ToString(reader["prenom"]);
                client.Adresse1 = Convert.ToString(reader["adresse1"]);
                client.Adresse2 = Convert.ToString(reader["adresse2"]);
                client.Adresse3 = Convert.ToString(reader["adresse3"]);
                client.Mobile = Convert.ToString(reader["mobile"]);
                client.Telephone = Convert.ToString(reader["telephone"]);
            }
            return client;
        }

        /// <summary>
        /// Vérifie en interrogeant la base si un client est déjà enregistré
        /// </summary>
        /// <param name="client"></param>
        /// <returns>"true" si le client existe déjà en base</returns>
        private bool IsClientExist(Client client)
        {
            var result = false;
            string sql = @"SELECT * FROM Client WHERE Nom=@1 AND Prenom=@2 AND Mobile=@3 OR Telephone=@4 AND Email=@5";
            var parameters = new Dictionary<string, object> {
                {"@1",client.Nom },
                {"@2",client.Prenom },
                {"@3",client.Mobile },
                {"@4",client.Telephone },
                {"@5",client.Email }

            };
            var clients = new List<Client>();
            using (var reader = Get(sql, parameters))
            {
                while (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Réalise des test sur les propriétés de l'objet Client
        /// avant insertion en base.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Le nombre de ligne affecté en base. -1 si aucune ligne insérée</returns>
        public int InsertClient(Client client)
        {
            if (!IsDataCorrect(client))
                throw new Exception(erreur);
            if (IsClientExist(client))
                throw new Exception("le client est déjà enregistré");

            var sql = @"INSERT INTO Client (Nom,Prenom,Adresse1,Adresse2,Adresse3,CodePostal,Ville,Email,Telephone,Mobile,StatutClient_Id)
                        VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)";
            var parameters = new Dictionary<string, object>() {
                {"@1",client.Nom },
                {"@2",client.Prenom },
                {"@3",client.Adresse1 },
                {"@4",client.Adresse2 },
                {"@5",client.Adresse3 },
                {"@6",client.CodePostal },
                {"@7",client.Ville },
                {"@8",client.Email },
                {"@9",client.Telephone },
                {"@10",client.Mobile },
                {"@11",client.StatutClient },
            };
            var result = 0;
            try
            {
                result = Insert(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
            }

            return result;
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Teste les nouvelles données à insérer et met à jour les données du client présent en base
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Le nombre de ligne affecté en base. -1 si aucune ligne affectée</returns>
        public int UpdateClient(Client client)
        {
            if (!IsDataCorrect(client))
                throw new Exception(erreur);

            var sql = @"
                        UPDATE Client
                        SET Nom=@1,Prenom=@2,Adresse1=@3,Adresse2=@4,Adresse3=@5,CodePostal=@6,Ville=@7,Email=@8,Telephone=@9,Mobile=@10,StatutClient_Id=@11
                        WHERE Id=@12
                      ";
            var parameters = new Dictionary<string, object>() {
                {"@1",client.Nom},
                {"@2",client.Prenom},
                {"@3",client.Adresse1},
                {"@4",string.IsNullOrEmpty(client.Adresse2) ? NON_RENSEIGNE : client.Adresse2},
                {"@5",string.IsNullOrEmpty(client.Adresse3) ? NON_RENSEIGNE : client.Adresse3},
                {"@6",client.CodePostal },
                {"@7",client.Ville },
                {"@8",client.Email },
                {"@9",string.IsNullOrEmpty(client.Telephone) ? NON_RENSEIGNE : client.Telephone},
                {"@10",string.IsNullOrEmpty(client.Mobile) ? NON_RENSEIGNE : client.Mobile },
                {"@11",client.StatutClient },
                {"@12",client.Id }
            };
            var result = 0;
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

        #region DELETE
        /// <summary>
        /// Efface en base le client avec l'Id en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Le nombre de ligne affecté en base. -1 si aucune ligne affectée</returns>
        public int DeleteClient(int id)
        {
            var sql = @"
                        DELETE FROM Client
                        WHERE Id=@1
                      ";
            var parameters = new Dictionary<string, object>() {
                {"@1",id },
            };
            var result = 0;
            try
            {
                result = Delete(sql, parameters);
            }
            catch (Exception e)
            {
                result = -1;
                Console.WriteLine(e.Message);
            }

            return result;
        }
        #endregion

        private bool IsDataCorrect(Client client)
        {
           
            //statutClient ne doit pas être null
            if (client.StatutClient > 2 || client.StatutClient == 0)
            {
                erreur = "Le client n'a pas de statut \n";
            }

            var utils = new RegexUtilities();
            //Test de validite de l'email si vide
            if (!utils.IsValidEmail(client.Email))
                erreur += "L'email n'est pas au bon format \n";
            if (string.IsNullOrEmpty(client.Email))
                erreur += "L'email est obligatoire";
            //Test de validité des numéros de téléphone au moins un des deux renseignés
            if (!utils.IsValidTelephoneNumber(client.Telephone))
                erreur += "le numero de téléphone devrait être 0xxxxxxxxx \n";
            if (!utils.IsValidTelephoneNumber(client.Mobile))
                erreur += "le numero de mobile devrait être 0xxxxxxxxx \n";
            if (string.IsNullOrEmpty(client.Telephone) && string.IsNullOrEmpty(client.Mobile))
                erreur += "Au moins un numero de telephone doit être renseigné";
 

            //Test des autres données
            if (string.IsNullOrEmpty(client.Nom))
                erreur += "le nom n'est pas renseigné \n";
            if (string.IsNullOrEmpty(client.Prenom))
                erreur += "le nom n'est pas renseigné \n";
            if (string.IsNullOrEmpty(client.Adresse1))
                erreur += "l'adresse n'est pas renseigné \n";
            if (string.IsNullOrEmpty(client.CodePostal))
                erreur += "le code postal n'est pas renseigné \n";
            if (string.IsNullOrEmpty(client.Ville))
                erreur += "la ville n'est pas renseignée \n";

            return string.IsNullOrEmpty(erreur);
        }

    }
}
