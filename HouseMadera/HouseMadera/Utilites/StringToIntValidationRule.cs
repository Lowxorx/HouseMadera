using System.Windows.Controls;

namespace HouseMadera.Utilites
{
    public class StringToIntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i;
            if (int.TryParse(value.ToString(), out i))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Le Code Postal doit comporter que des chiffres");
        }
    }
}
