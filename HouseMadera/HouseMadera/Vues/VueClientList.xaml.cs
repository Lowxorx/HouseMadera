﻿using MahApps.Metro.Controls;
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

        //  public Dictionary<int,string> Localite { get; set; }
      

        public RegexUtilities regle { get; set; }
        public VueClientList()
        {
            InitializeComponent();
        }



        //private void AfficherClients(object sender, RoutedEventArgs e)
        //{

        //    vm.GetAllClients();
        //    dataGrid.ItemsSource = vm.Clients;

        //}

        //private void RechercherClient(object sender, TextChangedEventArgs e)
        //{
        //    vm.GetFilteredClients(comboBox_recherche.SelectionBoxItem.ToString(), textBox_recherche.Text);
        //    dataGrid.ItemsSource = vm.Clients;
        //}

        //private void AfficherSignUp(object sender, RoutedEventArgs e)
        //{
        //    //flyout_SignUp.IsOpen = true;

        //}
    
      

       
    }
}