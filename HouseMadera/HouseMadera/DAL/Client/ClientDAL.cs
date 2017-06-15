using System;
using System.Collections.Generic;
using HouseMadera.Utilites;
using HouseMadera.Modeles;
using System.Data.Common;
using HouseMadera.Utilities;

namespace HouseMadera.DAL
{
    public class ClientDAL : DAL, IDAL<Client>
    {
        private string erreur;
        const string NON_RENSEIGNE = "NULL";

        public ClientDAL(string nomBdd) : base(nomBdd)
        {

        }



        /// <summary>
        /// Selectionne le premier client avec l'ID en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet Client</returns>
        public Client GetClient(int id)
        {

            string sql = @"SELECT * FROM Client WHERE Id = @1";
            Dictionary<string, object> parametres = new Dictionary<string, object>()
            {
                {"@1", id}
            };
            DbDataReader reader = Get(sql, parametres);
            Client client = new Client();
            while (reader.Read())
            {
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
            bool result = false;
            string sql = @"SELECT * 
                           FROM Client
                           WHERE Lower(Nom)=@1 AND Lower(Prenom)=@2 AND ( Mobile=@3 OR Telephone=@4) AND Email=@5 AND (Suppression ='' OR Suppression IS NULL)";
            Dictionary<string, object> parameters = new Dictionary<string, object> {
                {"@1",client.Nom.ToLower().Trim() },
                {"@2",client.Prenom.ToLower().Trim() },
                {"@3",client.Mobile },
                {"@4",client.Telephone },
                {"@5",client.Email }

            };
            List<Client> clients = new List<Client>();
            using (DbDataReader reader = Get(sql, parameters))
            {
                while (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtient une liste d'objet Client en fonction des critères de recherche colonne et valeur 
        /// </summary>
        /// <param name="filter">contient le nom de la colonne</param>
        /// <param name="value">contient la valeur à utiliser pour la recherche</param>
        /// <returns>Une liste d'objet Client résultante de la recherche</returns>
        public List<Client> GetFilteredClient(string filter, string value)
        {

            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return new List<Client>();

            filter = filter.Replace(" ", string.Empty);
            string colonne = (filter == "Adresse") ? "Adresse" + "1" : filter;

            Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                 {"@1", "%"+value+"%" }
             };

            string sql = @"
                             SELECT * FROM Client
                             WHERE " + colonne + @" LIKE @1
                             ORDER BY " + colonne + @" DESC";
            List<Client> clients = new List<Client>();
            DbDataReader reader = Get(sql, parameters);
            while (reader.Read())
            {
                Client client = initialiserClient(reader);
                if (client != null)
                    clients.Add(client);
            }
            return clients;

        }

        /// <summary>
        /// Retourne la liste des clients de la table Client dont le champ Suppression est null
        /// </summary>
        /// <returns>une liste d'objet Client </returns>
        public List<Client> GetAllClients()
        {
            string sql = @"
                            SELECT * FROM Client 
                            WHERE Suppression IS NULL OR Suppression =''
                            ORDER BY Nom DESC";
            List<Client> clients = new List<Client>();
            using (DbDataReader reader = Get(sql, null))
            {
                while (reader.Read())
                {
                    Client client = initialiserClient(reader);
                    if (client != null)
                        clients.Add(client);
                }
            }

            return clients;
        }

        #region SYNCHRONISATION

        /// <summary>
        /// Methode implémentée de l'interface IClientDAL permettant de récupérer tous les clients de la base
        /// </summary>
        /// <returns>Une liste d'objet Client</returns>
        public List<Client> GetAllModeles()
        {
            string sql = @"
                            SELECT * FROM Client
                            ORDER BY Nom DESC";
            List<Client> clients = new List<Client>();
            using (DbDataReader reader = Get(sql, null))
            {
                while (reader.Read())
                {
                    Client client = initialiserClient(reader);
                    if (client != null)
                        clients.Add(client);
                }
            }

            return clients;
        }

        /// <summary>
        /// Methode implémentée de l'interface IClientDAL permettant l'insertion d'un Client en base
        /// </summary>
        /// <param name="client">Le modèle à insérer</param>
        /// <returns>Le nombre de lignes affectées</returns>
        public int InsertModele(Client client,MouvementSynchronisation sens)
        {
            if (!isDataCorrect(client))
                throw new ValidationClientException(erreur);
            if (IsClientExist(client))
                throw new ClientException();

            string sql = @"INSERT INTO Client (Nom,Prenom,Adresse1,Adresse2,Adresse3,CodePostal,Ville,Email,Telephone,Mobile,StatutClient_Id,MiseAJour,Suppression,Creation)
                        VALUES(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14)";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
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
                {"@12", DateTimeDbAdaptor.FormatDateTime( client.MiseAJour,Bdd) },
                {"@13", DateTimeDbAdaptor.FormatDateTime( client.Suppression,Bdd) },
                {"@14", DateTimeDbAdaptor.FormatDateTime( client.Creation,Bdd) }
            };
            int result = 0;
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

        /// <summary>
        /// Methode implémentée de l'interface IClientDAL. Elle effectue une copie des valeurs du deuxième paramètre dans le premier
        /// et met à jour le client en base.
        /// </summary>
        /// <param name="clientLocal">Représente l'objet issue de la base locale </param>
        /// <param name="clientDistant">Représente l'objet issue de la base distante</param>
        /// <returns>Le nombre de lignes affectées</returns>
        public int UpdateModele(Client clientLocal, Client clientDistant,MouvementSynchronisation sens)
        {


            //recopie des données du client distant dans le client local
            if (clientDistant != null)
                clientLocal.Copy(clientDistant);

            //Vérifie la cohérence des données à mettre à jour
            if (!isDataCorrect(clientLocal))
                throw new Exception(erreur);

            string sql = @"
                        UPDATE Client
                        SET Nom=@1,Prenom=@2,Adresse1=@3,Adresse2=@4,Adresse3=@5,CodePostal=@6,Ville=@7,Email=@8,Telephone=@9,Mobile=@10,StatutClient_Id=@11,MiseAJour=@12
                        WHERE Id=@13
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",clientLocal.Nom},
                {"@2",clientLocal.Prenom},
                {"@3",clientLocal.Adresse1},
                {"@4",string.IsNullOrEmpty(clientLocal.Adresse2) ? NON_RENSEIGNE : clientLocal.Adresse2},
                {"@5",string.IsNullOrEmpty(clientLocal.Adresse3) ? NON_RENSEIGNE : clientLocal.Adresse3},
                {"@6",clientLocal.CodePostal },
                {"@7",clientLocal.Ville },
                {"@8",clientLocal.Email },
                {"@9",string.IsNullOrEmpty(clientLocal.Telephone) ? NON_RENSEIGNE : clientLocal.Telephone},
                {"@10",string.IsNullOrEmpty(clientLocal.Mobile) ? NON_RENSEIGNE : clientLocal.Mobile },
                {"@11",clientLocal.StatutClient },
                {"@12", DateTimeDbAdaptor.FormatDateTime( clientLocal.MiseAJour,Bdd)},
                {"@13",clientLocal.Id }
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

        /// <summary>
        /// Met à jour en base la date de suppression du client (suppression logique)
        /// </summary>
        /// <param name="client">Représente le client à effacer</param>
        /// <returns>Le nombre de lignes affectées</returns>
        public int DeleteModele(Client client)
        {
            if (!isDataCorrect(client))
                throw new Exception(erreur);

            string sql = @"
                        UPDATE Client
                        SET Suppression= @2
                        WHERE Id=@1
                      ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",client.Id},
                {"@2",DateTimeDbAdaptor.FormatDateTime(client.Suppression,Bdd)}

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

        #region METHODES

        /// <summary>
        /// Vérifie la cohérence des informations pour chaque propriété de l'objet passé en paramètre
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private bool isDataCorrect(Client client)
        {

            //statutClient ne doit pas être null
            if (client.StatutClient > 2 || client.StatutClient == 0)
            {
                erreur = "Le client n'a pas de statut \n";
            }

            RegexUtilities utils = new RegexUtilities();
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

        /// <summary>
        /// Instancie un objet Client avec les données contenue dans l'objet passé en paramètre
        /// </summary>
        /// <param name="reader">Objet contenant les informations de la base</param>
        /// <returns>Instance d'un objet Client</returns>
        private Client initialiserClient(DbDataReader reader)
        {
            Client client = new Client()
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
            string telephone = Convert.ToString(reader["telephone"]);
            client.Telephone = string.IsNullOrEmpty(telephone) || telephone == "NULL" ? string.Empty : telephone;
            string email = Convert.ToString(reader["email"]);
            client.Email = string.IsNullOrEmpty(email) || email == "NULL" ? string.Empty : email;
            client.StatutClient = Convert.ToInt32(reader["statutClient_id"]);
            client.MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["miseajour"]));
            client.Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["suppression"]));
            client.Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["creation"]));

            return client;
        }

        /// <summary>
        /// converti une chaine de caractère en objet DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns>retourne un objet DateTime ou null</returns>
        private DateTime? initialiserDate(string value)
        {
            DateTime maj = new DateTime();
            if (DateTime.TryParse(value, out maj))
                return maj;
            else
                return null;
        }

        #endregion
    }
}