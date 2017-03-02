using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.VueModele
{
    public class VueModeleCommune
    {
        public string NomCommune { get; set; }
        public string CodePostal { get; set; }

        public VueModeleCommune(string value)
        {
            NomCommune = ExtractNomCommune(value);
            CodePostal = ExtractCodePostal(value);
        }

       

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
