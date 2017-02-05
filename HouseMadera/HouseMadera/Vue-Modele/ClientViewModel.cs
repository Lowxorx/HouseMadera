using System;


namespace HouseMadera.Vue_Modele
{
    public class ClientViewModel :System.ComponentModel.IDataErrorInfo
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Numero { get; set; }
        public string Voie { get; set; }
        public string NomVoie { get; set; }
        public string Complement { get; set; }
        public int CodePostal { get; set; }
        public string Localite { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    //case "CodePostal":
                }
                throw new NotImplementedException();
            }
        }
    }
}
