using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    class VueModeleChoixAdmin : ViewModelBase
    {
        public ICommand AdminProjet { get; private set; }
        public ICommand AdminClient { get; private set; }
        public ICommand Logout { get; private set; }
        public ICommand LancerSynchro { get; set; }
        private bool isSynchronisationActive;
        public bool IsSynchronisationActive
        {
            get { return isSynchronisationActive; }
            set
            {
                isSynchronisationActive = value;
                RaisePropertyChanged(() => IsSynchronisationActive);
            }
        }

        [PreferredConstructor]
        public VueModeleChoixAdmin()
        {
            Logout = new RelayCommand(Deco);
            AdminProjet = new RelayCommand(AProjet);
            AdminClient = new RelayCommand(AClient);
            LancerSynchro = new RelayCommand(Synchroniser);
        }

        /// <summary>
        /// Cette méthode permet de synchroniser tous les modèles synchronisable en commençant par les modèles que n'ont pas de clé étrangère
        /// </summary>
        private void Synchroniser()
        {
            IsSynchronisationActive = true;
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
            Synchronisation<ModuleDAL, Module> syncModule = new Synchronisation<ModuleDAL, Module>(new Module());
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

            IsSynchronisationActive = false;
            //Affichage du compte rendu dans un pop-up
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            if (window != null)
            {
                string rapport = string.Format("La synchronisation des données est terminée {0}/27 avec {1} Erreurs\n", Synchronisation.NbModeleSynchronise, Synchronisation.NbErreurs);
                var result = window.ShowMessageAsync("Synchronisation", rapport, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings());
            }



        }

        private Commercial commercialConnecte;
        public Commercial CommercialConnecte
        {
            get { return commercialConnecte; }
            set { commercialConnecte = value; }
        }

        private async void Deco()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment vous déconnecter ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    AnimateHide = false,
                    AnimateShow = true
                });

                if (result == MessageDialogResult.Affirmative)
                {
                    VueLogin vl = new VueLogin();
                    vl.Show();
                    window.Close();
                }
            }
        }

        private void AClient()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueClientList vcl = new VueClientList();
            vcl.Show();
            window.Close();
        }

        private void AProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueChoixProjet vcp = new VueChoixProjet();
            ((VueModeleChoixProjet)vcp.DataContext).CommercialConnecte = CommercialConnecte;
            vcp.Show();
            window.Close();
        }
    }
}