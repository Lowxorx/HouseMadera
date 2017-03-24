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

    void InsertModule()
    {

    }

    //new
    void InsertCloison()
    {
        foreach (GameObject target in listCloison)
        {
            ModulePlace module = new ModulePlace();
            module.Produit_Id = 1;
            module.Libelle = target.name;
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

            module.Module_Id = 1;
            module.SlotPlace_Id = 1;
            dbManager.Insert(module);
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
