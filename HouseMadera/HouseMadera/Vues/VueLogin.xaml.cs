using HouseMadera.Modèles;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HouseMadera.Vues
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class VueLogin : MetroWindow
    {
        public VueLogin()
        {
            InitializeComponent();
        }

        private void btConnexion_Click(object sender, RoutedEventArgs e)
        {
            var test = new CheckCred();
            var com = new Commercial();
            com.NomUtilisateur = tbUsername.Text;
            com.Password = tbPassword.Password;
            test.VerifLogin(com);
        }
    }
}
