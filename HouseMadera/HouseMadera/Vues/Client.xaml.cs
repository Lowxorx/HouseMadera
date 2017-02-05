using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using HouseMadera.DAL;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Drawing;
using System;
using HouseMadera.Vue_Modele;

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
            this.DataContext = new ClientViewModel();
          
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
    
        private void textBox_Nom_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TODO  à factoriser 
            //Le nom ne doit pas comporter de chiffre
            var match = Regex.Match(textBox_Nom.Text, @"\d+");
            if (match.Success)
                //TODO afficher l'erreur
                Console.WriteLine("Le nom ne doit pas contenir de chiffre");
            else
                Console.WriteLine("OK");
        }

        private void textBox_Prenom_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Le prenom ne doit pas comporter de chiffre
            var match = Regex.Match(textBox_Nom.Text, @"\d+");
            if (match.Success)
                //TODO afficher l'erreur
                Console.WriteLine("Le prenom ne doit pas contenir de chiffre");
            else
                Console.WriteLine("OK");
        }

        private void textBox_Voie_TextChanged(object sender, TextChangedEventArgs e)
        {
            //La voie
            var match = Regex.Match(textBox_Nom.Text, @"^\d*\s?(bis|ter)?$");
            if (match.Success)
                //TODO afficher l'erreur
                Console.WriteLine("Voie OK");
            else
                Console.WriteLine("Le format de la voie est oncorrect");
        }


    }
}
