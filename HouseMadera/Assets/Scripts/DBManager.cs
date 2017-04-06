using Mono.Data.Sqlite;
using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public List<GameObject> listCloison = new List<GameObject>();
    public List<GameObject> listModule = new List<GameObject>();
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
        dbManager.Execute(sql, System.DateTime.Now.ToString());
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
        module.Creation = System.DateTime.Now.ToString();
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
        //foreach (GameObject target in listModule)
        //{
        //    ModulePlace module = new ModulePlace();
        //    module.Produit_Id = 1;
        //    module.Libelle = target.name;
        //    List<Gamme> gamme = new List<Gamme>(from mGamme in dbManager.Table<Gamme>() where mGamme.Id == gammeChoosed select mGamme);
        //    string nomGamme = gamme[0].Nom;
        //    List<Module> modules = new List<Module>(from mModule in dbManager.Table<Module>() select mModule);
        //    foreach(Module mod in modules)
        //    {

        //    }
        //}

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
}
