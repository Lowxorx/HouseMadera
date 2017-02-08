using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.DAL;
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
    public class VueModeleLogin : ViewModelBase
    {
        public ICommand Connexion { get; private set; }
        public ICommand Quitter { get; private set; }

        [PreferredConstructor]
        public VueModeleLogin()
        {
            Connexion = new RelayCommand(ConnexionExec);
            Quitter = new RelayCommand(Exit);
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

        private async void Exit()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment quitter ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
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
        private async void ConnexionExec()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();

            if (pwCommercial != null && loginCommercial != null)
            {

                var newCommercial = new Commercial{ Login = LoginCommercial, Password = PwCommercial };
                using (var dal = new CommercialDAL(DAL.DAL.Bdd))
                {
                    string loginstatus = dal.Connect(newCommercial);
                    Console.WriteLine("Code retour login : " + loginstatus);
                    if (loginstatus == "0")
                    {
                        VueChoixAdmin vcp = new VueChoixAdmin();
                        vcp.Show();
                        window.Close();
                    }
                    else if (loginstatus == "1")
                    {
                        if (window != null)
                            await window.ShowMessageAsync("Erreur", "Nom d'utilisateur ou mot de passe incorrect");
                    }
                    else if (loginstatus == "2")
                    {
                        if (window != null)
                            await window.ShowMessageAsync("Erreur", "Impossible de se connecter à la base de données");
                    }
                }
            }
            else
            {
                if (window != null)
                    await window.ShowMessageAsync("Avertissement", "Merci de saisir vos identifiants.");
            }
        }

    }
}
