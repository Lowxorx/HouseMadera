using System.Data.Entity;

namespace HouseMadera.DataLayer
{
    public class HouseMaderaContextInitializer : CreateDatabaseIfNotExists<HouseMaderaContext>
    {
        protected override void Seed(HouseMaderaContext context)
        {
            base.Seed(context);
        }

        //TODO seed
    }
}