
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

namespace HouseMadera.VueModele
{
    public class VueModeleClientList : ViewModelBase
    {


        #region PROPRIETES
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
        public ICommand EditClient { get; private set; }
        public ICommand Deconnexion { get; private set; }
        public List<string> filtres { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        
        /// <summary>
        /// Client Selectionne lié à une ligne de la datagrid
        /// </summary>
        private Client clientSelectionne;
        public Client ClientSelectionne
        {
            get { return clientSelectionne; }
            set
            {
               clientSelectionne = value;
            }
        }

        public RegexUtilities reg { get; set; }
        #endregion

        public VueModeleClientList()
        {
            EditClient = new RelayCommand(EClient);
            Deconnexion = new RelayCommand(Deconnecter);
            reg = new RegexUtilities();
            Clients = new ObservableCollection<Client>(AfficherClient());
            ClientSelectionne = new Client();
            filtres = new List<string>() { "Nom", "Adresse" };

        }


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

        #region METHODES
        /// <summary>
        /// Ferme la fenetre courante et affiche la nouvelle fenêtre
        /// </summary>
        private void EClient()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueClientEdit vce = new VueClientEdit(clientSelectionne);
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
        #endregion

    }
}
