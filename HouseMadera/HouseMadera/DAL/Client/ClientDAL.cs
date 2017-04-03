using System;
using System.Collections.Generic;
using HouseMadera.Utilities;
using HouseMadera.Modeles;
using System.Data.Common;

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
                Client client = initialiserClient(reader);
                if (client != null)
                    clients.Add(client);
            }
            return clients;
        }

        /// <summary>
        /// Selectionne le premier client avec l'ID en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet Client</returns>
        public Client GetClient(int id)
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
                //client.Id = Convert.ToInt32(reader["id"]);
                //client.Nom = Convert.ToString(reader["nom"]);
                //client.Prenom = Convert.ToString(reader["prenom"]);
                //client.Adresse1 = Convert.ToString(reader["adresse1"]);
                //client.Adresse2 = Convert.ToString(reader["adresse2"]);
                //client.Adresse3 = Convert.ToString(reader["adresse3"]);
                //client.Mobile = Convert.ToString(reader["mobile"]);
                //client.Telephone = Convert.ToString(reader["telephone"]);
                client = initialiserClient(reader);
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

        public List<Client> GetFilteredClient(string filter, string value)
        {

            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return new List<Client>();

            filter = filter.Replace(" ", string.Empty);
            var colonne = (filter == "Adresse") ? "Adresse" + "1" : filter;

            var parameters = new Dictionary<string, object>()
             {
                 {"@1", "%"+value+"%" }
             };

            string sql = @"
                             SELECT * FROM Client
                             WHERE " + colonne + @" LIKE @1
                             ORDER BY " + colonne + @" DESC";
            var clients = new List<Client>();
            var reader = Get(sql, parameters);
            while (reader.Read())
            {
                Client client = initialiserClient(reader);
                if (client != null)
                    clients.Add(client);
            }
            return clients;

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
            if (!isDataCorrect(client))
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
            if (!isDataCorrect(client))
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
        
        /// <summary>
        /// Permet de vérifier si la validité des données du formulaire 
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Une erreur si il y en a une</returns>
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

        private Client initialiserClient(DbDataReader reader)
        {
            var client = new Client()
            {
                Id = Convert.ToInt32(reader["id"]),
                Nom = Convert.ToString(reader["nom"]),
                Prenom = Convert.ToString(reader["prenom"]),
                Adresse1 = Convert.ToString(reader["adresse1"])
            };
            string adresse2 = Convert.ToString(reader["adresse2"]);
            client.Adresse2 = string.IsNullOrEmpty(adresse2) || adresse2 == "NULL" ? string.Empty : adresse2;
            string adresse3 = Convert.ToString(reader["adresse3"]);
            client.Adresse3 = string.IsNullOrEmpty(adresse3) || adresse3 == "NULL" ? string.Empty : adresse3;
            string codePostal = Convert.ToString(reader["codePostal"]);
            client.CodePostal = string.IsNullOrEmpty(codePostal) || codePostal == "NULL" ? string.Empty : codePostal;
            string ville = Convert.ToString(reader["ville"]);
            client.Ville = string.IsNullOrEmpty(ville) || codePostal == "NULL" ? string.Empty : ville;
            string mobile = Convert.ToString(reader["mobile"]);
            client.Mobile = string.IsNullOrEmpty(mobile) || mobile == "NULL" ? string.Empty : mobile;
            var telephone = Convert.ToString(reader["telephone"]);
            client.Telephone = string.IsNullOrEmpty(telephone) || telephone == "NULL" ? string.Empty : telephone;
            string email = Convert.ToString(reader["email"]);
            client.Email = string.IsNullOrEmpty(email) || email == "NULL" ? string.Empty : email;
            client.StatutClient = Convert.ToInt32(reader["statutClient_id"]);
            return client;
        }
        #endregion

    }
}
