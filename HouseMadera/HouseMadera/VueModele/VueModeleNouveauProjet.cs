using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleNouveauProjet : ViewModelBase, IDataErrorInfo
    {
        [PreferredConstructor]
        public VueModeleNouveauProjet()
        {
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Retour = new RelayCommand(RetourArriere);
            ValiderProjet = new RelayCommand(VerifierEtValiderProjet);
            SelectionnerClient = new RelayCommand(ChoisirOuCreerClient);
            Deconnexion = new RelayCommand(Logout);

            // Actions à effectuer au chargement de la vue :
            ProjetDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            IsFormulaireOk = false;

        }

        public ICommand ValiderProjet { get; private set; }
        public ICommand SelectionnerClient { get; private set; }
        public ICommand WindowLoaded { get; private set; }
        public ICommand Retour { get; private set; }
        public ICommand Deconnexion { get; private set; }

        private Commercial commercialConnecte;
        public Commercial CommercialConnecte
        {
            get { return commercialConnecte; }
            set { commercialConnecte = value; }
        }

        private string clientSelect;

        public string ClientSelect
        {
            get { return clientSelect; }
            set
            {
                clientSelect = value;
                RaisePropertyChanged(() => ClientSelect);
            }
        }

        private Client clientActuel;
        public Client ClientActuel
        {
            get { return clientActuel; }
            set { clientActuel = value; }
        }

        private async void RetourArriere()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment fermer ce projet ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    AnimateHide = false,
                    AnimateShow = true
                });

                if (result == MessageDialogResult.Affirmative)
                {
                    VueChoixProjet vcp = new VueChoixProjet();
                    ((VueModeleChoixProjet)vcp.DataContext).CommercialConnecte = commercialConnecte;
                    window.Close();
                    vcp.Show();
                }
            }
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
        }

        private async void Logout()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment vous déconnecter ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    AnimateHide = false,
                    AnimateShow = true
                });

                if (result == MessageDialogResult.Affirmative)
                {
                    VueLogin vl = new VueLogin();
                    window.Close();
                    vl.Show();

                }
            }
        }

        private void VerifierEtValiderProjet()
        {
            IsFormulaireOk = VerifierTouslesChamps();
            if (IsFormulaireOk)
            {

            }

        }

        private void ChoisirOuCreerClient()
        {
            VueClientList vcl = new VueClientList();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns> 'true' si tous les champs respectent les conditions sinon 'false' </returns>
        private bool VerifierTouslesChamps()
        {
            if (isProjetRefValid && isProjetNomValid)
                return true;
            else
                return false;
        }

        private bool isFormulaireOk;
        public bool IsFormulaireOk
        {
            get { return isFormulaireOk; }
            set
            {
                isFormulaireOk = value;
                RaisePropertyChanged(() => IsFormulaireOk);
            }
        }

        private string projetDate;
        public string ProjetDate
        {
            get { return projetDate; }
            set
            {
                projetDate = value;
                RaisePropertyChanged(() => ProjetDate);
            }
        }

        private bool isProjetRefValid;
        private string projetRef;
        public string ProjetRef
        {
            get { return projetRef; }
            set { projetRef = value; }
        }

        private bool isProjetNomValid;
        private string projetNom;
        public string ProjetNom
        {
            get { return projetNom; }
            set { projetNom = value; }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                try
                {
                    string result = string.Empty;
                    switch (columnName)
                    {
                        case "ProjetNom":
                            if (ProjetNom == null || ProjetNom == string.Empty || ProjetNom.Length == 0)
                            {
                                result = "Merci de renseigner le nom du projet.";
                                isProjetNomValid = false;
                            }
                            else
                            {
                                isProjetNomValid = true;
                            }
                            break;
                        case "ProjetRef":
                            if (ProjetRef == null || ProjetRef == string.Empty || ProjetRef.Length == 0)
                            {
                                result = "Merci de renseigner une référence de projet.";
                                isProjetRefValid = false;
                            }
                            else
                            {
                                isProjetRefValid = true;
                            }
                            break;
                    }
                    IsFormulaireOk = VerifierTouslesChamps();
                    return result;
                }
                catch (Exception ex)
                {
                    // Ex ignorée au lancement de la vue
                    Logger.WriteEx(ex);
                    return string.Empty;
                }
            }
        }
    }
}
