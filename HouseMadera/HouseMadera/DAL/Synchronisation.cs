using HouseMadera.Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.DAL
{
    public class Synchronisation<TDAL, TMODELE>
        where TDAL : DAL, IDAL<TMODELE>
        where TMODELE : ISynchronizable, new()
    {
        private const string DISTANTE = "MYSQL";
        private const string LOCALE = "SQLITE";


        private TDAL dalBddLocale;
        private TDAL dalBddDistante;
        public string NomModele { get; set; }

        private List<TMODELE> listeModeleDistante = new List<TMODELE>();
        private List<TMODELE> listeModeleLocale = new List<TMODELE>();
        public Dictionary<int, int> correspondanceModeleId = new Dictionary<int, int>();
        private bool changements = false;

        public Synchronisation(TMODELE modele)
        {
            NomModele = typeof(TMODELE).Name;

            //Récupérer les enregistrements de la base distante
            using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
            {
                try
                {
                    listeModeleDistante = dalBddDistante.GetAll();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            //Récupérer les enregistrements de la base locale
            using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), LOCALE))
            {
                try
                {

                    listeModeleLocale = dalBddLocale.GetAll();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }



        }

        public void synchroniserDonnees()
        {


            //Comparer les deux listes
            //Est-ce que mon client local existe en remote
            foreach (var modeleLocal in listeModeleLocale)
            {
                bool isInDistant = false;
                bool isUpToDate = false;
                bool isDeleted = false;
                foreach (var modeleDistant in listeModeleDistante)
                {
                    isInDistant = modeleLocal.Equals(modeleDistant);
                    if (isInDistant)
                    {
                        //Est-ce que le modèle a été supprimé sur le serveur distant
                        isDeleted = modeleLocal.IsDeleted(modeleDistant);
                        if (isDeleted)
                        {
                            using (dalBddLocale= (TDAL)Activator.CreateInstance(typeof(TDAL), LOCALE))
                            {
                                dalBddLocale.DeleteModele(modeleLocal);
                            }
                        }
                        else
                        {
                            //Est-ce que le modèle a été modifié sur le serveur distant
                            isUpToDate = modeleLocal.IsUpToDate(modeleDistant);
                            
                            if (!isUpToDate)
                            {
                                using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), LOCALE))
                                {
                                    dalBddLocale.UpdateModele(modeleLocal, modeleDistant);
                                }
                            }
                        }
                        //enregistrer les id dans une table de correspondance
                        correspondanceModeleId.Add(modeleLocal.Id, modeleDistant.Id);
                        break;
                    }

                }
                if (!isInDistant)
                {
                    Console.WriteLine("l'entité {0} avec l'ID " + modeleLocal.Id + " n'existe pas sur le serveur distant\n", NomModele);
                    //Enregistrer le client sur le serveur distant
                    using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
                    {
                        try
                        {
                            int nbLigneInseree = dalBddDistante.InsertModele(modeleLocal);
                            if (nbLigneInseree > 0)
                            {
                                Console.WriteLine("Enregistrement --------> OK\n");
                                changements = true;
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }

            //Est-ce que mon client distant existe en local
            foreach (var modeleDistant in listeModeleDistante)
            {
                bool isInLocal = false;
                bool isUpToDate = false;
                bool isDeleted = false;

                foreach (var modeleLocal in listeModeleLocale)
                {
                    isInLocal = modeleDistant.Equals(modeleLocal);

                    if (isInLocal)
                    {
                        //Est-ce que le modèle a été supprimé sur le serveur distant
                        isDeleted = modeleLocal.IsDeleted(modeleLocal);
                        if (isDeleted)
                        {
                            using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
                            {
                                dalBddDistante.DeleteModele(modeleDistant);
                            }
                        }
                        else
                        {
                            //Est-ce que le modèle a été modifié sur le serveur distant
                            isUpToDate = modeleDistant.IsUpToDate(modeleLocal);

                            if (!isUpToDate)
                            {
                                using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
                                {
                                    dalBddDistante.UpdateModele(modeleDistant, modeleLocal);
                                }
                            }
                        }
                      
                        break;
                    }
                       
                }
                if (!isInLocal)
                {
                    Console.WriteLine("l'entité {0} avec l'ID " + modeleDistant.Id + " n'existe pas sur le serveur local\n", NomModele);
                    // Console.WriteLine(modeleDistant.Id + " n'existe pas sur le serveur local");
                    using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), LOCALE))
                    {
                        try
                        {
                            int nbLigneInseree = dalBddLocale.InsertModele(modeleDistant);
                            if (nbLigneInseree > 0)
                            {
                                Console.Write("-------- OK\n");
                                changements = true;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }


            string resultat = changements ? "Synchronisation réussie" : "Tout est à jour";
            Console.WriteLine(resultat);
            Console.WriteLine("correspondance des ID de la table {0}:\n", NomModele);
            Console.WriteLine("SQLITE".PadRight(10) + "MYSQL\n");
            foreach (var cor in correspondanceModeleId)
            {
                Console.WriteLine(cor.Key.ToString().PadRight(10) + cor.Value);
            }
            Console.ReadLine();
        }
    }
}
