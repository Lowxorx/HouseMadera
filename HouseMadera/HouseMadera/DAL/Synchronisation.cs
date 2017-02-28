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
        private TMODELE _modele;

        private List<TMODELE> listeModeleDistante = new List<TMODELE>();
        private List<TMODELE> listeModeleLocale = new List<TMODELE>();
        public Dictionary<int, int> correspondanceModeleId = new Dictionary<int, int>();
        private bool changements = false;

        public Synchronisation(TMODELE modele)
        {
            _modele = modele;

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
                foreach (var modeleDistant in listeModeleDistante)
                {
                    isInDistant = modeleLocal.Equals(modeleDistant);
                    if (isInDistant)
                    {
                        //enregistrer les id dans une table de correspondance
                        correspondanceModeleId.Add(modeleLocal.Id, modeleDistant.Id);
                        break;
                    }

                }
                if (!isInDistant)
                {
                    Console.WriteLine(modeleLocal.Id + " n'existe pas sur le serveur distant");
                    //Enregistrer le client sur le serveur distant
                    using (dalBddDistante)
                    {
                        try
                        {
                            int nbLigneInseree = dalBddDistante.InsertNew(modeleLocal);
                            if (nbLigneInseree > 0)
                            {
                                Console.Write("-------- OK");
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
                foreach (var modeleLocal in listeModeleLocale)
                {
                    isInLocal = modeleDistant.Equals(modeleLocal);
                    if (isInLocal)
                        break;
                }
                if (!isInLocal)
                {
                    Console.WriteLine(modeleDistant.Id + " n'existe pas sur le serveur local");
                    using (dalBddLocale)
                    {
                        try
                        {
                            int nbLigneInseree = dalBddLocale.InsertNew(modeleDistant);
                            if (nbLigneInseree > 0)
                            {
                                Console.Write("-------- OK");
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
            Console.WriteLine("correspondance des ID de la table {0}:\n", _modele.GetType().Name);
            Console.WriteLine("SQLITE".PadRight(10) + "MYSQL\n");
            foreach (var cor in correspondanceModeleId)
            {
                Console.WriteLine(cor.Key.ToString().PadRight(10) + cor.Value);
            }
            Console.ReadLine();
        }
    }
}
