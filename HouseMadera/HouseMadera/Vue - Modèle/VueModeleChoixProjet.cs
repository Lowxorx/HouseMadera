using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modèles;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.Vue___Modèle
{
    public class VueModeleChoixProjet : ViewModelBase
    {
        [PreferredConstructor]
        public VueModeleChoixProjet()
        {
            NouveauProjet = new RelayCommand(CreationProjet);
            ReprendreProjet = new RelayCommand(RepriseProjet);
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
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

        public ICommand NouveauProjet { get; private set; }
        public ICommand ReprendreProjet { get; private set; }
        public ICommand WindowLoaded { get; set; }

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
            var listeProjet = new Projets();
            listeProjet.ChargerProjets();

            //var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            //if (window != null)
            //    await window.ShowMessageAsync("Avertissement", "Merci de saisir vos identifiants.");
            //Console.WriteLine("Pw conteneur est null");
        }
    }
}