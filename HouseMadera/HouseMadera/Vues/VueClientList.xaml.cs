using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using HouseMadera.DAL;
using System.Windows.Controls;
using HouseMadera.VueModele;
using HouseMadera.Utilites;

namespace HouseMadera.Vues
{
    /// <summary>
    /// Logique d'interaction pour Client.xaml
    /// </summary>
    public partial class VueClientList : MetroWindow
    {
        public VueClientList()
        {
            VueModeleClientList vm = new VueModeleClientList();
            DataContext = vm; 
            InitializeComponent();
        }
    }
}
