using GalaSoft.MvvmLight;
using HouseMadera.Modeles;
using HouseMadera.Utilites;
using System;
using System.Collections.ObjectModel;

namespace HouseMadera.VueModele
{
    public class VueModeleClientEdit : ViewModelBase
    {
        #region
        private string _nom;
        public string Nom { get; set; }
        private string _prenom;
        public string Prenom { get; set; }
        private string _numero;
        public string Numero { get; set; }
        private string _voie;
        public string Voie { get; set; }
        private string _complement;
        public string Complement { get; set; }
        private int _codepostal;
        public int CodePostal { get; set; }
        private string _localite;
        public string Localite { get; set; }
        private string _mobile;
        public string Mobile { get; set; }
        private string _telephone;
        public string Telephone { get; set; }
        private string _email;
        public string Email { get; set; }
        private string recherche;
        public string NomCommune { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public RegexUtilities reg { get; set; }
        #endregion

        public VueModeleClientEdit()
        {
           
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

        private string ExtractNomCommune(string communeSelected)
        {
            var offset = 7;
            return communeSelected.Substring(offset, communeSelected.Length - offset);
        }

        private string ExtractCodePostal(string communeSelected)
        {
            var offset = 0;
            return communeSelected.Substring(offset, 5);
        }
    }
}
