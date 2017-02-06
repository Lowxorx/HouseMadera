using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleDetailsProjet : ViewModelBase
    {

        public ICommand WindowLoaded { get; private set; }
        public ICommand Deconnexion { get; private set; }

        private string titreProjet;
        public string TitreProjet
        {
            get { return titreProjet; }
            set
            {
                titreProjet = value;
                RaisePropertyChanged(() => TitreProjet);
            }
        }

        [PreferredConstructor]
        public VueModeleDetailsProjet()
        {
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Deconnexion = new RelayCommand(Logout);
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
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