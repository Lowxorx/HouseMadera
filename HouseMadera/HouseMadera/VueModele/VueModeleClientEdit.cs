using GalaSoft.MvvmLight;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace HouseMadera.VueModele
{
    public class VueModeleClientEdit : ViewModelBase, IDataErrorInfo
    {
        #region
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Numero { get; set; }
        public string Voie { get; set; }
        public string NomVoie { get; set; }
        public string Complement { get; set; }
        private string codePostal;
        public string CodePostal
        {
            get { return codePostal; }
            set
            {

                
                Communes.Clear();
                //si la nouvelle valeur est différente de la précédente && si ce n'est pas un code postal
                if ( CodePostal != value)
                {
                    //Dérouler la liste des suggestions si la liste n'est pas vide
                    //Communes.Clear();
                    Localite = string.IsNullOrEmpty(value) ? string.Empty : localite;
                    codePostal = value;
                    Communes = RechercherCommunes(value);
                    SuggestionCommunes = Communes.Count > 0 && CodePostal.Length <5  ? Visibility.Visible : Visibility.Collapsed;
                    
                }
                else
                {
                    SuggestionCommunes = Visibility.Collapsed;
                }
                RaisePropertyChanged(() => CodePostal);

            }
        }
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
        private string mobile;
        public string Mobile { get; set; }
        private string telephone;
        public string Telephone { get; set; }
        private string email;
        public string Email { get; set; }
        public string NomCommune { get; set; }
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
        public ObservableCollection<Client> Clients { get; set; }
        public RegexUtilities reg { get; set; }
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

        #endregion

        public VueModeleClientEdit()
        {
            Communes = new List<Commune>();
            reg = new RegexUtilities();
       

        }


        public string Error
        {
            get
            {
                throw new NotImplementedException();
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
                        if (Nom == null)
                            break;
                        result = reg.IsValidName(Nom) ? "Le nom ne doit pas contenir de chiffre" : string.Empty;
                        break;
                    case "Prenom":
                        if (Prenom == null)
                            break;
                        result = reg.IsValidName(Prenom) ? "Le prenom ne doit pas contenir de chiffre" : string.Empty;
                        break;
                    case "Numero":
                        if (Numero == null)
                            break;
                        result = reg.IsValidNumeroVoie(Numero) ? string.Empty : "Le format du numéro de voie incorrect ex 6,6 Bis, 6 ter ";
                        break;
                    case "Voie":
                        if (Voie == null)
                            break;
                        result = reg.HasSpecialCharacters(Voie) ? string.Empty : "Caractères spéciaux non admis ";
                        break;
                    case "Complement":
                        if (Complement == null)
                            break;
                        result = reg.HasSpecialCharacters(Complement) ? string.Empty : "Caractères spéciaux non admis ";
                        break;
                    case "Mobile":
                        if (Mobile == null)
                            break;
                        result = reg.IsValidTelephoneNumber(Mobile) ? string.Empty : "Format admis ex: 0xxxxxxxxx";
                        break;
                    case "Telephone":
                        if (Telephone == null)
                            break;
                        result = reg.IsValidTelephoneNumber(Telephone) ? string.Empty : "Format admis ex: 0xxxxxxxxx";
                        break;
                    case "Email":
                        if (Email == null)
                            break;
                        result = reg.IsValidEmail(Email) ? string.Empty : "Format de l'e-mail non valide ex: jean.dupont@exemple.fr ";
                        break;
                }

                return result;
            }
        }
        #endregion



        #region METHODES
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
