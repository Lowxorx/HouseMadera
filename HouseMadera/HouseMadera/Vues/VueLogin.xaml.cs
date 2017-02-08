using HouseMadera.Utilites;
using MahApps.Metro.Controls;
using System;

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
            var testConnection = new ConnectivityMonitor();
            var isOnline = testConnection.IsOnline();
            var bdd = isOnline ? "MYSQL" : "SQLITE";
            Console.WriteLine("Etat de la connexion\n En ligne ? : {0}\n Bdd choisie :{1}", isOnline, bdd);
            DAL.DAL.Bdd = bdd;

        }
    }
}
