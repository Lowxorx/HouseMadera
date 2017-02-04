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
            //TODO modifier "SQLITE" par Bdd
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
            //TODO modifier "SQLITE" par Bdd
            using (var dal = new ClientDAL("SQLITE"))
            {
                clients = dal.GetFilteredClient(comboBox_recherche.SelectionBoxItem.ToString(), textBox_recherche.Text);
            }
            dataGrid.ItemsSource = clients;
        }

        private void AfficherSignUp(object sender, RoutedEventArgs e)
        {
            flyout_SignUp.IsOpen = true;

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
