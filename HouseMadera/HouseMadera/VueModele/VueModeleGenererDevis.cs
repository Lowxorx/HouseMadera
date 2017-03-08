using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modeles;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace HouseMadera.VueModele
{
    public class VueModeleGenererDevis : ViewModelBase
    {

        [PreferredConstructor]
        public VueModeleGenererDevis()
        {
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Deconnexion = new RelayCommand(Logout);
            Retour = new RelayCommand(RetourDetailsProjets);
            EnregistrerDevis = new RelayCommand(SaveDevis);
            EnvoiDevis = new RelayCommand(EnvoyerDevis);
        }

        public ICommand WindowLoaded { get; private set; }
        public ICommand Deconnexion { get; private set; }
        public ICommand Retour { get; private set; }
        public ICommand EnvoiDevis { get; private set; }
        public ICommand EnregistrerDevis { get; private set; }


        private MetroWindow vuePrecedente;
        public MetroWindow VuePrecedente
        {
            get { return vuePrecedente; }
            set
            {
                vuePrecedente = value;

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

        private List<String> listeModules;
        public List<String> ListeModules
        {
            get { return listeModules; }
            set
            {
                listeModules = value;
                RaisePropertyChanged("ListeModules");
            }
        }

        private string prixHT;
        public string PrixHT
        {
            get { return prixHT; }
            set
            {
                prixHT = value;
                RaisePropertyChanged(() => PrixHT);
            }
        }

        private string prixTTC;
        public string PrixTTC
        {
            get { return prixTTC; }
            set
            {
                prixTTC = value;
                RaisePropertyChanged(() => PrixTTC);
            }
        }

        private DevisGenere dGen;
        public DevisGenere DGen
        {
            get { return dGen; }
            set { dGen = value; }
        }

        private void SaveDevis()
        {

        }

        private void EnvoyerDevis()
        {

            // envoi devis au client.
            // spécifier le chemin du pdf et le client

            // TODO : externaliser le template (dans un fichier ou dans un formulaire à remplir / modifier par le commercial)
            SmtpClient client = new SmtpClient()
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("serviceclient.madera@gmail.com", "Rila2016")
            };

            MailMessage mm = new MailMessage("serviceclient.madera@gmail.com", "paul.marquie@viacesi.fr");
            mm.Subject = @"Votre Devis pour votre maison modulaire - Madera";
            mm.Body = @"Cher client, " + Environment.NewLine +
                    "vous trouverez ci-joint le devis pour votre maison modulaire réalisée le " + DateTime.Now.ToLongDateString() + Environment.NewLine + @"." +
                    "N'hésitez pas à nous contacter pour toute information complémentaire dont vous auriez besoin." + Environment.NewLine +
                    "Cordialement," + Environment.NewLine +
                    "La société Madera.";

            Attachment attachment = new Attachment("Devis.pdf");
            mm.Attachments.Add(attachment);
            mm.BodyEncoding = Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mm);
            mm.Dispose();
        }

        private async void Logout()
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

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
            // Actions à effectuer au lancement du form :
            ListeModules = new List<String>();
            ListeModules = DGen.Modules;
            PrixHT = DGen.PrixHT + @" €";
            PrixTTC = DGen.PrixTTC + @" €";
            GenererPdfDevis();
        }

        private async void RetourDetailsProjets()
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

        private void GenererPdfDevis()
        {
            FileStream fs = new FileStream("First PDF document.pdf", FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.AddAuthor("Madera");
            document.AddCreator("Société Madera");
            document.AddKeywords("Devis généré par l'application Mader'house");
            document.AddTitle("Devis généré le " + DateTime.Now.ToLongDateString());
            document.Open();
            document.Add(new Paragraph("Hello World!"));
            document.Close();
            writer.Close();
            fs.Close();
        }

    }
}
