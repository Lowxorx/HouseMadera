using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
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
            SelectionnerClient = new RelayCommand(AjouterUnClient);
            Deconnexion = new RelayCommand(Deco);

            // Actions à effectuer au chargement de la vue :
            ProjetDate = DateTime.Now;
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

        private ObservableCollection<Client> listClient;
        public ObservableCollection<Client> ListClient
        {
            get
            {
                return listClient;
            }
            set
            {
                if (listClient == value)
                    return;
                listClient = value;
                RaisePropertyChanged("ListClient");
            }
        }

        private Client clientSelect;
        public Client ClientSelect
        {
            get { return clientSelect; }
            set
            {
                ClientNom = String.Format("{0} {1}", value.Prenom, value.Nom);
                clientSelect = value;
                RaisePropertyChanged(() => ClientSelect);
            }
        }

        private async void RetourArriere()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
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
                    vcp.Show();
                    window.Close();
                }
            }
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
            ChargerClients();
        }

        private void ChargerClients()
        {
            using (ClientDAL dal = new ClientDAL(DAL.DAL.Bdd))
            {
                ListClient = new ObservableCollection<Client>(dal.GetAllModeles());
                RaisePropertyChanged(() => ListClient);
            }
        }

        private async void Deco()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
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

        private async void VerifierEtValiderProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            IsFormulaireOk = VerifierTouslesChamps();
            if (IsFormulaireOk && ClientSelect != null)
            {
                Projet p = new Projet()
                {
                    Nom = ProjetNom,
                    Reference = ProjetRef,
                    CreateDate = Convert.ToDateTime(ProjetDate),
                    Client = ClientSelect,
                    Commercial = CommercialConnecte,
                    Creation = Convert.ToDateTime(ProjetDate),
                    MiseAJour = Convert.ToDateTime(ProjetDate)
                };
                int insertProjet = -2;
                try
                {
                    using (ProjetDAL dal = new ProjetDAL(DAL.DAL.Bdd))
                    {
                        insertProjet = dal.CreerProjet(p);
                        Console.WriteLine("résultat insert projet : " + insertProjet);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteEx(ex);
                    await window.ShowMessageAsync("Erreur", "Impossible d'insérer le projet en base");
                }

                if (insertProjet != -1)
                {
                    await window.ShowMessageAsync("Information", "Le projet a été correctement inséré en base");        
                }
                else
                {
                    await window.ShowMessageAsync("Erreur", "Impossible d'insérer le projet en base");
                }
            }
            else
            {
                await window.ShowMessageAsync("Avertissement", "Merci de compléter tous les champs");
            }

        }

        private void AjouterUnClient()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueClientEdit vce = new VueClientEdit();
            ((VueModeleClientEdit)vce.DataContext).VmNouveauProjet = this;
            vce.Show();
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

        private bool isProjetRefValid;
        private bool isProjetNomValid;

        private DateTime projetDate;
        public DateTime ProjetDate
        {
            get { return projetDate; }
            set
            {
                projetDate = value;
                RaisePropertyChanged(() => ProjetDate);
            }
        }

        private string projetRef;
        public string ProjetRef
        {
            get { return projetRef; }
            set { projetRef = value; }
        }

        private string projetNom;
        public string ProjetNom
        {
            get { return projetNom; }
            set { projetNom = value; }
        }

        private string clientNom;
        public string ClientNom
        {
            get { return clientNom; }
            set { clientNom = value; }
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
