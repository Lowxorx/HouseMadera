using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HouseMadera.Modèles;
using HouseMadera.Vues;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HouseMadera.Vue___Modèle
{
    public class VueModeleLogin : ViewModelBase
    {

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

        private void ConnexionExec()
        {
            if (pwCommercial != null)
            {
                Console.WriteLine(pwCommercial);
                Console.WriteLine(loginCommercial);
                var newCommercial = new Commercial{ NomUtilisateur = LoginCommercial, Password = PwCommercial };
                CommercialConnect c = new CommercialConnect();
                bool connected = c.Connect(newCommercial);
                Console.WriteLine(connected);
            }
            else
            {
                Console.WriteLine("Pw conteneur est null");
            }
        }
    }
}
