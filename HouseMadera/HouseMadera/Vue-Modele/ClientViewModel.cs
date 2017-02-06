
using System;
using System.Text.RegularExpressions;

namespace HouseMadera.Vue_Modele
{
    public class ClientViewModel 
    {
        private string _nom;
        public string Nom {
            get { return 
                    _nom; }
            set {
                var match = Regex.Match(value, @"\d+");
                if (match.Success)
                    throw new ApplicationException("Le nom ne doit pas contenir de chiffre");
            }
        }
        private string _prenom;
        public string Prenom { get; set; }
        private string _numero;
        public int Numero { get; set; }
        private string _voie;
        public string Voie { get; set; }
        private string _nomvoie;
        public string NomVoie { get; set; }
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
    }
}
