using HouseMadera.DAL;
using HouseMadera.Vue_Modele;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HouseMadera.Vues
{
    /// <summary>
    /// Logique d'interaction pour ClientEdit.xaml
    /// </summary>
    public partial class ClientEdit : MetroWindow
    {

        public ClientViewModel vm { get; set; }

        public ClientEdit()
        {
            InitializeComponent();
            vm = new ClientViewModel();
            DataContext = vm;
        }

        //private void RechercherCommune(object sender, TextChangedEventArgs e)
        //{
        //    int i;
        //    var isCodePostal = int.TryParse(textBox_CodePostal.Text.ToString(), out i);


        //    var communes = new List<Modeles.Commune>();
        //    //TODO modifier "SQLITE" par Bdd
        //    if (textBox_CodePostal.Text != string.Empty && isCodePostal)
        //    {
        //        using (var dal = new CommuneDAL("SQLITE"))
        //        {
        //            communes = dal.GetFilteredCommunes(Convert.ToInt32(textBox_CodePostal.Text));
        //        }
        //    }

        //    if (communes.Count > 0)
        //    {
        //        var communesSuggestionList = new List<string>();

        //        foreach (var commune in communes)
        //        {
        //            var suggestion = commune.ToString();
        //            communesSuggestionList.Add(suggestion);

        //        }
        //        ListBox_Suggestion.ItemsSource = communesSuggestionList;
        //        ListBox_Suggestion.Visibility = Visibility.Visible;
        //    }
        //    else if (string.IsNullOrEmpty(textBox_CodePostal.Text))
        //    {
        //        ListBox_Suggestion.ItemsSource = null;
        //        ListBox_Suggestion.Visibility = Visibility.Collapsed;
        //    }
        //}

        //private void RemplirLocalite(object sender, SelectionChangedEventArgs e)
        //{
        //    if (ListBox_Suggestion.ItemsSource != null)
        //    {
        //        ListBox_Suggestion.Visibility = Visibility.Collapsed;
        //        if (ListBox_Suggestion.SelectedItem != null)
        //        {
        //            var communeSelected = ListBox_Suggestion.SelectedItem.ToString();
        //            var vm = new CommuneViewModel(communeSelected);
        //            textBox_Localite.Text = string.IsNullOrEmpty(vm.NomCommune) ? string.Empty : vm.NomCommune;
        //            textBox_CodePostal.Text = string.IsNullOrEmpty(vm.CodePostal) ? string.Empty : vm.CodePostal;
        //            ListBox_Suggestion.Visibility = Visibility.Collapsed;
        //        }
        //    }
        //    else
        //        textBox_Localite.Text = string.Empty;
        //}
    }
}
