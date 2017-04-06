using HouseMadera.Utilities;
using MahApps.Metro.Controls;

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
            DAL.DAL.Bdd = "SQLITE";

        }
    }
}
