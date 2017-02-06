using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modeles;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using HouseMadera.DAL;
using System.Windows.Input;

namespace HouseMadera.VueModele
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

        private Projet selectProjet;
        public Projet SelectProjet
        {
            get { return selectProjet; }
            set
            {
                selectProjet = value;
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

        private ObservableCollection<Modeles.Commercial> listCommerciaux;
        public ObservableCollection<Modeles.Commercial> ListCommerciaux
        {
            get
            {
                return listCommerciaux;
            }
        }

        private void CreationProjet()
        {

        }

        private async void RepriseProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();

            if (selectProjet != null)
            {
                VueDetailsProjet vdp = new VueDetailsProjet();
                ((VueModeleDetailsProjet)vdp.DataContext).TitreProjet = @"Détails du projet " + selectProjet.Nom;
                vdp.Show();
                window.Close();
            }
            else
            {
                if (window != null)
                {
                    await window.ShowMessageAsync("Avertissement", "Merci de sélectionner un projet.");
                }
            }
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
            listProjets = ProjetDAL.ChargerProjets();
            RaisePropertyChanged(() => ListProjets);
            listCommerciaux = CommercialDAL.ChargerCommerciaux();
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