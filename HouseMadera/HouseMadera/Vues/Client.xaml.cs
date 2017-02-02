using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using HouseMadera.DAL;
using System.Windows.Controls;

namespace HouseMadera.Vues
{
    /// <summary>
    /// Logique d'interaction pour Client.xaml
    /// </summary>
    public partial class Client : MetroWindow
    {
        public Client()
        {
            InitializeComponent();
        }

        private void AfficherClients(object sender, RoutedEventArgs e)
        {
            var clients = new List<Modeles.Client>();
            using (var dal = new ClientDAL("SQLITE"))
            {
                clients = dal.GetAllClients();
            }
            dataGrid.ItemsSource = clients;

        }

        private void RechercherClient(object sender, TextChangedEventArgs e)
        {
            var clients = new List<Modeles.Client>();

            using (var dal = new ClientDAL("SQLITE"))
            {
                clients = dal.GetFilteredClient(comboBox_recherche.SelectionBoxItem.ToString(), textBox_recherche.Text);
            }
            dataGrid.ItemsSource = clients;
        }

       
    }
}
