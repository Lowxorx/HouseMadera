using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modèles;
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
        private ObservableCollection<Projet> listProjets;

        [PreferredConstructor]
        public VueModeleChoixProjet()
        {
            NouveauProjet = new RelayCommand(CreationProjet);
            ReprendreProjet = new RelayCommand(RepriseProjet);
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Deconnexion = new RelayCommand(Logout);
        }

        public ICommand NouveauProjet { get; private set; }
        public ICommand ReprendreProjet { get; private set; }
        public ICommand WindowLoaded { get; private set; }
        
        public ICommand Deconnexion { get; private set; }


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

        public ObservableCollection<Projet> ListProjets
        {
            get
            {
                return listProjets;
            }
        }

        private async void CreationProjet()
        {
            if (selectProjet != null)
            {

            }
            else
            {
                var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
                if (window != null)
                    await window.ShowMessageAsync("Avertissement", "Merci de saisir vos identifiants.");
                Console.WriteLine("Pw conteneur est null");
            }
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
                    await window.ShowMessageAsync("Avertissement", "Merci de saisir vos identifiants.");
                Console.WriteLine("Pw conteneur est null");
            }
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
            listProjets = Projets.ChargerProjets();
            foreach (Projet p in listProjets)
            {
                Console.WriteLine(p.Nom);
            }
            RaisePropertyChanged(() => ListProjets);

            //var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            //if (window != null)
            //    await window.ShowMessageAsync("Avertissement", "Merci de saisir vos identifiants.");
            //Console.WriteLine("Pw conteneur est null");
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
                    window.Close();
                }
            }            
        }
    }
}