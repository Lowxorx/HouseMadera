namespace HouseMadera.Mysql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Prenom = c.String(maxLength: 255),
                        Adresse1 = c.String(maxLength: 255),
                        Adresse2 = c.String(maxLength: 255),
                        Adresse3 = c.String(maxLength: 255),
                        CodePostal = c.String(maxLength: 20),
                        Ville = c.String(maxLength: 255),
                        Email = c.String(maxLength: 255),
                        Telephone = c.String(maxLength: 15),
                        Mobile = c.String(maxLength: 15),
                        StatutClient_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StatutClients", t => t.StatutClient_Id)
                .Index(t => t.StatutClient_Id);
            
            CreateTable(
                "dbo.Projets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Reference = c.String(),
                        UpdateDate = c.DateTime(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Client_Id = c.Int(),
                        Commercial_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .ForeignKey("dbo.Commercials", t => t.Commercial_Id)
                .Index(t => t.Client_Id)
                .Index(t => t.Commercial_Id);
            
            CreateTable(
                "dbo.Commercials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Prenom = c.String(maxLength: 255),
                        Login = c.String(maxLength: 255),
                        Password = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Produits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Devis_Id = c.Int(),
                        Plan_Id = c.Int(),
                        Projet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devis", t => t.Devis_Id)
                .ForeignKey("dbo.Plans", t => t.Plan_Id)
                .ForeignKey("dbo.Projets", t => t.Projet_Id)
                .Index(t => t.Devis_Id)
                .Index(t => t.Plan_Id)
                .Index(t => t.Projet_Id);
            
            CreateTable(
                "dbo.Devis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        DateCreation = c.DateTime(nullable: false),
                        PrixHT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrixTTC = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StatutDevis_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StatutDevis", t => t.StatutDevis_Id)
                .Index(t => t.StatutDevis_Id);
            
            CreateTable(
                "dbo.StatutDevis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        CreateDate = c.DateTime(nullable: false),
                        CoupePrincipe_Id = c.Int(),
                        Gamme_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CoupePrincipes", t => t.CoupePrincipe_Id)
                .ForeignKey("dbo.Gammes", t => t.Gamme_Id)
                .Index(t => t.CoupePrincipe_Id)
                .Index(t => t.Gamme_Id);
            
            CreateTable(
                "dbo.CoupePrincipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gammes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Isolant_Id = c.Int(),
                        Finition_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Isolants", t => t.Isolant_Id)
                .ForeignKey("dbo.Finitions", t => t.Finition_Id)
                .Index(t => t.Isolant_Id)
                .Index(t => t.Finition_Id);
            
            CreateTable(
                "dbo.Finitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        TypeFinition_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TypeFinitions", t => t.TypeFinition_Id)
                .Index(t => t.TypeFinition_Id);
            
            CreateTable(
                "dbo.TypeFinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Qualite_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Qualites", t => t.Qualite_Id)
                .Index(t => t.Qualite_Id);
            
            CreateTable(
                "dbo.Qualites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TypeIsolants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Qualite_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Qualites", t => t.Qualite_Id)
                .Index(t => t.Qualite_Id);
            
            CreateTable(
                "dbo.Isolants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        TypeIsolant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TypeIsolants", t => t.TypeIsolant_Id)
                .Index(t => t.TypeIsolant_Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Hauteur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Largeur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Gamme_Id = c.Int(),
                        TypeModule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gammes", t => t.Gamme_Id)
                .ForeignKey("dbo.TypeModules", t => t.TypeModule_Id)
                .Index(t => t.Gamme_Id)
                .Index(t => t.TypeModule_Id);
            
            CreateTable(
                "dbo.Composants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Prix = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TypeComposant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TypeComposants", t => t.TypeComposant_Id)
                .Index(t => t.TypeComposant_Id);
            
            CreateTable(
                "dbo.TypeComposants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Qualite_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Qualites", t => t.Qualite_Id)
                .Index(t => t.Qualite_Id);
            
            CreateTable(
                "dbo.SlotPlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abscisse = c.Int(nullable: false),
                        Ordonnee = c.Int(nullable: false),
                        Module_Id = c.Int(),
                        Slot_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.Module_Id)
                .ForeignKey("dbo.Slots", t => t.Slot_Id)
                .Index(t => t.Module_Id)
                .Index(t => t.Slot_Id);
            
            CreateTable(
                "dbo.ModulePlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Module_Id = c.Int(),
                        SlotPlace_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.Module_Id)
                .ForeignKey("dbo.SlotPlaces", t => t.SlotPlace_Id)
                .Index(t => t.Module_Id)
                .Index(t => t.SlotPlace_Id);
            
            CreateTable(
                "dbo.Slots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                        Hauteur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Largeur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TypeSlot_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TypeSlots", t => t.TypeSlot_Id)
                .Index(t => t.TypeSlot_Id);
            
            CreateTable(
                "dbo.TypeSlots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TypeModules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatutClients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComposantModules",
                c => new
                    {
                        Composant_Id = c.Int(nullable: false),
                        Module_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Composant_Id, t.Module_Id })
                .ForeignKey("dbo.Composants", t => t.Composant_Id, cascadeDelete: true)
                .ForeignKey("dbo.Modules", t => t.Module_Id, cascadeDelete: true)
                .Index(t => t.Composant_Id)
                .Index(t => t.Module_Id);
            
            CreateTable(
                "dbo.ModulePlacePlans",
                c => new
                    {
                        ModulePlace_Id = c.Int(nullable: false),
                        Plan_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ModulePlace_Id, t.Plan_Id })
                .ForeignKey("dbo.ModulePlaces", t => t.ModulePlace_Id, cascadeDelete: true)
                .ForeignKey("dbo.Plans", t => t.Plan_Id, cascadeDelete: true)
                .Index(t => t.ModulePlace_Id)
                .Index(t => t.Plan_Id);
            
            CreateTable(
                "dbo.TypeModuleTypeSlots",
                c => new
                    {
                        TypeModule_Id = c.Int(nullable: false),
                        TypeSlot_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TypeModule_Id, t.TypeSlot_Id })
                .ForeignKey("dbo.TypeModules", t => t.TypeModule_Id, cascadeDelete: true)
                .ForeignKey("dbo.TypeSlots", t => t.TypeSlot_Id, cascadeDelete: true)
                .Index(t => t.TypeModule_Id)
                .Index(t => t.TypeSlot_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "StatutClient_Id", "dbo.StatutClients");
            DropForeignKey("dbo.Produits", "Projet_Id", "dbo.Projets");
            DropForeignKey("dbo.Produits", "Plan_Id", "dbo.Plans");
            DropForeignKey("dbo.Plans", "Gamme_Id", "dbo.Gammes");
            DropForeignKey("dbo.TypeModuleTypeSlots", "TypeSlot_Id", "dbo.TypeSlots");
            DropForeignKey("dbo.TypeModuleTypeSlots", "TypeModule_Id", "dbo.TypeModules");
            DropForeignKey("dbo.Modules", "TypeModule_Id", "dbo.TypeModules");
            DropForeignKey("dbo.Slots", "TypeSlot_Id", "dbo.TypeSlots");
            DropForeignKey("dbo.SlotPlaces", "Slot_Id", "dbo.Slots");
            DropForeignKey("dbo.ModulePlaces", "SlotPlace_Id", "dbo.SlotPlaces");
            DropForeignKey("dbo.ModulePlacePlans", "Plan_Id", "dbo.Plans");
            DropForeignKey("dbo.ModulePlacePlans", "ModulePlace_Id", "dbo.ModulePlaces");
            DropForeignKey("dbo.ModulePlaces", "Module_Id", "dbo.Modules");
            DropForeignKey("dbo.SlotPlaces", "Module_Id", "dbo.Modules");
            DropForeignKey("dbo.Modules", "Gamme_Id", "dbo.Gammes");
            DropForeignKey("dbo.TypeComposants", "Qualite_Id", "dbo.Qualites");
            DropForeignKey("dbo.Composants", "TypeComposant_Id", "dbo.TypeComposants");
            DropForeignKey("dbo.ComposantModules", "Module_Id", "dbo.Modules");
            DropForeignKey("dbo.ComposantModules", "Composant_Id", "dbo.Composants");
            DropForeignKey("dbo.Gammes", "Finition_Id", "dbo.Finitions");
            DropForeignKey("dbo.TypeIsolants", "Qualite_Id", "dbo.Qualites");
            DropForeignKey("dbo.Isolants", "TypeIsolant_Id", "dbo.TypeIsolants");
            DropForeignKey("dbo.Gammes", "Isolant_Id", "dbo.Isolants");
            DropForeignKey("dbo.TypeFinitions", "Qualite_Id", "dbo.Qualites");
            DropForeignKey("dbo.Finitions", "TypeFinition_Id", "dbo.TypeFinitions");
            DropForeignKey("dbo.Plans", "CoupePrincipe_Id", "dbo.CoupePrincipes");
            DropForeignKey("dbo.Devis", "StatutDevis_Id", "dbo.StatutDevis");
            DropForeignKey("dbo.Produits", "Devis_Id", "dbo.Devis");
            DropForeignKey("dbo.Projets", "Commercial_Id", "dbo.Commercials");
            DropForeignKey("dbo.Projets", "Client_Id", "dbo.Clients");
            DropIndex("dbo.Clients", new[] { "StatutClient_Id" });
            DropIndex("dbo.Produits", new[] { "Projet_Id" });
            DropIndex("dbo.Produits", new[] { "Plan_Id" });
            DropIndex("dbo.Plans", new[] { "Gamme_Id" });
            DropIndex("dbo.TypeModuleTypeSlots", new[] { "TypeSlot_Id" });
            DropIndex("dbo.TypeModuleTypeSlots", new[] { "TypeModule_Id" });
            DropIndex("dbo.Modules", new[] { "TypeModule_Id" });
            DropIndex("dbo.Slots", new[] { "TypeSlot_Id" });
            DropIndex("dbo.SlotPlaces", new[] { "Slot_Id" });
            DropIndex("dbo.ModulePlaces", new[] { "SlotPlace_Id" });
            DropIndex("dbo.ModulePlacePlans", new[] { "Plan_Id" });
            DropIndex("dbo.ModulePlacePlans", new[] { "ModulePlace_Id" });
            DropIndex("dbo.ModulePlaces", new[] { "Module_Id" });
            DropIndex("dbo.SlotPlaces", new[] { "Module_Id" });
            DropIndex("dbo.Modules", new[] { "Gamme_Id" });
            DropIndex("dbo.TypeComposants", new[] { "Qualite_Id" });
            DropIndex("dbo.Composants", new[] { "TypeComposant_Id" });
            DropIndex("dbo.ComposantModules", new[] { "Module_Id" });
            DropIndex("dbo.ComposantModules", new[] { "Composant_Id" });
            DropIndex("dbo.Gammes", new[] { "Finition_Id" });
            DropIndex("dbo.TypeIsolants", new[] { "Qualite_Id" });
            DropIndex("dbo.Isolants", new[] { "TypeIsolant_Id" });
            DropIndex("dbo.Gammes", new[] { "Isolant_Id" });
            DropIndex("dbo.TypeFinitions", new[] { "Qualite_Id" });
            DropIndex("dbo.Finitions", new[] { "TypeFinition_Id" });
            DropIndex("dbo.Plans", new[] { "CoupePrincipe_Id" });
            DropIndex("dbo.Devis", new[] { "StatutDevis_Id" });
            DropIndex("dbo.Produits", new[] { "Devis_Id" });
            DropIndex("dbo.Projets", new[] { "Commercial_Id" });
            DropIndex("dbo.Projets", new[] { "Client_Id" });
            DropTable("dbo.TypeModuleTypeSlots");
            DropTable("dbo.ModulePlacePlans");
            DropTable("dbo.ComposantModules");
            DropTable("dbo.StatutClients");
            DropTable("dbo.TypeModules");
            DropTable("dbo.TypeSlots");
            DropTable("dbo.Slots");
            DropTable("dbo.ModulePlaces");
            DropTable("dbo.SlotPlaces");
            DropTable("dbo.TypeComposants");
            DropTable("dbo.Composants");
            DropTable("dbo.Modules");
            DropTable("dbo.Isolants");
            DropTable("dbo.TypeIsolants");
            DropTable("dbo.Qualites");
            DropTable("dbo.TypeFinitions");
            DropTable("dbo.Finitions");
            DropTable("dbo.Gammes");
            DropTable("dbo.CoupePrincipes");
            DropTable("dbo.Plans");
            DropTable("dbo.StatutDevis");
            DropTable("dbo.Devis");
            DropTable("dbo.Produits");
            DropTable("dbo.Commercials");
            DropTable("dbo.Projets");
            DropTable("dbo.Clients");
        }
    }
}
