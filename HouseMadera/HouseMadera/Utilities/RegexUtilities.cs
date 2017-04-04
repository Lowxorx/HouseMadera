using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HouseMadera.Utilities
{
    public class RegexUtilities
    {
        bool invalid = false;

        public bool IsValidEmail(string email)
        {
            invalid = false;
            if (string.IsNullOrEmpty(email))
                return true;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;
            try
            {
                //TODO à modifier
                return Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public bool IsValidTelephoneNumber(string telephone)
        {
            invalid = false;
            //TODO à modifier
            try
            {
                if (string.IsNullOrEmpty(telephone))
                    return true;
                return Regex.IsMatch(telephone,
                      @"^[0]\d\d\d\d\d\d\d\d\d$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public bool IsNomInvalide(string value)
        {

            if (string.IsNullOrEmpty(value))
                return false;
            var match = Regex.Match(value, @"\d+"); //Le nom ne doit pas contenir de numero
            return match.Success;
               
        }

        //public bool IsValidNumeroVoie(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return true;
        //    var match = Regex.Match(value, @"^\d*\s?(bis|ter)?$");
        //    return match.Success;
        //}

        public bool HasSpecialCharacters(string value)
        {
            if(string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"[!@#;.+/`\(\^\{\)\]@\[\]§%€\$\*]"); //Ne doi^t pas contenir de caractères spéciaux
        }

    }
    
}
