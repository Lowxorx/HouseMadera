using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HouseMadera.Modèles;
using HouseMadera.Vues;
using System;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Ioc;
using System.Windows;
using MahApps.Metro.Controls;
using System.Linq;

namespace HouseMadera.Vue___Modèle
{
    public class VueModeleLogin : ViewModelBase
    {

        [PreferredConstructor]
        public VueModeleLogin()
        {
            Connexion = new RelayCommand(ConnexionExec);
        }

        private string loginCommercial;
        public string LoginCommercial
        {
            get { return loginCommercial; }
            set
            {
                if (!string.Equals(loginCommercial, value))
                {
                    loginCommercial = value;
                    RaisePropertyChanged(() => LoginCommercial);
                }
            }
        }
        private string pwCommercial;
        public string PwCommercial
        {
            get { return pwCommercial; }
            set
            {
                if (!string.Equals(pwCommercial, value))
                {
                    pwCommercial = value;
                    RaisePropertyChanged(() => PwCommercial);
                }
            }
        }

        public ICommand Connexion { get; private set; }

        private async void ConnexionExec()
        {
            if (pwCommercial != null && loginCommercial != null)
            {
                Console.WriteLine(pwCommercial);
                Console.WriteLine(loginCommercial);
                var newCommercial = new Commercial{ NomUtilisateur = LoginCommercial, Password = PwCommercial };
                CommercialConnect c = new CommercialConnect();
                bool connected = c.Connect(newCommercial);
                if (connected)
                {
                    Console.WriteLine(connected);
                }
                else
                {
                    var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
                    if (window != null)
                        await window.ShowMessageAsync("Erreur", "Impossible de se connecter à la base de données.");
                }

            }
            else
            {
                var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
                if (window != null)
                    await window.ShowMessageAsync("Avertissement", "Merci de saisir vos identifiants.");
                Console.WriteLine("Pw conteneur est null");
            }
        }

    }
}
