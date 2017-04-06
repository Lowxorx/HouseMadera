using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using HouseMadera.Vues;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleDetailsProjet : ViewModelBase
    {

        [PreferredConstructor]
        public VueModeleDetailsProjet()
        {
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Deconnexion = new RelayCommand(Deco);
            Retour = new RelayCommand(RetourAdminProjet);
            EditerProduit = new RelayCommand(EditionProduit);
            GenererDevis = new RelayCommand(GenDevis);
            GenererPlan = new RelayCommand(GenPlan);
            SelectedProduitCmd = new RelayCommand(ChangeDetailsProduits);
            NouveauProduit = new RelayCommand(CreerUnProduit);
        }

        public ICommand WindowLoaded { get; private set; }
        public ICommand Deconnexion { get; private set; }
        public ICommand Retour { get; private set; }
        public ICommand EditerProduit { get; private set; }
        public ICommand GenererDevis { get; private set; }
        public ICommand GenererPlan { get; private set; }
        public ICommand SelectedProduitCmd { get; private set; }
        public ICommand NouveauProduit { get; private set; }


        private MetroWindow vuePrecedente;
        public MetroWindow VuePrecedente
        {
            get { return vuePrecedente; }
            set
            {
                vuePrecedente = value;
            }
        }

        private Produit selectedProduit;
        public Produit SelectedProduit
        {
            get { return selectedProduit; }
            set
            {
                if (selectedProduit != value)
                    selectedProduit = value;
                RaisePropertyChanged(() => SelectedProduit);
            }
        }

        private Projet selectedProjet;
        public Projet SelectedProjet
        {
            get { return selectedProjet; }
            set
            {
                selectedProjet = value;
            }
        }

        private DevisGenere dGen;
        public DevisGenere DGen
        {
            get { return dGen; }
            set { dGen = value; }
        }

        private string devisActuel;

        public string DevisActuel
        {
            get { return devisActuel; }
            set { devisActuel = value; }
        }

        private ObservableCollection<Produit> listeProduit;
        public ObservableCollection<Produit> ListeProduit
        {
            get
            {
                return listeProduit;
            }
            set
            {
                listeProduit = value;
                RaisePropertyChanged(() => ListeProduit);
            }
        }  

        private string titreProjet;
        public string TitreProjet
        {
            get { return titreProjet; }
            set
            {
                titreProjet = value;
                RaisePropertyChanged(() => TitreProjet);
            }
        }

        private string detailsPrixTTCProduit;
        public string DetailsPrixTTCProduit
        {
            get { return detailsPrixTTCProduit; }
            set
            {
                detailsPrixTTCProduit = value;
                RaisePropertyChanged(() => DetailsPrixTTCProduit);
            }
        }

        private string detailsPrixProduit;
        public string DetailsPrixProduit
        {
            get { return detailsPrixProduit; }
            set
            {
                detailsPrixProduit = value;
                RaisePropertyChanged(() => DetailsPrixProduit);
            }
        }

        private string detailsStatutDevisProduit;
        public string DetailsStatutDevisProduit
        {
            get { return detailsStatutDevisProduit; }
            set
            {
                detailsStatutDevisProduit = value;
                RaisePropertyChanged(() => DetailsStatutDevisProduit);
            }
        }

        private string detailsStatutProduit;
        public string DetailsStatutProduit
        {
            get { return  detailsStatutProduit; }
            set
            {
                detailsStatutProduit = value;
                RaisePropertyChanged(() => DetailsStatutProduit);
            }
        }

        private bool genBtnActif = false;
        public bool GenBtnActif
        {
            get { return genBtnActif; }
            set
            {
                genBtnActif = value;
                RaisePropertyChanged("GenBtnActif");
            }
        }

        private void ChangeDetailsProduits()
        {
            if (selectedProduit.StatutProduit.Nom == "Valide")
            {
                DetailsPrixProduit = string.Format("Prix HT : {0} €", Convert.ToString(selectedProduit.Devis.PrixHT));
                DetailsPrixTTCProduit = string.Format("Prix TTC : {0} €", Convert.ToString(selectedProduit.Devis.PrixTTC));
                DetailsStatutDevisProduit = string.Format("Statut du devis : {0}", Convert.ToString(selectedProduit.Devis.StatutDevis.Nom));
                DetailsStatutProduit = string.Format("Statut du Produit : {0}", Convert.ToString(selectedProduit.StatutProduit.Nom));
                GenBtnActif = true;
            }
            else
            {
                DetailsPrixProduit = "";
                DetailsPrixTTCProduit = "";
                DetailsStatutDevisProduit = "";
                DetailsStatutProduit = string.Format("Statut du Produit : {0}", Convert.ToString(selectedProduit.StatutProduit.Nom));
                GenBtnActif = false;
            }
        }

        private async void Deco()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
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
                    vuePrecedente.Show();
                    vuePrecedente.Close();
                    window.Close();
                    vl.Show();

                }
            }
        }

        private async void RetourAdminProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment fermer ce projet ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    AnimateHide = false,
                    AnimateShow = true
                });

                if (result == MessageDialogResult.Affirmative)
                {
                    vuePrecedente.Show();
                    window.Close();
                }
            }
        }

        private void EditionProduit()
        {

        }

        private void CreerUnProduit()
        {
            try
            {
                string arg = String.Format("{0}", SelectedProjet.Id);
                Console.WriteLine(arg);
                Process HouseEditor = new Process();
                HouseEditor.StartInfo.FileName = AppInfo.AppPath + @"\MaderaHouseEditor";
                HouseEditor.StartInfo.Arguments = arg;
                HouseEditor.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async void GenDevis()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            // Génération du devis 
            List<DataGenerationDevis> listDg = new List<DataGenerationDevis>();
            using (DevisDAL dDal = new DevisDAL(DAL.DAL.Bdd))
            {
                listDg = dDal.GenererDevis(SelectedProduit);
            }
            // Traitement des données
            List<string> modulesDistincts = new List<string>();
            foreach (DataGenerationDevis dg in listDg)
            {
                if (!modulesDistincts.Contains(dg.NomModule))
                {
                    modulesDistincts.Add(dg.NomModule);
                }
            }

            string outputToDevis = @"Devis pour " + listDg.First().NomProduit + " le " + DateTime.Now.ToLongDateString() + Environment.NewLine;
            outputToDevis += string.Format(@"Client : {0} {1}" + Environment.NewLine + Environment.NewLine, listDg.First().Client.Nom, listDg.First().Client.Prenom);
            outputToDevis += @"Détails des modules sélectionnés :" + Environment.NewLine;
            decimal prixTotal = 0;
            double tva  = 1.2;
            List<string> modulesToGrid = new List<string>();
            foreach (string s in modulesDistincts)
            {
                decimal prixModule = 0;
                foreach (DataGenerationDevis dg in listDg)
                {
                    if (s == dg.NomModule)
                    {
                        prixModule += Convert.ToDecimal(dg.PrixComposant) * dg.NombreComposant;
                    }
                }
                prixTotal += prixModule;
                string outputModule = String.Format("Module : {0} | Prix HT : {1} € \n", s, Convert.ToString(prixModule));
                modulesToGrid.Add(outputModule);
                outputToDevis += outputModule;
            }
            string prixFinal = String.Format(Environment.NewLine + "Prix Total HT : {0} € | Prix Total TTC : {1} € \n", Convert.ToString(prixTotal), Convert.ToString(Convert.ToDouble(prixTotal) * tva));
            outputToDevis += prixFinal;

            DGen = new DevisGenere()
            {
                Output = outputToDevis,
                PrixHT = Convert.ToString(prixTotal),
                PrixTTC = Convert.ToString(Convert.ToDouble(prixTotal) * tva),
                Modules = modulesToGrid,
                client = listDg.First().Client
            };

            // Création du PDF
            GenererPdfDevis();

            // Création du devis à insérer en BDD
            Devis d = new Devis()
            {
                DateCreation = DateTime.Now,
                Nom = listDg.First().NomProduit,
                PrixHT = prixTotal,
                PrixTTC = Convert.ToDecimal(Convert.ToDouble(prixTotal) * tva),
                StatutDevis = new StatutDevis() {Id = 2},
                Pdf = File.ReadAllBytes(AppInfo.AppPath + @"\Devis\" + DevisActuel)
            };


            try
            {
                int insertDevis = 0;

                using (DevisDAL dDAl = new DevisDAL(DAL.DAL.Bdd))
                {
                    insertDevis = dDAl.InsertDevis(d);
                }

                if (insertDevis > 0)
                {
                    VueGenererDevis vgd = new VueGenererDevis();
                    ((VueModeleGenererDevis)vgd.DataContext).TitreVue = TitreProjet;
                    ((VueModeleGenererDevis)vgd.DataContext).DGen = DGen;
                    vgd.Show();
                }
                else
                {
                    if (window != null)
                    {
                        await window.ShowMessageAsync("Erreur", "Le devis n'a pas été inséré en base.");
                    }
                }
            }
            catch (Exception ex)
            {

                await window.ShowMessageAsync("Erreur", ex.Message);
                Logger.WriteEx(ex);
            }

        }

        private string GenererPdfDevis()
        {
            DevisActuel = @"devis" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            Console.WriteLine(AppInfo.AppPath + @"\Devis\" + DevisActuel);
            if (!Directory.Exists(AppInfo.AppPath + @"\Devis"))
            {
                Directory.CreateDirectory(AppInfo.AppPath + @"\Devis");
            }
            FileStream fs = new FileStream(AppInfo.AppPath + @"\Devis\" + DevisActuel, FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.AddAuthor("Madera");
            document.AddCreator("Société Madera");
            document.AddKeywords("Devis généré par l'application Mader'house");
            document.AddTitle("Devis généré le " + DateTime.Now.ToLongDateString());
            document.Open();
            document.Add(new Paragraph(DGen.Output));
            document.Close();
            writer.Close();
            fs.Close();
            return DevisActuel;
        }

        private async void GenPlan()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            var rand = new Random();
            var files = Directory.GetFiles(AppInfo.AppPath + @"\Plans\","*.pdf");
            try
            {
                int i = rand.Next(files.Length);
                if (i == 0)
                {
                    i = 1;
                }
                Process.Start(AppInfo.AppPath + @"\Plans\" + i + ".pdf");
            }
            catch (Exception)
            {
                await window.ShowMessageAsync("Erreur", "Impossible d'ouvrir le plan");
            }
        }

        private void ActualiserTitreForm()
        {
            TitreProjet = @"Détails du projet " + selectedProjet.Nom;
            RaisePropertyChanged(() => TitreProjet);
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
            // Actions à effectuer au lancement du form :
            RecupProduitsParProjet();
            ActualiserTitreForm();
        }

        private void RecupProduitsParProjet()
        {
            using (var dal = new ProduitDAL(DAL.DAL.Bdd))
            {
                listeProduit = dal.GetAllProduitsByProjet(selectedProjet);
            }
            RaisePropertyChanged(() => ListeProduit);
        }
    }
}