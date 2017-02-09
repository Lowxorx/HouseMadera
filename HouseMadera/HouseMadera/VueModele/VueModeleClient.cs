
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilites;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleClient : ViewModelBase, IDataErrorInfo
    {
        public ICommand EditClient { get; private set; }


        #region
        private string _nom;
        public string Nom { get; set; }
        private string _prenom;
        public string Prenom { get; set; }
        private string _numero;
        public string Numero { get; set; }
        private string _voie;
        public string Voie { get; set; }
        private string _complement;
        public string Complement { get; set; }
        private int _codepostal;
        public int CodePostal { get; set; }
        private string _localite;
        public string Localite { get; set; }
        private string _mobile;
        public string Mobile { get; set; }
        private string _telephone;
        public string Telephone { get; set; }
        private string _email;
        public string Email { get; set; }
        private string recherche;
        public string Recherche
        {
            get
            {
                return recherche;
            }
            set
            {
                if (Filtre != null && Clients != null)
                {

                    List<Client> clientsTrouves = new List<Client>();
                    //Si le textbox est vide on affiche tous les clients
                    if (string.IsNullOrEmpty(value))
                        clientsTrouves = AfficherClient();
                    else
                        clientsTrouves = AfficherClientFiltre(Filtre, value);
                    Clients = new ObservableCollection<Client>(clientsTrouves);
                    RaisePropertyChanged(() => Clients);
                    recherche = value;
                    RaisePropertyChanged(() => Recherche);
                }

            }
        }
        private string filtre;
        public string Filtre
        {
            get { return filtre; }
            set { filtre = value; }
        }
        public string NomCommune { get; set; }
        public List<string> filtres { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public RegexUtilities reg { get; set; }
        #endregion

        public VueModeleClient()
        {
            EditClient = new RelayCommand(EClient);
            reg = new RegexUtilities();
            Clients = new ObservableCollection<Client>(AfficherClient());
            filtres = new List<string>() { "Nom", "Adresse" };

        }

        private void EClient()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueClientEdit vce = new VueClientEdit();
            vce.Show();
            window.Close();
        }


        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region REGLES DE VALIDATION
        public string this[string columnName]
        {
            get
            {
                var result = string.Empty;
                switch (columnName)
                {
                    case "Nom":
                        if (Nom == null)
                            break;
                        result = reg.IsValidName(Nom) ? "Le nom ne doit pas contenir de chiffre" : string.Empty;
                        break;
                    case "Prenom":
                        if (Prenom == null)
                            break;
                        result = reg.IsValidName(Prenom) ? "Le prenom ne doit pas contenir de chiffre" : string.Empty;
                        break;
                    case "Numero":
                        if (Numero == null)
                            break;
                        result = reg.IsValidNumeroVoie(Numero) ? string.Empty : "Le format du numéro de voie incorrect ex 6,6 Bis, 6 ter ";
                        break;
                    case "Voie":
                        if (Voie == null)
                            break;
                        result = reg.HasSpecialCharacters(Voie) ? string.Empty : "Caractères spéciaux non admis ";
                        break;
                    case "Complement":
                        if (Complement == null)
                            break;
                        result = reg.HasSpecialCharacters(Complement) ? string.Empty : "Caractères spéciaux non admis ";
                        break;
                    case "Mobile":
                        if (Mobile == null)
                            break;
                        result = reg.IsValidTelephoneNumber(Mobile) ? string.Empty : "Format admis ex: 0xxxxxxxxx";
                        break;
                    case "Telephone":
                        if (Telephone == null)
                            break;
                        result = reg.IsValidTelephoneNumber(Telephone) ? string.Empty : "Format admis ex: 0xxxxxxxxx";
                        break;
                    case "Email":
                        if (Email == null)
                            break;
                        result = reg.IsValidEmail(Email) ? string.Empty : "Format de l'e-mail non valide ex: jean.dupont@exemple.fr ";
                        break;
                }

                return result;
            }
        }
        #endregion

        /// <summary>
        /// Recupère en base tous les clients
        /// </summary>
        /// <returns>La liste de tous les clients</returns>
        public List<Client> AfficherClient()
        {
            List<Client> clients = new List<Client>();
            using (var dal = new ClientDAL("SQLITE"))
            {
                clients = dal.GetAllClients();
            }
            return clients;
        }

        /// <summary>
        /// Récupère la liste des clients filtrés 
        /// </summary>
        /// <param name="filtre">nom de la colonne de la grille à filtrer</param>
        /// <param name="valeur">valeur saisie dans la textbox</param>
        /// <returns>Une liste de clients</returns>
        public List<Client> AfficherClientFiltre(string filtre, string valeur)
        {
            List<Client> clients = new List<Client>();
            //TODO SQLITE à remplacer par Bdd
            using (var dal = new ClientDAL("SQLITE"))
            {
                clients = dal.GetFilteredClient(filtre, valeur);
            }

            return clients;
        }

        private string ExtractNomCommune(string communeSelected)
        {
            var offset = 7;
            return communeSelected.Substring(offset, communeSelected.Length - offset);
        }

        private string ExtractCodePostal(string communeSelected)
        {
            var offset = 0;
            return communeSelected.Substring(offset, 5);
        }
    }
}
