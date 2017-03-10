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

            // Check de la connexion
            ConnectivityMonitor testConnection = new ConnectivityMonitor();
            bool isOnline = testConnection.IsOnline();
            string bdd = isOnline ? "MYSQL" : "SQLITE";
            DAL.DAL.Bdd = bdd;

        }
    }
}
