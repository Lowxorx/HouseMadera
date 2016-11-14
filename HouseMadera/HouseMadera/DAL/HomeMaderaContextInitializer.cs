using System.Data.Entity;

namespace HomeMadera.DataLayer
{
    public class HomeMaderaContextInitializer : CreateDatabaseIfNotExists<HomeMaderaContext>
    {
        protected override void Seed(HomeMaderaContext context)
        {
            base.Seed(context);
        }

        //TODO seed
    }
}