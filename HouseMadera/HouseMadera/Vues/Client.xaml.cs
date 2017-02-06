using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using HouseMadera.DAL;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Drawing;
using System;
using HouseMadera.Vue_Modele;
using HouseMadera.Utilites;

namespace HouseMadera.Vues
{
    /// <summary>
    /// Logique d'interaction pour Client.xaml
    /// </summary>
    public partial class Client : MetroWindow
    {

        //  public Dictionary<int,string> Localite { get; set; }

        public RegexUtilities regle { get; set; }
        public Client()
        {
            InitializeComponent();
            this.DataContext = new ClientViewModel();
            // Localite = new Dictionary<int, string>();
            regle = new RegexUtilities();
          
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

        private void textBox_Numero_TextChanged(object sender, TextChangedEventArgs e)
        {
            //La voie
            var match = Regex.Match(textBox_Numero.Text, @"^\d*\s?(bis|ter)?$");
            if (match.Success)
                //TODO afficher l'erreur
                Console.WriteLine("Voie OK");
            else
                Console.WriteLine("Le format de la voie est incorrect");
        }


        private void textbox_Mobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isOk = regle.IsValidTelephoneNumber(textBox_Mobile.Text);
            if (isOk)
                Console.WriteLine("Mobile OK");
            else
                Console.WriteLine("Format incorect du numero de mobile");

        }

        private void textbox_Telephone_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isOk = regle.IsValidTelephoneNumber(textBox_Telephone.Text);
            if (isOk)
                Console.WriteLine("Telephone OK");
            else
                Console.WriteLine("Format incorect du numero de telephone");

        }

        private void textbox_Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isOk = regle.IsValidEmail(textBox_Email.Text);
            if (isOk)
                Console.WriteLine("Email OK");
            else
                Console.WriteLine("Format incorect de l'e-mail");

        }

        private void RechercherCommune(object sender, TextChangedEventArgs e)
        {
            int i;
            var isCodePostal = int.TryParse(textBox_CodePostal.Text.ToString(), out i);


            var communes = new List<Modeles.Commune>();
            //TODO modifier "SQLITE" par Bdd
            if(textBox_CodePostal.Text != string.Empty && isCodePostal)
            {
                using (var dal = new CommuneDAL("SQLITE"))
                {
                    communes = dal.GetFilteredCommunes(Convert.ToInt32(textBox_CodePostal.Text));
                }
            }
           
            if(communes.Count>0)
            {
               var communesSuggestionList = new List<string>();
               
                foreach(var commune in communes)
                {
                    var suggestion = commune.ToString();
                    communesSuggestionList.Add(suggestion);
                    
                }
                ListBox_Suggestion.ItemsSource = communesSuggestionList;
                ListBox_Suggestion.Visibility = Visibility.Visible;
            }
            else if (string.IsNullOrEmpty(textBox_CodePostal.Text))
            {
                ListBox_Suggestion.ItemsSource = null;
                ListBox_Suggestion.Visibility = Visibility.Collapsed;
            }
        }

       private void RemplirLocalite(object sender, SelectionChangedEventArgs e)
        {
            if(ListBox_Suggestion.ItemsSource != null)
            {
                ListBox_Suggestion.Visibility = Visibility.Collapsed;
                if (ListBox_Suggestion.SelectedItem != null)
                {
                    var communeSelected = ListBox_Suggestion.SelectedItem.ToString();
                    var vm = new CommuneViewModel(communeSelected);
                    textBox_Localite.Text = string.IsNullOrEmpty(vm.NomCommune) ? string.Empty : vm.NomCommune;
                    textBox_CodePostal.Text = string.IsNullOrEmpty(vm.CodePostal) ? string.Empty : vm.CodePostal;
                    ListBox_Suggestion.Visibility = Visibility.Collapsed;
                }
            }
            else
                textBox_Localite.Text = string.Empty;
        }

       
    }
}
