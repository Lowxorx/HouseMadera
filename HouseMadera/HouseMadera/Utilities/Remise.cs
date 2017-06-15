namespace HouseMadera.Utilities
{
    public class Remise
    {
        public static decimal CalculerPrixRemise(int remise, decimal prix)
        {
            return prix - ((prix * remise) / 100);
        }
    }
}
