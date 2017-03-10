
using System;
using System.Collections.Generic;


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
            recupererDonnees();
            ////Récupérer les enregistrements de la base distante
            //using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
            //{
            //    try
            //    {
            //        listeModeleDistante = dalBddDistante.GetAllModeles();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
            ////Récupérer les enregistrements de la base locale
            //using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), LOCALE))
            //{
            //    try
            //    {

            //        listeModeleLocale = dalBddLocale.GetAllModeles();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
        }

        public void synchroniserDonnees()
        {
            comparerDonnees(listeModeleLocale, listeModeleDistante, "SORTANT");
            comparerDonnees(listeModeleDistante, listeModeleLocale, "ENTRANT");
            //enregistrer les id dans une table de correspondance
            creerTableCorrespondance();
            afficherResultat();
            //Comparer les deux listes
            //Est-ce que l'entité locale existe dans la base distante
            //foreach (var modeleLocal in listeModeleLocale)
            //{
            //    bool isInDistant = false;
            //    bool isUpToDate = false;
            //    bool isDeleted = false;
            //    foreach (var modeleDistant in listeModeleDistante)
            //    {
            //        isInDistant = modeleLocal.Equals(modeleDistant);
            //        if (isInDistant)
            //        {
            //            //Est-ce que l'entité a été supprimée dans la base distante
            //            isDeleted = modeleLocal.IsDeleted(modeleDistant);
            //            if (isDeleted)
            //            {
            //                using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), LOCALE))
            //                {
            //                    Console.WriteLine("\nL'entité {0} avec l'ID {1} a été effacé dans la base distante\n", NomModele, modeleLocal.Id);
            //                    string deleteResult = dalBddLocale.DeleteModele(modeleLocal) > 0 ? "OK" : "NOK";
            //                    Console.WriteLine("\nSuppression dans la base locale -----> {0}", deleteResult);
            //                }
            //            }
            //            else
            //            {
            //                //Est-ce que l'entité a été modifiée dans la base distante
            //                isUpToDate = modeleLocal.IsUpToDate(modeleDistant);

            //                if (!isUpToDate)
            //                {
            //                    using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), LOCALE))
            //                    {
            //                        Console.WriteLine("\nl'entité {0} avec l'ID {1} a été modifié dans la base distante\n", NomModele, modeleLocal.Id);
            //                        string updateResult = dalBddLocale.UpdateModele(modeleLocal, modeleDistant) > 0 ? "OK" : "NOK";
            //                        Console.WriteLine("\n Modification dans la base locale -----> {0}", updateResult);
            //                    }
            //                }
            //            }
            //            //enregistrer les id dans une table de correspondance
            //            correspondanceModeleId.Add(modeleLocal.Id, modeleDistant.Id);
            //            break;
            //        }

            //    }
            //    if (!isInDistant)
            //    {
            //        Console.WriteLine("l'entité {0} avec l'ID {1} n'existe pas dans la base distante\n", NomModele, modeleLocal.Id);
            //        //Enregistrer l'entité dans la base distante
            //        using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
            //        {
            //            try
            //            {
            //                int nbLigneInseree = dalBddDistante.InsertModele(modeleLocal);
            //                if (nbLigneInseree > 0)
            //                {
            //                    Console.WriteLine("Enregistrement --------> OK\n");
            //                    changements = true;
            //                }

            //            }
            //            catch (Exception e)
            //            {
            //                Console.WriteLine(e.Message);
            //            }
            //        }
            //    }
            //}

            ////Est-ce que l'entité distante éxiste dans la base locale
            //foreach (var modeleDistant in listeModeleDistante)
            //{
            //    bool isInLocal = false;
            //    bool isUpToDate = false;
            //    bool isDeleted = false;

            //    foreach (var modeleLocal in listeModeleLocale)
            //    {
            //        isInLocal = modeleDistant.Equals(modeleLocal);

            //        if (isInLocal)
            //        {
            //            //Est-ce que l'entité a été supprimée dans la base locale
            //            isDeleted = modeleLocal.IsDeleted(modeleLocal);
            //            if (isDeleted)
            //            {
            //                using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
            //                {
            //                    Console.WriteLine("\nL'entité {0} avec l'ID {1} a été effacé sur le serveur local\n", NomModele, modeleDistant.Id);
            //                    dalBddDistante.DeleteModele(modeleDistant);
            //                    Console.WriteLine("\nSuppression ----- OK");
            //                }
            //            }
            //            else
            //            {
            //                //Est-ce que l'entité a été modifiée dans la base locale
            //                isUpToDate = modeleDistant.IsUpToDate(modeleLocal);

            //                if (!isUpToDate)
            //                {
            //                    using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
            //                    {
            //                        Console.WriteLine("\nL'entité {0} avec l'ID {1} a été modifié sur le serveur local\n", NomModele,modeleDistant.Id);
            //                        dalBddDistante.UpdateModele(modeleDistant, modeleLocal);
            //                        Console.WriteLine("\nModification ----- OK");
            //                    }
            //                }
            //            }

            //            break;
            //        }

            //    }
            //    if (!isInLocal)
            //    {
            //        Console.WriteLine("l'entité {0} avec l'ID {1} n'existe pas dans la base locale\n", NomModele, modeleDistant.Id);
            //        using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), LOCALE))
            //        {
            //            try
            //            {
            //                int nbLigneInseree = dalBddLocale.InsertModele(modeleDistant);
            //                if (nbLigneInseree > 0)
            //                {
            //                    Console.Write("-------- OK\n");
            //                    changements = true;
            //                }
            //            }
            //            catch (Exception e)
            //            {
            //                Console.WriteLine(e.Message);
            //            }
            //        }
            //    }
            //}


            
        }

        private void creerTableCorrespondance()
        {
            recupererDonnees();
            foreach(TMODELE modeleLocal in listeModeleLocale)
            {
                foreach(TMODELE modeleDistant in listeModeleDistante)
                {
                    if (modeleLocal.Equals(modeleDistant))
                    {
                        correspondanceModeleId.Add(modeleLocal.Id, modeleDistant.Id);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Compare chaque élément d'une liste avec une autre. Si l'élément est semblable
        /// détermine si il a été mis à jour ou supprimé.Si l'élément  comparable est
        /// absent de la liste distante celui-ci est inséré dans la base distante.
        /// </summary>
        /// <param name="liste1"> représente la liste principale (locale) à comparer</param>
        /// <param name="liste2">représente la liste secondaire(distante) à comparer</param>
        /// <param name="sens">indique le sens de comparaison local / distant ou distant / local</param>
        private void comparerDonnees(List<TMODELE> liste1, List<TMODELE> liste2, string sens)
        {
            string bdd = string.Empty;
            string locale = string.Empty;
            string distante = string.Empty;

            switch (sens)
            {
                //du local au distant
                case "SORTANT":
                    bdd = "locale";
                    locale = LOCALE;
                    distante = DISTANTE;
                    break;
                //du distant au local
                case "ENTRANT":
                    bdd = "distante";
                    locale = DISTANTE;
                    distante = LOCALE;
                    break;

            }

            Console.WriteLine("############################################ {0}", sens);
            foreach (var modeleListe1 in liste1)
            {
                bool isInDistant = false;
                bool isUpToDate = false;
                bool isDeleted = false;
                foreach (var modeleListe2 in liste2)
                {
                    isInDistant = modeleListe1.Equals(modeleListe2);
                    if (isInDistant)
                    {
                        //Est-ce que le modèle a été supprimé sur le serveur distant
                        isDeleted = modeleListe1.IsDeleted(modeleListe2);
                        if (isDeleted)
                        {
                            using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), locale))
                            {
                                Console.WriteLine("\nl'entité {0} avec l'ID {1} a été effacé sur la bdd  {2}\n", NomModele, modeleListe1.Id, bdd);
                                dalBddLocale.DeleteModele(modeleListe1);
                                Console.WriteLine("\nSuppression ----- OK");
                            }
                        }
                        else
                        {
                            //Est-ce que le modèle a été modifié sur le serveur distant
                            isUpToDate = modeleListe1.IsUpToDate(modeleListe2);

                            if (!isUpToDate)
                            {
                                using (dalBddLocale = (TDAL)Activator.CreateInstance(typeof(TDAL), locale))
                                {
                                    Console.WriteLine("\nL'entité {0} avec l'ID {1} a été modifié sur la bdd {2}\n", NomModele, modeleListe1.Id, bdd);
                                    dalBddLocale.UpdateModele(modeleListe1, modeleListe2);
                                    Console.WriteLine("\nModification -------> OK");
                                }
                            }
                        }
                        break;
                    }
                }
                if (!isInDistant)
                {
                    Console.WriteLine("L'entité {0} avec l'ID {1} n'existe pas sur bdd {2}\n", NomModele, modeleListe1.Id, bdd);
                    //Enregistrer le client sur le serveur distant
                    using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), distante))
                    {
                        try
                        {
                            int nbLigneInseree = dalBddDistante.InsertModele(modeleListe1);
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
        }

        private void recupererDonnees()
        {
            //Récupérer les enregistrements de la base distante
            using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), DISTANTE))
            {
                try
                {
                    listeModeleDistante = dalBddDistante.GetAllModeles();
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

                    listeModeleLocale = dalBddLocale.GetAllModeles();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void afficherResultat()
        {
            string resultat = changements ? "Synchronisation réussie" : "Tout est à jour";
            Console.WriteLine(resultat);
            Console.WriteLine("correspondance des ID de la table {0}:\n", NomModele);
            Console.WriteLine("SQLITE".PadRight(10) + "MYSQL\n");
            foreach (var cor in correspondanceModeleId)
            {
                Console.WriteLine(cor.Key.ToString().PadRight(10) + cor.Value);
            }
            Console.ReadKey();
        }

    }
}
