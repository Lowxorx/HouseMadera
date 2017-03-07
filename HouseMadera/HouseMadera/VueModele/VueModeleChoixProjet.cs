using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleChoixProjet : ViewModelBase
    {
        [PreferredConstructor]
        public VueModeleChoixProjet()
        {
            NouveauProjet = new RelayCommand(CreationProjet);
            ReprendreProjet = new RelayCommand(RepriseProjet);
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Retour = new RelayCommand(RetourAdminProjet);
            Deconnexion = new RelayCommand(Logout);

            // Actions à effectuer au chargement de la vue :
            ChargerProjet();
            ChargerCommerciaux();
        }

        public ICommand NouveauProjet { get; private set; }
        public ICommand ReprendreProjet { get; private set; }
        public ICommand WindowLoaded { get; private set; }
        public ICommand Retour { get; private set; }
        public ICommand Deconnexion { get; private set; }

        private Projet selectedProjet;
        public Projet SelectedProjet
        {
            get { return selectedProjet; }
            set
            {
                selectedProjet = value;
            }
        }

        private string commercialCoLabel;
        public string CommercialCoLabel
        {
            get { return commercialCoLabel; }
            set { commercialCoLabel = value; }
        }

        private string filtreProjet;

        public string FiltreProjet
        {
            get { return filtreProjet; }
            set
            {
                filtreProjet = value;
                RaisePropertyChanged("FiltreProjet");
                FiltrerProjetsParFiltrePerso();
            }
        }


        private Commercial commercialSelectionne;
        public Commercial CommercialSelectionne
        {
            get { return commercialSelectionne; }
            set
            {
                commercialSelectionne = value;
                FiltrerProjetsParCommerciaux();
                RaisePropertyChanged("CommercialSelectionne");
            }
        }

        private Commercial commercialConnecte;
        public Commercial CommercialConnecte
        {
            get { return commercialConnecte; }
            set { commercialConnecte = value; }
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
                if (listeProjets == value)
                    return;
                listeProjets = value;
                RaisePropertyChanged("ListeProjets");
            }
        }

        private ObservableCollection<Projet> listeProjetsFiltre = new ObservableCollection<Projet>();
        public ObservableCollection<Projet> ListeProjetsFiltre
        {
            get
            {
                return listeProjetsFiltre;
            }
            set
            {
                if (listeProjetsFiltre == value)
                    return;
                listeProjetsFiltre = value;
                RaisePropertyChanged("ListeProjetsFiltre");
            }
        }

        private ObservableCollection<Commercial> listCommerciaux;
        public ObservableCollection<Commercial> ListCommerciaux
        {
            get
            {
                return listCommerciaux;
            }
            set
            {
                if (listCommerciaux == value)
                    return;
                listCommerciaux = value;
                RaisePropertyChanged("ListCommerciaux");
            }
        }

        private void CreationProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().First();
            VueNouveauProjet vnp = new VueNouveauProjet();
            ((VueModeleNouveauProjet)vnp.DataContext).CommercialConnecte = CommercialConnecte;
            vnp.Show();
            window.Close();
        }

        private async void RepriseProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().First();

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

        private void ChargerProjet()
        {
            using (var dal = new ProjetDAL(DAL.DAL.Bdd))
            {
                ListeProjets = new ObservableCollection<Projet>(dal.ChargerProjets());
                RaisePropertyChanged(() => ListeProjets);
            }
        }

        private void ChargerCommerciaux()
        {
            using (CommercialDAL dal = new CommercialDAL(DAL.DAL.Bdd))
            {
                ListCommerciaux = new ObservableCollection<Commercial>(dal.ChargerCommerciaux());
                RaisePropertyChanged(() => ListCommerciaux);
            }
        }

        private void ChargerDetailsCommercialConnecte()
        {
            using (CommercialDAL dal = new CommercialDAL(DAL.DAL.Bdd))
            {
                CommercialConnecte = dal.GetCommercial(CommercialConnecte.Login);
                CommercialCoLabel = String.Format("Connecté en tant que {0} {1}", CommercialConnecte.Prenom, CommercialConnecte.Nom);
                RaisePropertyChanged(() => CommercialConnecte);
                RaisePropertyChanged(() => CommercialCoLabel);
            }
        }

        private async void RetourAdminProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            if (window != null)
            {
                MessageDialogResult result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment fermer ce projet ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
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

        private void FiltrerProjetsParCommerciaux()
        {
            ListeProjetsFiltre.Clear();
            int filtre = CommercialSelectionne.Id;
            foreach (Projet p in ListeProjets)
            {
                if (p.Commercial.Id == filtre)
                {
                    ListeProjetsFiltre.Add(p);
                }
            }
            RaisePropertyChanged(() => ListeProjetsFiltre);
        }

        private void FiltrerProjetsParFiltrePerso()
        {
            ObservableCollection<Projet> listeProjetTmp = new ObservableCollection<Projet>();
            string filtre = FiltreProjet;
            if (filtre.Length == 0)
            {
                FiltrerProjetsParCommerciaux();
            }
            else
            {
                FiltrerProjetsParCommerciaux();
                foreach (Projet p in ListeProjetsFiltre)
                {
                    if (p.Nom.Contains(filtre) || p.Client.Nom.Contains(filtre) || p.Client.Prenom.Contains(filtre))
                    {
                        listeProjetTmp.Add(p);
                    }
                }
                ListeProjetsFiltre = listeProjetTmp;
                RaisePropertyChanged(() => ListeProjetsFiltre);
            }
        }

        private void SelectionnerCommercialDefaut()
        {
            foreach(Commercial c in ListCommerciaux)
            {
                if (c.Login == CommercialConnecte.Login)
                {
                    CommercialSelectionne = c;
                }
            }
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
            ChargerDetailsCommercialConnecte();
            SelectionnerCommercialDefaut();
        }

        private async void Logout()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().First();
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
