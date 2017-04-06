using System;
using System.Reflection;
using HouseMadera.DAL;
using System.Collections.Generic;
using System.Linq;
using Synchronisation.Classes;
using Synchronisation.Interfaces;
using HouseMadera.Modeles;

namespace TestSynchronisationBdd
{
    class Program
    {
        private const string MAIN_ASSEMBLY = "HouseMadera";
        public static List<Type> ModelesSynchronises;

        static void Main(string[] args)
        {
            Synchronisation<CommercialDAL, Commercial> syncCommercial = new Synchronisation<CommercialDAL, Commercial>(new Commercial());
            syncCommercial.synchroniserDonnees();
            Synchronisation<StatutDevisDAL, StatutDevis> syncStatutDevis = new Synchronisation<StatutDevisDAL, StatutDevis>(new StatutDevis());
            syncStatutDevis.synchroniserDonnees();
            Synchronisation<DevisDAL, Devis> syncDevis = new Synchronisation<DevisDAL, Devis>(new Devis());
            syncDevis.synchroniserDonnees();
            Synchronisation<StatutClientDAL, StatutClient> syncStatutClient = new Synchronisation<StatutClientDAL, StatutClient>(new StatutClient());
            syncStatutClient.synchroniserDonnees();
            Synchronisation<ClientDAL, Client> syncClient = new Synchronisation<ClientDAL, Client>(new Client());
            syncClient.synchroniserDonnees();
            Synchronisation<ProjetDAL, Projet> syncProjet = new Synchronisation<ProjetDAL, Projet>(new Projet());
            syncProjet.synchroniserDonnees();
            Synchronisation<TypeSlotDAL, TypeSlot> syncTypeSlot = new Synchronisation<TypeSlotDAL, TypeSlot>(new TypeSlot());
            syncTypeSlot.synchroniserDonnees();
            Synchronisation<SlotDAL, Slot> syncSlot = new Synchronisation<SlotDAL, Slot>(new Slot());
            syncSlot.synchroniserDonnees();
            Synchronisation<StatutProduitDAL, StatutProduit> syncStatutProduit = new Synchronisation<StatutProduitDAL, StatutProduit>(new StatutProduit());
            syncStatutProduit.synchroniserDonnees();
            Synchronisation<CoupePrincipeDAL, CoupePrincipe> syncCoupePrincipe = new Synchronisation<CoupePrincipeDAL, CoupePrincipe>(new CoupePrincipe());
            syncCoupePrincipe.synchroniserDonnees();
            Synchronisation<QualiteDAL, Qualite> syncQualite = new Synchronisation<QualiteDAL, Qualite>(new Qualite());
            syncQualite.synchroniserDonnees();
            Synchronisation<TypeModuleDAL, TypeModule> syncTypeModule = new Synchronisation<TypeModuleDAL, TypeModule>(new TypeModule());
            syncTypeModule.synchroniserDonnees();
            Synchronisation<TypeIsolantDAL, TypeIsolant> syncTypeIsolant = new Synchronisation<TypeIsolantDAL, TypeIsolant>(new TypeIsolant());
            syncTypeIsolant.synchroniserDonnees();
            Synchronisation<TypeFinitionDAL, TypeFinition> syncTypeFinition = new Synchronisation<TypeFinitionDAL, TypeFinition>(new TypeFinition());
            syncTypeFinition.synchroniserDonnees();
            Synchronisation<FinitionDAL, Finition> syncFinition = new Synchronisation<FinitionDAL, Finition>(new Finition());
            syncFinition.synchroniserDonnees();
            Synchronisation<IsolantDAL, Isolant> syncIsolant = new Synchronisation<IsolantDAL, Isolant>(new Isolant());
            syncIsolant.synchroniserDonnees();
            Synchronisation<GammeDAL, Gamme> syncGamme = new Synchronisation<GammeDAL, Gamme>(new Gamme());
            syncGamme.synchroniserDonnees();
            Synchronisation<TypeComposantDAL, TypeComposant> syncTypeComposant = new Synchronisation<TypeComposantDAL, TypeComposant>(new TypeComposant());
            syncTypeComposant.synchroniserDonnees();
            Synchronisation<ComposantDAL, Composant> syncComposant = new Synchronisation<ComposantDAL, Composant>(new Composant());
            syncComposant.synchroniserDonnees();
            Synchronisation<ModuleDAL, HouseMadera.Modeles.Module> syncModule = new Synchronisation<ModuleDAL, HouseMadera.Modeles.Module>(new HouseMadera.Modeles.Module());
            syncModule.synchroniserDonnees();
            Synchronisation<ComposantModuleDAL, ComposantModule> syncComposantModule = new Synchronisation<ComposantModuleDAL, ComposantModule>(new ComposantModule(), true);
            syncComposantModule.synchroniserDonnees();
            Synchronisation<TypeModulePlacableDAL, TypeModulePlacable> syncTypeModulePlacable = new Synchronisation<TypeModulePlacableDAL, TypeModulePlacable>(new TypeModulePlacable());
            syncTypeModulePlacable.synchroniserDonnees();
            Synchronisation<SlotPlaceDAL, SlotPlace> syncSlotPlace = new Synchronisation<SlotPlaceDAL, SlotPlace>(new SlotPlace());
            syncSlotPlace.synchroniserDonnees();
            Synchronisation<PlanDAL, Plan> syncPlan = new Synchronisation<PlanDAL, Plan>(new Plan());
            syncPlan.synchroniserDonnees();
            Synchronisation<ProduitDAL, Produit> syncProduit = new Synchronisation<ProduitDAL, Produit>(new Produit());
            syncProduit.synchroniserDonnees();
            Synchronisation<ModulePlaceDAL, ModulePlace> syncModulePlace = new Synchronisation<ModulePlaceDAL, ModulePlace>(new ModulePlace(), true);
            syncModulePlace.synchroniserDonnees();
            Synchronisation<ModulePlacePlanDAL, ModulePlacePlan> syncModulePlacePlan = new Synchronisation<ModulePlacePlanDAL, ModulePlacePlan>(new ModulePlacePlan(), true);
            syncModulePlacePlan.synchroniserDonnees();























            //Assembly houseMadera = Assembly.Load(MAIN_ASSEMBLY);
            ////List<Type> heriteDeDal = new List<Type>();
            ////List<Type> synchronizable = new List<Type>();
            ////Dictionary<Type, Type> correspondanceModeleDAL = new Dictionary<Type, Type>();
            //ModelesSynchronises = new List<Type>();


            //bool isImplementeISynchronizable;
            //foreach (Type classe in houseMadera.GetTypes())
            //{
            //    int i = 0;
            //    string padding = string.Empty;
            //    isImplementeISynchronizable = (classe.GetInterface("ISynchronizable") == null) ? false : true;
            //    //Trouve les classes implémentant l'interface ISynchronizable
            //    if (isImplementeISynchronizable)
            //    {
            //        Console.WriteLine(string.Format("################ {0}",classe.Name));
            //        RechercherDependance(classe,padding,i);
            //        i++;
            //    }
            //}
            ////foreach (Type type in houseMadera.GetTypes())
            ////{
            ////    //Affiche uniquement les types dont le nom est DAL
            ////    if (type.Name.Contains("DAL") && type.BaseType == typeof(DAL))
            ////    {
            ////        heriteDeDal.Add(type);
            ////        Console.WriteLine(string.Format("Type : {0} --> Hérite de DAL ", type.Name));
            ////    }
            ////    else
            ////    {
            ////        bool isImplementeISynchronizable = (type.GetInterface("ISynchronizable") == null) ? false : true;
            ////        //Affiche les classes implémentant l'interface ISynchronizable
            ////        if (isImplementeISynchronizable)
            ////        {
            ////            synchronizable.Add(type);
            ////            Console.WriteLine(string.Format("Type : {0} --> Implémente ISynchronizable ", type.Name));
            ////        }
            ////    }
            ////}

            ////Console.WriteLine(string.Format("Nombre de modele :{0}", synchronizable.Count));
            ////Console.WriteLine(string.Format("Nombre de modeleDAL :{0}", heriteDeDal.Count));

            //////Association modele-modeleDAL utilisé par la suite pour la classe générique Synchronisation<TMODELEDAL,TMODELE>
            ////foreach (Type modeleDAL in heriteDeDal)
            ////{
            ////    foreach (Type modele in synchronizable)
            ////    {
            ////        string tmp = modeleDAL.Name.Replace("DAL", string.Empty);
            ////        if (tmp == modele.Name)
            ////            correspondanceModeleDAL.Add(modeleDAL, modele);
            ////    }
            ////}
            ////foreach (var item in correspondanceModeleDAL)
            ////{
            ////    Console.WriteLine(string.Format("{0} - {1}", item.Key, item.Value));
            ////}

            //Console.ReadKey();
        }

        public static Type RechercherDependance(Type modele,string padding,int i)
        {
            bool isImplementeISynchronizable;
            isImplementeISynchronizable = (modele.GetInterface("ISynchronizable") == null) ? false : true;
            //Trouve les classes implémentant l'interface ISynchronizable
            if (isImplementeISynchronizable)
            {
                
                padding = padding.PadLeft(i,'-');
                Console.WriteLine(string.Format("{0}{1}",padding,modele.Name));
                foreach (PropertyInfo propriete in modele.GetProperties())
                {
                     RechercherDependance(propriete.PropertyType,padding,i);
                    i++;
                }
                if (!ModelesSynchronises.Contains<Type>(modele))
                {
                    //TODO Appeller methode de synchronisation
                    ModelesSynchronises.Add(modele);
                }
                   
            }

            

            return null;
        }
    }
}


