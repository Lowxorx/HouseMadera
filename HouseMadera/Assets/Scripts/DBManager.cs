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
    int idDoor;
    int idWindow;
    int idCloison;
    int gammeChoosed = 1;

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
        List<ModulePlace> modulePlace = new List<ModulePlace>(from mPlace in dbManager.Table<ModulePlace>() select mPlace);
        foreach (ModulePlace target in modulePlace)
        {
            if (target.Produit_Id.ToString().Equals(GameObject.Find("UIManager").GetComponent<UIParameter>().produits))
            {
                dbManager.Delete(target);
            }
        }
    }

    //new
    void InsertModulePlace()
    {
        InsertCloison();
    }

    void Insertion(string name)
    {
        foreach (GameObject target in listModule)
        {
            ModulePlace module = new ModulePlace();
            module.Produit_Id = 1;
            module.Libelle = target.name;
            List<Gamme> gamme = new List<Gamme>(from mGamme in dbManager.Table<Gamme>() where mGamme.Id == gammeChoosed select mGamme);
            string nomGamme = gamme[0].Nom;
            List<Module> modules = new List<Module>(from mModule in dbManager.Table<Module>() select mModule);
            foreach(Module mod in modules)
            {
                if (mod.Nom.Contains(nomGamme) && (mod.Nom.Contains("Cloison"))
                {

                }
            }
        }
    }

    void InsertModule()
    {
        foreach (GameObject target in listModule)
        {
            ModulePlace module = new ModulePlace();
            module.Produit_Id = 1;
            module.Libelle = target.name;
            List<Gamme> gamme = new List<Gamme>(from mGamme in dbManager.Table<Gamme>() where mGamme.Id == gammeChoosed select mGamme);
            string nomGamme = gamme[0].Nom;
            List<Module> modules = new List<Module>(from mModule in dbManager.Table<Module>() select mModule);
            foreach(Module mod in modules)
            {

            }
        }
    }

    //new
    void InsertCloison()
    {
        foreach (GameObject target in listCloison)
        {
            ModulePlace module = new ModulePlace();
            module.Produit_Id = 1;
            module.Libelle = target.name;
            List<Gamme> gamme = new List<Gamme>(from mGamme in dbManager.Table<Gamme>() where mGamme.Id == gammeChoosed select mGamme);
            string nomGamme = gamme[0].Nom;
            List<Module> modules = new List<Module>(from mModule in dbManager.Table<Module>()  select mModule);
            foreach(Module mod in modules)
            {
                if (mod.Nom.Contains(nomGamme) && mod.Nom.Contains("Cloison"))
                {
                    module.Module_Id = mod.Id;
                    Debug.Log(mod.Id);
                } 
            }
            List<SlotPlace> slotPlaces = new List<SlotPlace>(from mSlotPlace in dbManager.Table<SlotPlace>() select mSlotPlace);
            foreach(SlotPlace slo in slotPlaces)
            {
                if(slo.Libelle.Contains(Regex.Match(target.name, @"\d+").Value))
                {
                    module.SlotPlace_Id = slo.Id;
                }
            }
            if (target.transform.GetChild(0).GetComponent<CloisonManager>().verticalActive)
            {
                module.Vertical = 1;
            }
            else
            {
                module.Vertical = 0;
            }

            if (target.transform.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
            {
                module.Horizontal = 1;
            }
            else
            {
                module.Horizontal = 0;
            }
            //dbManager.Insert(module);
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
