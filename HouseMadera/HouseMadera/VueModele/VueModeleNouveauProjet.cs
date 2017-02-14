using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modeles;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleNouveauProjet : ViewModelBase
    {
        [PreferredConstructor]
        public VueModeleNouveauProjet()
        {
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Retour = new RelayCommand(RetourArriere);
            ValiderProjet = new RelayCommand(VerifierEtValiderProjet);
            Deconnexion = new RelayCommand(Logout);

            // Actions à effectuer au chargement de la vue :

        }

        public ICommand ValiderProjet { get; private set; }
        public ICommand WindowLoaded { get; private set; }
        public ICommand Retour { get; private set; }
        public ICommand Deconnexion { get; private set; }

        private Commercial commercialConnecte;
        public Commercial CommercialConnecte
        {
            get { return commercialConnecte; }
            set { commercialConnecte = value; }
        }

        private async void RetourArriere()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment fermer ce projet ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    AnimateHide = false,
                    AnimateShow = true
                });

                if (result == MessageDialogResult.Affirmative)
                {
                    VueChoixProjet vcp = new VueChoixProjet();
                    ((VueModeleChoixProjet)vcp.DataContext).CommercialConnecte = commercialConnecte;
                    window.Close();
                    vcp.Show();
                }
            }
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
        }

        private async void Logout()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
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

        private void VerifierEtValiderProjet()
        {

        }

        private string projetDate;

        public string ProjetDate
        {
            get { return projetDate; }
            set { projetDate = value; }
        }

        private string projetRef;

        public string ProjetRef
        {
            get { return projetRef; }
            set { projetRef = value; }
        }

        private string projetNom;

        public string ProjetNom
        {
            get { return projetNom; }
            set { projetNom = value; }
        }

        private string clientNom;

        public string ClientNom
        {
            get { return clientNom; }
            set { clientNom = value; }
        }

        private string clientPrenom;

        public string ClientPrenom
        {
            get { return clientPrenom; }
            set { clientPrenom = value; }
        }

        private string clientFixe;

        public string ClientFixe
        {
            get { return clientFixe; }
            set { clientFixe = value; }
        }

        private string clientMobile;

        public string ClientMobile
        {
            get { return clientMobile; }
            set { clientMobile = value; }
        }

        private string clientEmail;

        public string ClientEmail
        {
            get { return clientEmail; }
            set { clientEmail = value; }
        }

        private string clientAdr1;

        public string ClientAdr1
        {
            get { return clientAdr1; }
            set { clientAdr1 = value; }
        }

        private string clientAdr2;

        public string ClientAdr2
        {
            get { return clientAdr2; }
            set { clientAdr2 = value; }
        }

        private string clientAdr3;

        public string ClientAdr3
        {
            get { return clientAdr3; }
            set { clientAdr3 = value; }
        }

        private string clientCP;

        public string ClientCP
        {
            get { return clientCP; }
            set { clientCP = value; }
        }

        private string clientVille;

        public string ClientVille
        {
            get { return clientVille; }
            set { clientVille = value; }
        }

    }
}
