
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Reflection;

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
        public ICommand Retour { get; private set; }

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

        private Commercial commercialConnecte;
        public Commercial CommercialConnecte
        {
            get { return commercialConnecte; }
            set
            {
                commercialConnecte = value;
            }
        }

        private MetroWindow vuePrecedente;
        public MetroWindow VuePrecedente
        {
            get { return vuePrecedente; }
            set { vuePrecedente = value; }
        }
        #endregion

        public VueModeleClientList()
        {
            EditClient = new RelayCommand(EClient);
            Deconnexion = new RelayCommand(Deconnecter);
            ModifClient = new RelayCommand(ModifierClient);
            Retour = new RelayCommand(RetourAccueil);
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
            Filtre = Filtres[0];
        }

        #region METHODES
        /// <summary>
        /// Ferme la fenetre courante et affiche la fenêtre accueil
        /// </summary>
        private void RetourAccueil()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            if (window != null)
            {
                //    var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment fermer l'édition de client ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                //    {
                //        AffirmativeButtonText = "Oui",
                //        NegativeButtonText = "Non",
                //        AnimateHide = false,
                //        AnimateShow = true
                //    });

                //    if (result == MessageDialogResult.Affirmative)
                //    {

                VueChoixAdmin vca = new VueChoixAdmin();
                    ((VueModeleChoixAdmin)vca.DataContext).CommercialConnecte = CommercialConnecte;
                    vca.Show();
                    window.Close();
            }
            //}
        }

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
            vm.CommercialConnecte = CommercialConnecte;
            vm.VuePrecedente = window;
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
            ((VueModeleClientEdit)vce.DataContext).CommercialConnecte = CommercialConnecte;
            ((VueModeleClientEdit)vce.DataContext).VuePrecedente = window;
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
            using (var dal = new ClientDAL(DAL.DAL.Bdd))
            {
                clients = dal.GetAllModeles();
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
           return Clients.Where(c => SelectionnerProprieteClient(c, filtre).Contains(valeur.ToLower())).Select(c=> c).ToList();
        }
        /// <summary>
        /// Trouve la propriété du modele correspondant au filtre et retourne la valeur de cette propriété 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="filtre"></param>
        /// <returns>Une chaine caractère représentant la valeur de la propriété</returns>
        private string SelectionnerProprieteClient(Client c,string filtre)
        {
            if (c == null)
                return string.Empty;
            return c.GetType().GetRuntimeProperty(filtre).GetValue(c).ToString().ToLower();
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
