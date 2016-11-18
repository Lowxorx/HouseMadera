using HouseMadera.Entities;
using System.Data.Entity;

namespace HouseMadera.DataLayer
{
    public class HouseMaderaContext : DbContext
    {
        public HouseMaderaContext() : base("HouseMaderaDb")
        {
            Database.SetInitializer<HouseMaderaContext>(new HouseMaderaContextInitializer());
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Commercial> Commercials { get; set; }
        public DbSet<Composant> Composants { get; set; }
        public DbSet<CoupePrincipe> CoupesPrincipes { get; set; }
        public DbSet<Devis> Devis { get; set; }
        public DbSet<Finition> Finitions { get; set; }
        public DbSet<Gamme> Gammes { get; set; }
        public DbSet<Isolant> Isolants { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModulePlace> ModulesPlaces { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<Qualite> Qualites { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<SlotPlace> SlotsPlaces { get; set; }
        public DbSet<StatutClient> StatutsClients { get; set; }
        public DbSet<StatutDevis> StatutsDevis { get; set; }
        public DbSet<TypeComposant> TypesComposants { get; set; }
        public DbSet<TypeFinition> TypesFinitions { get; set; }
        public DbSet<TypeIsolant> TypesIsolants { get; set; }
        public DbSet<TypeModule> TypesModules { get; set; }
        public DbSet<TypeSlot> TypesSlots { get; set; }
    }
}