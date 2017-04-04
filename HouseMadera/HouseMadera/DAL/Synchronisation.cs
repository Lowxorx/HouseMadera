using HouseMadera.Utilites;
using System;
using System.Collections.Generic;


namespace HouseMadera.DAL
{
    public class Synchronisation<TDAL, TMODELE> : Synchronisation
        where TDAL : DAL, IDAL<TMODELE>
        where TMODELE : ISynchronizable, new()
    {
        private const string DISTANTE = "MYSQL";
        private const string LOCALE = "SQLITE";


        private TDAL dalBddLocale;
        private TDAL dalBddDistante;
        public string NomModele { get; set; }
        private bool _isTableLiaison;

        private List<TMODELE> listeModeleDistante = new List<TMODELE>();
        private List<TMODELE> listeModeleLocale = new List<TMODELE>();
        public static Dictionary<int, int> CorrespondanceModeleId = new Dictionary<int, int>();

        private bool changements = false;

        public Synchronisation(TMODELE modele, bool isTableLiaison = false)
        {
            NomModele = typeof(TMODELE).Name;
            _isTableLiaison = isTableLiaison;
#if DEBUG
            Console.WriteLine(string.Format("************ Synchronisation du modele {0} ************", NomModele));
#endif
            recupererDonnees();
        }

        /// <summary>
        /// Compare les données de la base locale et distante dans les deux sens.
        /// Créé la table de correspondance des Id et affiche le résultat dans la console
        /// </summary>
        public void synchroniserDonnees()
        {
            comparerDonnees(listeModeleLocale, listeModeleDistante, "SORTANTE");
            comparerDonnees(listeModeleDistante, listeModeleLocale, "ENTRANTE");
            //enregistrer les id dans une table de correspondance
            if (!_isTableLiaison)
                creerTableCorrespondance();
            NbModeleSynchronise++;
#if DEBUG
            
            //afficherResultat();
#endif
        }

        /// <summary>
        /// Compare chaque élément de la liste locale et de la liste distante. Si les deux objets sont égaux alors la fonction créé une entrée dans le dictionnaire
        /// La clé représente l'ID de la table locale et la valeur représente l'ID de la table distante
        /// </summary>
        private void creerTableCorrespondance()
        {
            try
            {
                recupererDonnees();
                foreach (TMODELE modeleLocal in listeModeleLocale)
                {
                    foreach (TMODELE modeleDistant in listeModeleDistante)
                    {
                        if (modeleLocal.Equals(modeleDistant))
                        {
                            CorrespondanceModeleId.Add(modeleLocal.Id, modeleDistant.Id);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NbErreurs++;
                string titre = string.Format("Synchronisation de : {0} --> Création de la table de correspondance \n", NomModele);
                Logger.WriteTrace(titre);
                Logger.WriteEx(ex);
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
                case "SORTANTE":
                    bdd = "locale";
                    locale = LOCALE;
                    distante = DISTANTE;
                    break;
                //du distant au local
                case "ENTRANTE":
                    bdd = "distante";
                    locale = DISTANTE;
                    distante = LOCALE;
                    break;

            }

            Console.WriteLine("validation  {0} : ", sens);

            if (liste1.Count == 0)
                Console.WriteLine("La table ne comporte aucun enregistrement");
            try
            {
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
                                    //Console.WriteLine("\nl'entité {0} avec l'ID {1} a été effacé sur la bdd  {2}\n", NomModele, modeleListe1.Id, bdd);
                                    dalBddLocale.DeleteModele(modeleListe1);
                                    //Console.WriteLine("\nSuppression ----- OK");
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
                                        //Console.WriteLine("\nL'entité {0} avec l'ID {1} a été modifié sur la bdd {2}\n", NomModele, modeleListe1.Id, bdd);
                                        dalBddLocale.UpdateModele(modeleListe1, modeleListe2);
                                        //Console.WriteLine("\nModification -------> OK");
                                    }
                                }
                            }
                            break;
                        }
                    }
                    if (!isInDistant)
                    {
                        //Console.WriteLine("L'entité {0} avec l'ID {1} n'existe pas sur dans la bdd {2}\n", NomModele, modeleListe1.Id, distante);
                        using (dalBddDistante = (TDAL)Activator.CreateInstance(typeof(TDAL), distante))
                        {
                            int nbLigneInseree = dalBddDistante.InsertModele(modeleListe1);
                            if (nbLigneInseree > 0)
                            {
                                //Console.WriteLine("Enregistrement dans {0} --------> OK\n", distante);
                                changements = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NbErreurs++;
                string precision = (sens == "SORTANTE") ? LOCALE + " vers " + DISTANTE : DISTANTE + " vers " + LOCALE;
                string titre = string.Format("Synchronisation de : {0}--> Comparaison des données {1}\n", NomModele, precision);
                Logger.WriteTrace(titre);
                Logger.WriteEx(ex);
            }
        }

        /// <summary>
        /// Récupère tous les enregistrement de la table locale et distante.
        /// </summary>
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
                    NbErreurs++;
                    string titre = string.Format("Synchronisation de : {0} --> Récupération des données de la bdd {1}\n", NomModele, DISTANTE);
                    Logger.WriteTrace(titre);
                    Logger.WriteEx(e);
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
                    NbErreurs++;
                    string titre = string.Format("Synchronisation de : {0} --> Récupération des données de la bdd {1}\n", NomModele, LOCALE);
                    Logger.WriteTrace(titre);
                    Logger.WriteEx(e);
                }
            }
        }

#if DEBUG 
        /// <summary>
        /// Affiche dans la console toutes les entrées du dictionnaire
        /// </summary>
        public void afficherResultat()
        {
            string resultat = changements ? "Synchronisation réussie" : "Tout est à jour";
            Console.WriteLine(resultat);
            Console.WriteLine("correspondance des ID de la table {0}:\n", NomModele);
            Console.WriteLine("SQLITE".PadRight(10) + "MYSQL\n");
            foreach (var cor in CorrespondanceModeleId)
            {
                Console.WriteLine(cor.Key.ToString().PadRight(10) + cor.Value);
            }
            Console.ReadKey();
        }
#endif

    }


}
