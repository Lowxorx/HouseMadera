using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilites;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleClientEdit : ViewModelBase, IDataErrorInfo
    {
        #region PROPRIETES
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

        public RegexUtilities reg { get; set; }
        public ICommand Enregistrer { get; private set; }
        public ICommand Retour { get; private set; }
        #endregion

        public VueModeleClientEdit()
        {
            Enregistrer = new RelayCommand(EnregistrerClient);
            Retour = new RelayCommand(AfficherPagePrecedente);
            Communes = new List<Commune>();
            reg = new RegexUtilities();
            IsClientEnregistre = false;
        }

    

        public string Error
        {
            get
            {
                return Error;
            }
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
                        isChampsNomOk = Nom == null || result != string.Empty ? false : true;
                        break;
                    case "Prenom":
                        result = reg.IsNomInvalide(Prenom) ? "Le prenom ne doit pas contenir de chiffre" : string.Empty;
                        isChampsPrenomOk = Prenom == null || result != string.Empty ? false : true;
                        break;
                    case "Voie":
                        result = reg.HasSpecialCharacters(Voie) ? "Caractères spéciaux non admis " : string.Empty;
                        isChampsVoieOk = Voie == null || result != string.Empty ? false : true;
                        break;
                    case "Complement":
                        result = reg.HasSpecialCharacters(Complement) ? "Caractères spéciaux non admis " : string.Empty;
                        isChampsComplementOk = Complement == null || result != string.Empty ? true : false;
                        break;
                    case "Mobile":
                        result = reg.IsValidTelephoneNumber(Mobile) ? string.Empty : "Format admis ex: 0xxxxxxxxx";
                        isChampsMobileOk = Mobile == null || result != string.Empty ? false : true;
                        break;
                    case "Telephone":
                        result = reg.IsValidTelephoneNumber(Telephone) ? string.Empty : "Format admis ex: 0xxxxxxxxx";
                        isChampsTelephoneOk = Telephone == null || result != string.Empty ? false : true;
                        break;
                    case "Email":
                        result = reg.IsValidEmail(Email) ? string.Empty : "Format de l'e-mail non valide ex: jean.dupont@exemple.fr ";
                        isChampsEmailOk = Email == null || result != string.Empty ? false : true;
                        break;
                    case "Localite":
                        result = reg.HasSpecialCharacters(Localite) ? "Caractères spéciaux non admis " : string.Empty;
                       isChampsLocaliteOk = Localite == null || result != string.Empty ? false : true;
                        break;
                    case "CodePostal":
                        result = reg.HasSpecialCharacters(CodePostal) ? "Caractères spéciaux non admis " : string.Empty;
                        isChampsCodePostalOk = CodePostal == null || result != string.Empty ? false : true;
                        break;
                }
                 
                IsFormulaireOk = VerifierTouslesChamps();
                return result;
            }
        }
        #endregion

        #region METHODES

        private void AfficherPagePrecedente()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueClientList vcl = new VueClientList();
            vcl.Show();
            window.Close();
        }

        private bool VerifierTouslesChamps()
        {
            if (isChampsNomOk && isChampsPrenomOk && isChampsVoieOk && isChampsComplementOk && isChampsCodePostalOk && isChampsLocaliteOk && isChampsMobileOk && isChampsTelephoneOk && isChampsEmailOk)
                return true;
            else
                return false;
        }

        private void EnregistrerClient()
        {
            if (IsFormulaireOk)
            {
                Client nouveauClient = new Client()
                {
                    Nom = Nom,
                    Prenom = Prenom,
                    Adresse1 = Voie,
                    Adresse2 = Complement,
                    CodePostal = CodePostal,
                    Ville = Localite,
                    Telephone = Telephone,
                    Mobile = Mobile,
                    Email = Email,
                    StatutClient = INACTIF
                };
                try
                {
                    using (ClientDAL dal = new ClientDAL("SQLITE"))
                    {
                        int success = dal.InsertClient(nouveauClient);
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

        private List<Commune> RechercherCommunes(string codePostal)
        {
            int i;
            var isCodePostal = int.TryParse(codePostal, out i);

            List<Commune> communes = new List<Commune>();
            //TODO modifier "SQLITE" par Bdd
            if (codePostal != string.Empty && isCodePostal)
            {
                using (var dal = new CommuneDAL("SQLITE"))
                {
                    communes = dal.GetFilteredCommunes(Convert.ToInt32(codePostal));
                }
            }

            return communes;
        }
        #endregion
    }
}
