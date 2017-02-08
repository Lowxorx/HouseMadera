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
using System.ComponentModel;
using System.Windows.Data;

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

            using (var dal = new ProjetDAL(DAL.DAL.Bdd))
            {
                ListeProjets = new ObservableCollection<Projet>(dal.ChargerProjets());
            }
        }

        private Projet selectedProjet;
        public Projet SelectedProjet
        {
            get { return selectedProjet; }
            set
            {
                selectedProjet = value;
            }
        }

        private ObservableCollection<Projet> listeProjets;
        public ObservableCollection<Projet> ListeProjets
        {
            get
            {
                return listeProjets;
            }
            set
            {
                listeProjets = value;
                RaisePropertyChanged("ListeProjets");
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

        private void CreationProjet()
        {

        }

        private async void RepriseProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();

            if (selectedProjet != null)
            {
                VueDetailsProjet vdp = new VueDetailsProjet();
                ((VueModeleDetailsProjet)vdp.DataContext).VuePrecedente = window;
                ((VueModeleDetailsProjet)vdp.DataContext).SelectedProjet = selectedProjet;
                vdp.Show();
                window.Hide();
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
            //using (var dal = new ProjetDAL(DAL.DAL.Bdd))
            //{
            //    listeProjets = dal.ChargerProjets();
            //    RaisePropertyChanged(() => ListeProjets);
            //}
            using (var dal = new CommercialDAL(DAL.DAL.Bdd))
            {
                listCommerciaux = dal.ChargerCommerciaux();
                RaisePropertyChanged(() => ListCommerciaux);
            }
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