using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    class VueModeleChoixAdmin : ViewModelBase
    {
        private const int NB_MODELE = 27;
        public ICommand AdminProjet { get; private set; }
        public ICommand AdminClient { get; private set; }
        public ICommand Deconnexion { get; private set; }
        public ICommand LancerSynchro { get; set; }

        [PreferredConstructor]
        public VueModeleChoixAdmin()
        {
            // Check de la connexion
            ConnectivityMonitor testConnection = new ConnectivityMonitor();
            bool isOnline = testConnection.IsOnline();
            IsSynchronisationEffectuee = isOnline ? true : false;
            Deconnexion = new RelayCommand(Deco);
            AdminProjet = new RelayCommand(AProjet);
            AdminClient = new RelayCommand(AClient);
            LancerSynchro = new RelayCommand(Synchroniser);
        }

        private Commercial commercialConnecte;
        public Commercial CommercialConnecte
        {
            get { return commercialConnecte; }
            set { commercialConnecte = value; }
        }

        private bool isSynchronisationEffectuee;
        public bool IsSynchronisationEffectuee
        {
            get { return isSynchronisationEffectuee; }
            set
            {
                isSynchronisationEffectuee = value;
                RaisePropertyChanged(() => IsSynchronisationEffectuee);
            }
        }

        private double Pourcentage(int value)
        {
            return (double)((value * 100) / NB_MODELE);
        }

        /// <summary>
        /// Cette méthode permet de synchroniser tous les modèles synchronisable en commençant par les modèles que n'ont pas de clé étrangère
        /// </summary>
        private async void Synchroniser()
        {

            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();

            var controller = await window.ShowProgressAsync("Veuillez patienter...", "La synchronisation des données est en cours");
            controller.SetIndeterminate();

            Thread SyncThread = new Thread(async delegate ()
            {
                Synchronisation.NbErreurs = 0;
                Synchronisation.NbModeleSynchronise = 0;

                Synchronisation<CommercialDAL, Commercial> syncCommercial = new Synchronisation<CommercialDAL, Commercial>(new Commercial());
                syncCommercial.synchroniserDonnees();
                //la progression se fait de 0% à 100%
                controller.Minimum = 0;
                controller.Maximum = 100;
                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<StatutDevisDAL, StatutDevis> syncStatutDevis = new Synchronisation<StatutDevisDAL, StatutDevis>(new StatutDevis());
                syncStatutDevis.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<DevisDAL, Devis> syncDevis = new Synchronisation<DevisDAL, Devis>(new Devis());
                syncDevis.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));
                Synchronisation<StatutClientDAL, StatutClient> syncStatutClient = new Synchronisation<StatutClientDAL, StatutClient>(new StatutClient());
                syncStatutClient.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<ClientDAL, Client> syncClient = new Synchronisation<ClientDAL, Client>(new Client());
                syncClient.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<ProjetDAL, Projet> syncProjet = new Synchronisation<ProjetDAL, Projet>(new Projet());
                syncProjet.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<TypeSlotDAL, TypeSlot> syncTypeSlot = new Synchronisation<TypeSlotDAL, TypeSlot>(new TypeSlot());
                syncTypeSlot.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<SlotDAL, Slot> syncSlot = new Synchronisation<SlotDAL, Slot>(new Slot());
                syncSlot.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<StatutProduitDAL, StatutProduit> syncStatutProduit = new Synchronisation<StatutProduitDAL, StatutProduit>(new StatutProduit());
                syncStatutProduit.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<CoupePrincipeDAL, CoupePrincipe> syncCoupePrincipe = new Synchronisation<CoupePrincipeDAL, CoupePrincipe>(new CoupePrincipe());
                syncCoupePrincipe.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<QualiteDAL, Qualite> syncQualite = new Synchronisation<QualiteDAL, Qualite>(new Qualite());
                syncQualite.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<TypeModuleDAL, TypeModule> syncTypeModule = new Synchronisation<TypeModuleDAL, TypeModule>(new TypeModule());
                syncTypeModule.synchroniserDonnees();
                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<TypeIsolantDAL, TypeIsolant> syncTypeIsolant = new Synchronisation<TypeIsolantDAL, TypeIsolant>(new TypeIsolant());
                syncTypeIsolant.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<TypeFinitionDAL, TypeFinition> syncTypeFinition = new Synchronisation<TypeFinitionDAL, TypeFinition>(new TypeFinition());
                syncTypeFinition.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<FinitionDAL, Finition> syncFinition = new Synchronisation<FinitionDAL, Finition>(new Finition());
                syncFinition.synchroniserDonnees();
                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<IsolantDAL, Isolant> syncIsolant = new Synchronisation<IsolantDAL, Isolant>(new Isolant());
                syncIsolant.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<GammeDAL, Gamme> syncGamme = new Synchronisation<GammeDAL, Gamme>(new Gamme());
                syncGamme.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<TypeComposantDAL, TypeComposant> syncTypeComposant = new Synchronisation<TypeComposantDAL, TypeComposant>(new TypeComposant());
                syncTypeComposant.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<ComposantDAL, Composant> syncComposant = new Synchronisation<ComposantDAL, Composant>(new Composant());
                syncComposant.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<ModuleDAL, Modeles.Module> syncModule = new Synchronisation<ModuleDAL, Modeles.Module>(new Modeles.Module());
                syncModule.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<ComposantModuleDAL, ComposantModule> syncComposantModule = new Synchronisation<ComposantModuleDAL, ComposantModule>(new ComposantModule(), true);
                syncComposantModule.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<TypeModulePlacableDAL, TypeModulePlacable> syncTypeModulePlacable = new Synchronisation<TypeModulePlacableDAL, TypeModulePlacable>(new TypeModulePlacable());
                syncTypeModulePlacable.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<SlotPlaceDAL, SlotPlace> syncSlotPlace = new Synchronisation<SlotPlaceDAL, SlotPlace>(new SlotPlace());
                syncSlotPlace.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<PlanDAL, Plan> syncPlan = new Synchronisation<PlanDAL, Plan>(new Plan());
                syncPlan.synchroniserDonnees();
                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<ProduitDAL, Produit> syncProduit = new Synchronisation<ProduitDAL, Produit>(new Produit());
                syncProduit.synchroniserDonnees();
                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<ModulePlaceDAL, ModulePlace> syncModulePlace = new Synchronisation<ModulePlaceDAL, ModulePlace>(new ModulePlace(), true);
                syncModulePlace.synchroniserDonnees();

                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                Synchronisation<ModulePlacePlanDAL, ModulePlacePlan> syncModulePlacePlan = new Synchronisation<ModulePlacePlanDAL, ModulePlacePlan>(new ModulePlacePlan(), true);
                syncModulePlacePlan.synchroniserDonnees();
                controller.SetProgress(Pourcentage(Synchronisation.NbModeleSynchronise));

                await controller.CloseAsync();

                //Affichage du compte rendu dans un pop-up

                if (window != null)
                {
                    string conseil = Synchronisation.NbErreurs > 0 ? "Merci de contacter le support" : string.Empty;
                    string rapport = string.Format("La synchronisation des données est terminée ({0}/27) avec {1} Erreurs\n {2}", Synchronisation.NbModeleSynchronise, Synchronisation.NbErreurs, conseil);
                    window.BeginInvoke(delegate () { window.ShowMessageAsync("Synchronisation", rapport, MessageDialogStyle.Affirmative); });
                }
            });
            SyncThread.Start();
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
            ((VueModeleChoixProjet)vcp.DataContext).VuePrecedente = window;
            vcp.Show();
            window.Close();
        }

    }
}