using System.Data.Entity;

namespace HouseMadera.Mysql
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