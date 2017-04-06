using Mono.Data.Sqlite;
using SimpleSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public List<GameObject> listCloison = new List<GameObject>();
    public List<GameObject> listModule = new List<GameObject>();
    public List<GameObject> moduleWall1 = new List<GameObject>();
    public List<GameObject> moduleWall2 = new List<GameObject>();
    public List<GameObject> moduleWall3 = new List<GameObject>();
    public List<GameObject> moduleWall4 = new List<GameObject>();
    public SimpleSQLManager dbManager;
    int gammeChoosed = 2;

    void Start()
    {
         
        try
        {
            var modulePlace = new List<ModulePlace>(from mPlace in dbManager.Table<ModulePlace>() select mPlace);
            Debug.Log("Database Readed");
        }
        catch
        {
            dbManager.CreateTable<ModulePlace>();
        }
    }

    //new
    public void Save()
    {
        DeleteModulePlace();
        FillInformations();
        InsertModulePlace();

    }

    //new
    void DeleteModulePlace()
    {
        string sql = "UPDATE moduleplace SET Suppression = ? WHERE Produit_Id = 1";
        dbManager.Execute(sql, DateTime.Now.ToString());
    }

    //new
    void InsertModulePlace()
    {
        InsertCloison();
        InsertModule();
    }

    // finir l'Insertion (cloison finis, reste mur/porte/fenêtre)
    void Insertion(GameObject objet, string type)
    {
        Debug.Log("Insertion");
        ModulePlace module = new ModulePlace();
        module.Produit_Id = 1;
        DateTime time = DateTime.Now;
        module.Creation = time.ToString("yyyy-MM-dd HH:mm:ss.fff");
        List<Gamme> gamme = new List<Gamme>(from mGamme in dbManager.Table<Gamme>() where mGamme.Id == gammeChoosed select mGamme);
        string nomGamme = gamme[0].Nom;
        List<Module> modules = new List<Module>(from mModule in dbManager.Table<Module>() select mModule);

        switch (type)
        {
            case "Cloison":
                module.Libelle = objet.name;
                foreach (Module mod in modules)
                {
                    if(mod.Nom.Contains(nomGamme) && (mod.Nom.Contains(type)))
                    {
                        module.Module_Id = mod.Id;
                    }
                }
                if (objet.transform.GetChild(0).GetComponent<CloisonManager>().verticalActive)
                {
                    module.Vertical = 1;
                }
                else
                {
                    module.Vertical = 0;
                }

                if (objet.transform.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    module.Horizontal = 1;
                }
                else
                {
                    module.Horizontal = 0;
                }
                break;
            case "Porte":
                string wallNumber = Regex.Match(objet.transform.parent.parent.name, @"\d+").Value;
                module.Libelle = "Door - " + wallNumber;
                foreach (Module mod in modules)
                {
                    if (mod.Nom.Contains(nomGamme) && (mod.Nom.Contains(type)))
                    {
                        module.Module_Id = mod.Id;
                    }
                }
                break;
            case "Fenetre":
                string windowNumber = Regex.Match(objet.transform.parent.parent.name, @"\d+").Value;
                module.Libelle = "Fenetre - " + windowNumber;
                foreach (Module mod in modules)
                {
                    if (mod.Nom.Contains(nomGamme) && (mod.Nom.Contains(type)))
                    {
                        module.Module_Id = mod.Id;
                    }
                }
                break;
            case "Mur":
                foreach (Module mod in modules)
                {
                    if (mod.Nom.Contains(nomGamme) && (mod.Nom.Contains(type)))
                    {
                        module.Module_Id = mod.Id;
                    }
                }
                break;
        }

        List<SlotPlace> slotPlaces = new List<SlotPlace>(from mSlotPlace in dbManager.Table<SlotPlace>() select mSlotPlace);
        foreach (SlotPlace slo in slotPlaces)
        {
            if (type.Equals("Cloison") || type.Equals("Mur"))
            {
                if(slo.Libelle.Contains(Regex.Match(objet.name, @"\d+").Value))
                {
                    module.SlotPlace_Id = slo.Id;
                }
            }
            else if (type.Equals("Porte") || type.Equals("Fenetre"))
            {
                if (slo.Libelle.Contains(Regex.Match(objet.transform.parent.parent.name, @"\d+").Value) && slo.Libelle.Contains("Wall"))
                {
                    module.SlotPlace_Id = slo.Id;
                }
            }
        }
        dbManager.Insert(module);
        Debug.Log("Insered");
    }

    void InsertModule()
    {
        foreach (GameObject target in listModule)
        {
            if (target.name.Contains("Window"))
            {
                Insertion(target, "Fenetre");
            }
            else if (target.name.Contains("Door"))
            {
                Insertion(target, "Porte");
            }
        }
    }

    //new
    void InsertCloison()
    {
        foreach (GameObject target in listCloison)
        {
            Insertion(target, "Cloison");
        }
    }

    //new
    public void FillInformations()
    {
        FillListCloison();
        FillListModule();
    }

    //new
    void FillListCloison()
    {
        listCloison.Clear();
        GameObject house = GameObject.Find("House");
        foreach (Transform target in house.transform)
        {
            if (target.name.Contains("Cloison"))
            {
                if (target.GetChild(0).GetComponent<CloisonManager>().verticalActive || target.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    listCloison.Add(target.gameObject);
                }
            }
        }
    }

    //new
    void FillListModule()
    {
        listModule.Clear();
        GameObject house = GameObject.Find("House");
        foreach (Transform target in house.transform)
        {
            if (target.name.Contains("Wall"))
            {
                foreach (Transform targetSlot in target)
                {
                    if (targetSlot.name.Contains("Slot"))
                    {
                        if (targetSlot.transform.childCount > 0)
                        {
                            listModule.Add(targetSlot.GetChild(0).gameObject);
                        }
                    }
                }
            }
        }
    }

    public void LoadProduit()
    {
        LoadWall();
        LoadCloison();
    }

    void LoadCloison()
    {
        int idProduit = Int32.Parse(GameObject.Find("UIManager").GetComponent<UIParameter>().produits);
        var cloisons = new List<ModulePlace>(from mPlace in dbManager.Table<ModulePlace>() where mPlace.Produit_Id == idProduit && mPlace.Suppression == null select mPlace);

        foreach (ModulePlace test in cloisons)
        {
            if (test.Libelle.Contains("Cloison"))
            {
                GameObject cloison = GameObject.Find(test.Libelle);
                if (test.Vertical == 1)
                {
                    cloison.transform.GetChild(2).gameObject.SetActive(true);
                }
                if (test.Horizontal == 1)
                {
                    cloison.transform.GetChild(3).gameObject.SetActive(true);
                }
            }
            else if (test.Libelle.Contains("Fenetre"))
            {
                Debug.Log("Fenetre");
                GameObject window = new GameObject();
                window.name = "window";
                if (test.Libelle.Contains("1"))
                {
                    moduleWall1.Add(window);
                }
                if (test.Libelle.Contains("2"))
                {
                    moduleWall2.Add(window);
                }
                if (test.Libelle.Contains("3"))
                {
                    moduleWall3.Add(window);
                }
                if (test.Libelle.Contains("4"))
                {
                    moduleWall4.Add(window);
                }
            }
            else if (test.Libelle.Contains("Door"))
            {
                Debug.Log("Porte");
                GameObject door = new GameObject();
                door.name = "door";
                if (test.Libelle.Contains("1"))
                {
                    moduleWall1.Add(door);
                }
                if (test.Libelle.Contains("2"))
                {
                    moduleWall2.Add(door);
                }
                if (test.Libelle.Contains("3"))
                {
                    moduleWall3.Add(door);
                }
                if (test.Libelle.Contains("4"))
                {
                    moduleWall4.Add(door);
                }
            }

            int wall1count = moduleWall1.Count();
            int wall2count = moduleWall2.Count();
            int wall13count = moduleWall3.Count();
            int wall4count = moduleWall4.Count();
            
        }
        GameObject.Find("SelectionModuleGeneral").GetComponent<PanelModule>().InstantiateModule(moduleWall1, GameObject.Find("Wall1"));
        GameObject.Find("SelectionModuleGeneral").GetComponent<PanelModule>().InstantiateModule(moduleWall2, GameObject.Find("Wall2"));
        GameObject.Find("SelectionModuleGeneral").GetComponent<PanelModule>().InstantiateModule(moduleWall3, GameObject.Find("Wall3"));
        GameObject.Find("SelectionModuleGeneral").GetComponent<PanelModule>().InstantiateModule(moduleWall4, GameObject.Find("Wall4"));
    }

    void LoadWall()
    {
        for (int i = 1; i != 6; i++)
        {
            GameObject wall = GameObject.Find("Wall" + i);
            if (wall != null)
            {
                wall.GetComponent<WallSelection>().wallPlaced = true;
                wall.transform.GetChild(0).gameObject.SetActive(true);
                wall.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        
    }

    void LoadModule()
    {
        int idProduit = Int32.Parse(GameObject.Find("UIManager").GetComponent<UIParameter>().produits);
        var cloisons = new List<ModulePlace>(from mPlace in dbManager.Table<ModulePlace>() where mPlace.Produit_Id == idProduit && mPlace.Suppression == null select mPlace);
    }
}
