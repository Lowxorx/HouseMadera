
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilites;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System;
using MahApps.Metro.Controls.Dialogs;
using HouseMadera.VueModele;

namespace HouseMadera.VueModele
{
    public class VueModeleClientList : ViewModelBase
    {

        #region PROPRIETES
        /// <summary>
        /// Recherche lié à la textbox recherche
        /// </summary>
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

        /// <summary>
        /// Filtre selectionné dans la combobox
        /// </summary>
        private string filtre;
        public string Filtre
        {
            get { return filtre; }
            set
            {
                correspondanceFiltresColonnes.TryGetValue(value, out filtre);
            }
        }

        /// <summary>
        /// Commandes lié aux boutons de la vue
        /// </summary>
        public ICommand EditClient { get; private set; }
        public ICommand ModifClient { get; private set; }
        public ICommand Deconnexion { get; private set; }

        /// <summary>
        /// Clients lié à la datagrid
        /// </summary>
        public ObservableCollection<Client> Clients { get; set; }
        private Dictionary<string, string> correspondanceFiltresColonnes;

        /// <summary>
        /// filtres affichés dans la combobox
        /// </summary>
        private List<string> filtres;
        public List<string> Filtres
        {
            get { return filtres; }
            set { filtres = value; }
        }

        /// <summary>
        /// Client Selectionne lié à une ligne de la datagrid
        /// </summary>
        private Client clientSelectionne;
        public Client ClientSelectionne
        {
            get { return clientSelectionne; }
            set
            {
                if (value != null)
                {
                    clientSelectionne = value;
                    RaisePropertyChanged(() => ClientSelectionne);
                    IsClientSelected = true;
                }


            }
        }

        /// <summary>
        /// IsClientSelected lié au bouton de modification
        /// </summary>
        private bool isClientSelected;
        public bool IsClientSelected
        {
            get { return isClientSelected; }
            set
            {
                isClientSelected = value;
                RaisePropertyChanged(() => IsClientSelected);
            }
        }
        #endregion

        public VueModeleClientList()
        {
            EditClient = new RelayCommand(EClient);
            Deconnexion = new RelayCommand(Deconnecter);
            ModifClient = new RelayCommand(ModifierClient);
            Clients = new ObservableCollection<Client>(AfficherClient());
            IsClientSelected = false;
            correspondanceFiltresColonnes = new Dictionary<string, string>()
            {
                { "Nom", "Nom"},
                { "Adresse","Adresse1"},
                { "Code Postal","CodePostal"},
                { "Localite","Ville"},
                { "Mobile","Mobile"},
                { "Telephone","Telephone"},
                { "Email","Email"}
                };
            Filtres = getFiltresFromDictionnary(correspondanceFiltresColonnes);
        }

        #region METHODES
        /// <summary>
        ///  Récupère les filtres à afficher dans la combobox
        /// </summary>
        /// <param name="correspondanceFiltresColonnes"></param>
        /// <returns></returns>
        private List<string> getFiltresFromDictionnary(Dictionary<string, string> correspondanceFiltresColonnes)
        {
            List<string> filtres = new List<string>();
            foreach (var paires in correspondanceFiltresColonnes)
            {
                filtres.Add(paires.Key);
            }
            return filtres;
        }

        /// <summary>
        /// Ferme la fenetre courante et affiche la fenetre d'édition du client avec les champs pré-remplis
        /// </summary>
        private void ModifierClient()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueClientEdit vce = new VueClientEdit();
            VueModeleClientEdit vm = new VueModeleClientEdit();
            vm.InitVueModele(ClientSelectionne);
            vce.DataContext = vm;
            vce.Show();
            window.Close();
        }

        /// <summary>
        /// Ferme la fenetre courante et affiche la nouvelle fenêtre
        /// </summary>
        private void EClient()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueClientEdit vce = new VueClientEdit();
            vce.Show();
            window.Close();
        }

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
        /// <summary>
        /// Affiche un dialogue de confirmation puis redirige vers la page de connexion
        /// </summary>
        private async void Deconnecter()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment vous déconnecter ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    AnimateHide = false,
                    AnimateShow = true
                });

                if (result == MessageDialogResult.Affirmative)
                {
                    VueLogin vl = new VueLogin();
                    vl.Show();
                    window.Close();
                }
            }
        }

        #endregion

    }
}
