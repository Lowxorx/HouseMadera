namespace HouseMadera.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modif_Data_Annotation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Ville", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "Prenom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "Adresse1", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "Adresse2", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "Adresse3", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "CodePostal", c => c.String(maxLength: 20, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "Email", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "Telephone", c => c.String(maxLength: 15, storeType: "nvarchar"));
            AlterColumn("dbo.Clients", "Mobile", c => c.String(maxLength: 15, storeType: "nvarchar"));
            AlterColumn("dbo.Projets", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Commercials", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Commercials", "Prenom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Commercials", "Login", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Commercials", "Password", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Produits", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.StatutDevis", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Plans", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Gammes", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Finitions", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.TypeFinitions", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Qualites", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.TypeIsolants", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Isolants", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Modules", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Composants", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.TypeComposants", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Slots", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.TypeSlots", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.TypeModules", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.StatutClients", "Nom", c => c.String(maxLength: 255, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StatutClients", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.TypeModules", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.TypeSlots", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Slots", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.TypeComposants", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Composants", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Modules", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Isolants", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.TypeIsolants", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Qualites", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.TypeFinitions", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Finitions", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Gammes", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Plans", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.StatutDevis", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Produits", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Commercials", "Password", c => c.String(unicode: false));
            AlterColumn("dbo.Commercials", "Login", c => c.String(unicode: false));
            AlterColumn("dbo.Commercials", "Prenom", c => c.String(unicode: false));
            AlterColumn("dbo.Commercials", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Projets", "Nom", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "Mobile", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "Telephone", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "Email", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "CodePostal", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "Adresse3", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "Adresse2", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "Adresse1", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "Prenom", c => c.String(unicode: false));
            AlterColumn("dbo.Clients", "Nom", c => c.String(unicode: false));
            DropColumn("dbo.Clients", "Ville");
        }
    }
}
