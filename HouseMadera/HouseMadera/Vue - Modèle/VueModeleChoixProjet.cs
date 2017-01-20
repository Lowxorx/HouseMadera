using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modèles;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.Vue___Modèle
{
    public class VueModeleChoixProjet : ViewModelBase
    {

        public ICommand NouveauProjet { get; private set; }
        public ICommand ReprendreProjet { get; private set; }
        public ICommand WindowLoaded { get; private set; }
        public ICommand Deconnexion { get; private set; }

        [PreferredConstructor]
        public VueModeleChoixProjet()
        {
            NouveauProjet = new RelayCommand(CreationProjet);
            ReprendreProjet = new RelayCommand(RepriseProjet);
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Deconnexion = new RelayCommand(Logout);
        }

        private string selectProjet;
        public string SelectProjet
        {
            get { return selectProjet; }
            set
            {
                if (selectProjet != null)
                {

                }
            }
        }

        private ObservableCollection<Projet> listProjets;
        public ObservableCollection<Projet> ListProjets
        {
            get
            {
                return listProjets;
            }
        }

        private ObservableCollection<Commercial> listCommerciaux;
        public ObservableCollection<Commercial> ListCommerciaux
        {
            get
            {
                return listCommerciaux;
            }
        }

        private async void CreationProjet()
        {

        }

        private async void RepriseProjet()
        {
            if (selectProjet != null)
            {

            }
            else
            {
                var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
                if (window != null)
                    await window.ShowMessageAsync("Avertissement", "Merci de sélectionner un projet.");
            }
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
            listProjets = Projets.ChargerProjets();
            RaisePropertyChanged(() => ListProjets);
            listCommerciaux = Commerciaux.ChargerCommerciaux();
            RaisePropertyChanged(() => ListCommerciaux);
        }

        private async void Logout()
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
                    window.Close();
                    vl.Show();

                }
            }
        }
    }
}