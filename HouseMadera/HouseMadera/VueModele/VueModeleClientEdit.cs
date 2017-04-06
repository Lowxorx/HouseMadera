using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleClientEdit : ViewModelBase, IDataErrorInfo
    {
        #region PROPRIETES

        /// <summary>
        /// Constantes
        /// </summary>
        private const string OBLIGATOIRE = "*";
        private const int ACTIF = 1;
        private const int INACTIF = 2;


        /// <summary>
        /// Nom
        /// </summary>
        private bool isChampsNomOk = false;
        private string nom;
        public string Nom
        {
            get { return nom; }
            set
            {
                nom = value;
            }
        }

        /// <summary>
        /// Prenom
        /// </summary>
        private bool isChampsPrenomOk = false;
        private string prenom;
        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        /// <summary>
        /// Voie
        /// </summary>
        private bool isChampsVoieOk = false;
        private string voie;
        public string Voie
        {
            get { return voie; }
            set { voie = value; }
        }

        /// <summary>
        /// Complement
        /// </summary>
        private bool isChampsComplementOk = false;
        private string complement;
        public string Complement
        {
            get { return complement; }
            set { complement = value; }
        }

        /// <summary>
        /// Code Postal
        /// </summary>
        private bool isChampsCodePostalOk = false;
        private string codePostal;
        public string CodePostal
        {
            get { return codePostal; }
            set
            {


                Communes.Clear();
                //si la nouvelle valeur est différente de la précédente && si ce n'est pas un code postal
                if (CodePostal != value)
                {
                    //Dérouler la liste des suggestions si la liste n'est pas vide
                    //Communes.Clear();
                    Localite = string.IsNullOrEmpty(value) ? string.Empty : localite;
                    codePostal = value;
                    Communes = RechercherCommunes(value);
                    SuggestionCommunes = Communes.Count > 0 && CodePostal.Length < 5 ? Visibility.Visible : Visibility.Collapsed;

                }
                else
                {
                    SuggestionCommunes = Visibility.Collapsed;
                }
                RaisePropertyChanged(() => CodePostal);

            }
        }

        /// <summary>
        /// Localite
        /// </summary>
        private bool isChampsLocaliteOk = false;
        private string localite;
        public string Localite
        {
            get { return localite; }
            set
            {
                localite = value;

                SuggestionCommunes = Visibility.Collapsed; //Réduit la liste de suggestion
                RaisePropertyChanged(() => Localite);
            }
        }

        /// <summary>
        /// Mobible
        /// </summary>
        private bool isChampsMobileOk = false;
        private string mobile;
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        /// <summary>
        /// Telephone
        /// </summary>
        private bool isChampsTelephoneOk = false;
        private string telephone;
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        /// <summary>
        /// Email
        /// </summary>
        private bool isChampsEmailOk = false;
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// StatutClient lié au toggleswitch
        /// </summary>
        private bool statutClient;
        public bool StatutClient
        {
            get { return statutClient; }
            set
            {
                statutClient = value;
                RaisePropertyChanged(() => StatutClient);
            }
        }

        /// <summary>
        /// Communes
        /// </summary>
        private List<Commune> communes;
        public List<Commune> Communes
        {
            get { return communes; }
            set
            {
                communes = value;
                RaisePropertyChanged(() => Communes);
            }
        }

        /// <summary>
        /// SuggestionCommunes lié à la listBox Suggestion
        /// </summary>
        private object suggestionCommunes;
        public object SuggestionCommunes
        {
            get { return suggestionCommunes; }
            set
            {
                suggestionCommunes = value;
                RaisePropertyChanged(() => SuggestionCommunes);
            }
        }

        /// <summary>
        /// IsFormulaireOK
        /// </summary>
        private bool isFormulaireOk;
        public bool IsFormulaireOk
        {
            get { return isFormulaireOk; }
            set
            {
                // isFormulaireOK = VerifierInfosClient();
                isFormulaireOk = value;
                RaisePropertyChanged(() => IsFormulaireOk);
            }
        }



        /// <summary>
        /// IsClientEnregistre
        /// </summary>
        private bool isClientEnregistre;
        public bool IsClientEnregistre
        {
            get { return isClientEnregistre; }
            set
            {
                isClientEnregistre = value;
                RaisePropertyChanged(() => IsClientEnregistre);
            }
        }

        public string Error
        {
            get
            {
                return Error;
            }
        }
        /// <summary>
        /// CommuneSelectionnée lié a un item de la combobox code postal
        /// </summary>
        private Commune communeSelectionnee;
        public Commune CommuneSelectionnee
        {
            get { return communeSelectionnee; }
            set
            {
                if (value != null)
                {
                    communeSelectionnee = value;
                    RaisePropertyChanged(() => CommuneSelectionnee);
                    Localite = value.Nom_commune;
                    CodePostal = Convert.ToString(value.Code_postal);
                }
            }
        }

        /// <summary>
        /// ClientSelectionne en lien avec l'item de la datagrid (VueClientList)
        /// </summary>
        private Client clientSelectionne;
        public Client ClientSelectionne
        {
            get { return clientSelectionne; }
            set
            {

                clientSelectionne = value;
            }
        }

        /// <summary>
        /// Variables
        /// </summary>
        private bool isMiseAJourClient;
        private int idClientAMettreAJour;
        public RegexUtilities reg { get; set; }

        private Commercial commercialConnecte;
        public Commercial CommercialConnecte
        {
            get { return commercialConnecte; }
            set
            {
                commercialConnecte = value;
            }
        }

        private MetroWindow vuePrecedente;
        public MetroWindow VuePrecedente
        {
            get { return vuePrecedente; }
            set { vuePrecedente = value; }
        }



        /// <summary>
        /// Commandes liées aux boutons de la vue
        /// </summary>
        public ICommand Enregistrer { get; private set; }
        public ICommand Retour { get; private set; }
        public ICommand Deconnexion { get; set; }
        #endregion

        public VueModeleClientEdit()
        {
            Enregistrer = new RelayCommand(EnregistrerClient);
            Retour = new RelayCommand(AfficherPagePrecedente);
            Deconnexion = new RelayCommand(Deconnecter);
            Communes = new List<Commune>();
            reg = new RegexUtilities();
            IsClientEnregistre = false;
            isMiseAJourClient = false;
        }

        #region REGLES DE VALIDATION
        public string this[string columnName]
        {
            get
            {
                var result = string.Empty;
                switch (columnName)
                {
                    case "Nom":
                        result = reg.IsNomInvalide(Nom) ? "Le nom ne doit pas contenir de chiffre" : string.Empty;
                        isChampsNomOk = string.IsNullOrEmpty(Nom) || result != string.Empty ? false : true;
                        break;
                    case "Prenom":
                        result = reg.IsNomInvalide(Prenom) ? "Le prenom ne doit pas contenir de chiffre" : string.Empty;
                        isChampsPrenomOk = string.IsNullOrEmpty(Prenom) || result != string.Empty ? false : true;
                        break;
                    case "Voie":
                        result = reg.HasSpecialCharacters(Voie) ? "Caractères spéciaux non admis " : string.Empty;
                        isChampsVoieOk = string.IsNullOrEmpty(Voie) || result != string.Empty ? false : true;
                        break;
                    case "Complement":
                        result = reg.HasSpecialCharacters(Complement) ? "Caractères spéciaux non admis " : string.Empty;
                        isChampsComplementOk = string.IsNullOrWhiteSpace(result) ? true : false;
                        break;
                    case "Mobile":
                        result = reg.IsValidTelephoneNumber(Mobile) ? string.Empty : "Format admis ex: 0xxxxxxxxx";
                        isChampsMobileOk = string.IsNullOrEmpty(Mobile) || result != string.Empty ? false : true;
                        break;
                    case "Telephone":
                        result = reg.IsValidTelephoneNumber(Telephone) ? string.Empty : "Format admis ex: 0xxxxxxxxx";
                        // isChampsTelephoneOk = Telephone == null || result != string.Empty ? false : true;
                        isChampsTelephoneOk = true;
                        break;
                    case "Email":
                        result = reg.IsValidEmail(Email) ? string.Empty : "Format de l'e-mail non valide ex: jean.dupont@exemple.fr ";
                        isChampsEmailOk = string.IsNullOrEmpty(Email) || result != string.Empty ? false : true;
                        break;
                    case "Localite":
                        result = reg.HasSpecialCharacters(Localite) ? "Caractères spéciaux non admis " : string.Empty;
                        isChampsLocaliteOk = string.IsNullOrEmpty(Localite) || result != string.Empty ? false : true;
                        break;
                    case "CodePostal":
                        result = reg.HasSpecialCharacters(CodePostal) ? "Caractères spéciaux non admis " : string.Empty;
                        isChampsCodePostalOk = string.IsNullOrEmpty(CodePostal) || result != string.Empty ? false : true;
                        break;
                }

                IsFormulaireOk = VerifierTouslesChamps();
                return result;
            }
        }
        #endregion

        #region METHODES

        /// <summary>
        /// Initialise les propriétés du Vue Modele
        /// </summary>
        /// <param name="clientSelectionne"></param>
        public void InitVueModele(Client clientSelectionne)
        {
            if (clientSelectionne != null)
            {
                idClientAMettreAJour = clientSelectionne.Id;
                Nom = clientSelectionne.Nom;
                Prenom = clientSelectionne.Prenom;
                Voie = clientSelectionne.Adresse1;
                Complement = clientSelectionne.Adresse2 + " " + clientSelectionne.Adresse3;
                CodePostal = clientSelectionne.CodePostal;
                Localite = clientSelectionne.Ville;
                Mobile = clientSelectionne.Mobile;
                Telephone = clientSelectionne.Telephone;
                Email = clientSelectionne.Email;
                StatutClient = clientSelectionne.StatutClient == 1 ? true : false;
                IsFormulaireOk = true;
                isMiseAJourClient = true;
            }
        }

        /// <summary>
        /// Instancie la vue précédente et ferme la vue courante
        /// </summary>
        private async void AfficherPagePrecedente()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment fermer l'édition de client ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    AnimateHide = false,
                    AnimateShow = true
                });

                if (result == MessageDialogResult.Affirmative)
                {
                    VueClientList vcl = new VueClientList();
                    ((VueModeleClientList)vcl.DataContext).VuePrecedente = window;
                    ((VueModeleClientList)vcl.DataContext).CommercialConnecte = CommercialConnecte;
                    vcl.Show();
                    window.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> 'true' si tous les champs respectent les conditions sinon 'false' </returns>
        private bool VerifierTouslesChamps()
        {
            if (isChampsNomOk && isChampsPrenomOk && isChampsVoieOk && isChampsComplementOk && isChampsCodePostalOk && isChampsLocaliteOk && isChampsMobileOk && isChampsTelephoneOk && isChampsEmailOk)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Enregistre un nouveau client dans la base de donnée ou le met à jour 
        /// </summary>
        private void EnregistrerClient()
        {
            if (IsFormulaireOk)
            {
                Client client = new Client()
                {
                    Id = idClientAMettreAJour,
                    Nom = Nom,
                    Prenom = Prenom,
                    Adresse1 = Voie,
                    Adresse2 = Complement,
                    CodePostal = CodePostal,
                    Ville = Localite,
                    Telephone = Telephone,
                    Mobile = Mobile,
                    Email = Email,
                    StatutClient = StatutClient ? ACTIF : INACTIF,
                    MiseAJour = isMiseAJourClient ? DateTime.Now : (DateTime?)null,
                    Suppression = null,
                    Creation = !isMiseAJourClient ? (DateTime?)null:DateTime.Now

                };
                try
                {
                    using (ClientDAL dal = new ClientDAL(DAL.DAL.Bdd))
                    {
                        int success = isMiseAJourClient ? dal.UpdateModele(client,null) : dal.InsertModele(client);
                        //Si au moins une ligne a été créé en base alors on notifie le succes de l'enregistrement
                        IsClientEnregistre = success > 0 ? true : false;
                    }
                }
                catch
                {
                    IsClientEnregistre = false;
                    Console.WriteLine("Le client n'a pas pu être enregistré en base");
                }
            }


        }

        /// <summary>
        /// Recherche la ou les communes en fonction du code postal
        /// </summary>
        /// <param name="codePostal"></param>
        /// <returns></returns>
        private List<Commune> RechercherCommunes(string codePostal)
        {
            var isCodePostal = int.TryParse(codePostal, out int i);

            List<Commune> communes = new List<Commune>();
            if (codePostal != string.Empty && isCodePostal)
            {
                using (var dal = new CommuneDAL(DAL.DAL.Bdd))
                {
                    communes = dal.GetFilteredCommunes(Convert.ToInt32(codePostal));
                }
            }

            return communes;
        }

        /// <summary>
        /// Affiche une invite de confirmation et redirige vers la vue login 
        /// </summary>
        private async void Deconnecter()
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
                    vl.Show();
                    window.Close();
                }
            }
        }
        #endregion
    }
}
