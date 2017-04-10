using Mono.Data.Sqlite;
using SimpleSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DBManager : MonoBehaviour
{
    public List<GameObject> listCloison = new List<GameObject>();
    public List<GameObject> listModule = new List<GameObject>();
    public List<GameObject> listWall = new List<GameObject>();
    public List<GameObject> moduleWall1 = new List<GameObject>();
    public List<GameObject> moduleWall2 = new List<GameObject>();
    public List<GameObject> moduleWall3 = new List<GameObject>();
    public List<GameObject> moduleWall4 = new List<GameObject>();
    public Switch gammeSwitch;
    public SimpleSQLManager dbManager;
    int gammeChoosed;
    public string gammeName;
    public GameObject savePanel;

    void Start()
    {
         
        try
        {
            var modulePlace = new List<ModulePlace>(from mPlace in dbManager.Table<ModulePlace>() select mPlace);
        }
        catch
        {
            dbManager.CreateTable<ModulePlace>();
        }
        FillListWall();
    }

    void FindGamme()
    {
        bool status = gammeSwitch.isOn;
        List<Gamme> gamme = new List<Gamme>(from mGamme in dbManager.Table<Gamme>()select mGamme);
        if (status)
        {
            foreach (Gamme gam in gamme)
            {
                if (gam.Nom.Contains("Luxe"))
                {
                    gammeChoosed = gam.Id;
                    gammeName = gam.Nom;
                }
            }
        }
        else
        {
            foreach (Gamme gam in gamme)
            {
                if (gam.Nom.Contains("Low-Cost"))
                {
                    gammeChoosed = gam.Id;
                    gammeName = gam.Nom;
                }
            }
        }
    }

    //new
    public void Save()
    {
        FindGamme();
        DeleteModulePlace();
        FillInformations();
        InsertModulePlace();

    }

    //new
    void DeleteModulePlace()
    {
        string sql = "UPDATE moduleplace SET Suppression = ? WHERE Produit_Id = " + GameObject.Find("UIManager").GetComponent<UIParameter>().produits;
        dbManager.Execute(sql, DateTime.Now.ToString());
    }

    //new
    void InsertModulePlace()
    {
        try
        {
            InsertCloison();
            InsertModule();
            savePanel.GetComponent<MessageSave>().Message("Produit Sauvegardé", Color.green);
        }
        catch
        {
            savePanel.GetComponent<MessageSave>().Message("Erreur lors de la sauvegarde", Color.red);
        }
        
    }

    public void GetGamme()
    {
        FillListModule();
        FillListCloison();
        FillListWall();
        bool status = gammeSwitch.isOn;
        Debug.Log(status);
        if (status)
        {
            foreach(GameObject walls in listWall)
            {
                walls.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().Murluxe;
            }

            foreach(GameObject fenetre in listModule)
            {
                if (fenetre.name.Contains("Window"))
                {
                    fenetre.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().FenetreLuxe;
                    fenetre.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            foreach (GameObject fenetre in listModule)
            {
                if (fenetre.name.Contains("Door"))
                {
                    fenetre.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().PorteLuxe;
                    fenetre.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            GameObject house = GameObject.Find("House");
            foreach (Transform target in house.transform)
            {
                if (target.name.Contains("Cloison"))
                {
                    target.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().ParquetLuxe;
                }
            }


            foreach (GameObject cloison in listCloison)
            {
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().verticalActive)
                {
                    cloison.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                }
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    cloison.transform.GetChild(3).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                }
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().verticalArch)
                {
                    foreach (Transform arch in cloison.transform)
                    {
                        Debug.Log(arch.gameObject.name);
                        if (arch.name.Contains("Arch"))
                        {
                            arch.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                            arch.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                            arch.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                        }
                    }
                }

                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().horizontalArch)
                {
                    foreach (Transform arch in cloison.transform)
                    {
                        Debug.Log(arch.gameObject.name);
                        if (arch.name.Contains("Arch"))
                        {
                            arch.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                            arch.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                            arch.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                        }
                    }
                }
            }

        }
        else
        {
            foreach (GameObject walls in listWall)
            {
                walls.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().Murlowcost;
            }

            foreach (GameObject fenetre in listModule)
            {
                if (fenetre.name.Contains("Window"))
                {
                    fenetre.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().FenetreLowcost;
                    fenetre.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            foreach (GameObject fenetre in listModule)
            {
                if (fenetre.name.Contains("Door"))
                {
                    fenetre.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().PorteLowcost;
                    fenetre.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            GameObject house = GameObject.Find("House");
            foreach (Transform target in house.transform)
            {
                if (target.name.Contains("Cloison"))
                {
                    target.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().parquetLowcost;
                }
            }


            foreach (GameObject cloison in listCloison)
            {
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().verticalActive)
                {
                    cloison.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                }
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    cloison.transform.GetChild(3).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                }
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().verticalArch)
                {
                    foreach (Transform arch in cloison.transform)
                    {
                        Debug.Log(arch.gameObject.name);
                        if (arch.name.Contains("Arch"))
                        {
                            arch.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                            arch.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                            arch.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                        }
                    }
                }

                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().horizontalArch)
                {
                    foreach (Transform arch in cloison.transform)
                    {
                        Debug.Log(arch.gameObject.name);
                        if (arch.name.Contains("Arch"))
                        {
                            arch.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                            arch.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                            arch.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                        }
                    }
                }
            }
        }
    }

    public void GetGammeWithId(int id)
    {
        FillListModule();
        FillListCloison();
        FillListWall();
        if (id == 1)
        {
            foreach (GameObject walls in listWall)
            {
                walls.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().Murluxe;
            }

            foreach (GameObject fenetre in listModule)
            {
                if (fenetre.name.Contains("Window"))
                {
                    fenetre.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().FenetreLuxe;
                    fenetre.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            foreach (GameObject fenetre in listModule)
            {
                if (fenetre.name.Contains("Door"))
                {
                    fenetre.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().PorteLuxe;
                    fenetre.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            GameObject house = GameObject.Find("House");
            foreach (Transform target in house.transform)
            {
                if (target.name.Contains("Cloison"))
                {
                    target.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().ParquetLuxe;
                }
            }


            foreach (GameObject cloison in listCloison)
            {
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().verticalActive)
                {
                    cloison.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                }
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    cloison.transform.GetChild(3).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                }
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().verticalArch)
                {
                    foreach (Transform arch in cloison.transform)
                    {
                        Debug.Log(arch.gameObject.name);
                        if (arch.name.Contains("Arch"))
                        {
                            arch.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                            arch.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                            arch.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                        }
                    }
                }

                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().horizontalArch)
                {
                    foreach (Transform arch in cloison.transform)
                    {
                        Debug.Log(arch.gameObject.name);
                        if (arch.name.Contains("Arch"))
                        {
                            arch.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                            arch.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                            arch.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLuxe;
                        }
                    }
                }
            }

        }
        if (id == 2)
        {
            foreach (GameObject walls in listWall)
            {
                walls.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().Murlowcost;
            }

            foreach (GameObject fenetre in listModule)
            {
                if (fenetre.name.Contains("Window"))
                {
                    fenetre.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().FenetreLowcost;
                    fenetre.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            foreach (GameObject fenetre in listModule)
            {
                if (fenetre.name.Contains("Door"))
                {
                    fenetre.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().PorteLowcost;
                    fenetre.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            GameObject house = GameObject.Find("House");
            foreach (Transform target in house.transform)
            {
                if (target.name.Contains("Cloison"))
                {
                    target.GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().parquetLowcost;
                }
            }


            foreach (GameObject cloison in listCloison)
            {
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().verticalActive)
                {
                    cloison.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                }
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().horizontalActive)
                {
                    cloison.transform.GetChild(3).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                }
                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().verticalArch)
                {
                    foreach (Transform arch in cloison.transform)
                    {
                        Debug.Log(arch.gameObject.name);
                        if (arch.name.Contains("Arch"))
                        {
                            arch.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                            arch.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                            arch.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                        }
                    }
                }

                if (cloison.transform.GetChild(0).GetComponent<CloisonManager>().horizontalArch)
                {
                    foreach (Transform arch in cloison.transform)
                    {
                        Debug.Log(arch.gameObject.name);
                        if (arch.name.Contains("Arch"))
                        {
                            arch.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                            arch.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                            arch.transform.GetChild(2).GetComponent<Renderer>().material.mainTexture = GameObject.Find("UIManager").GetComponent<UITextures>().CloisonLowcost;
                        }
                    }
                }
            }
        }
    }

    void Insertion(GameObject objet, string type)
    {
        try
        {
            Debug.Log("Insertion");
            ModulePlace module = new ModulePlace();
            module.Produit_Id = Int32.Parse(GameObject.Find("UIManager").GetComponent<UIParameter>().produits);
            DateTime time = DateTime.Now;
            module.Creation = time.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            int nomGamme = gammeChoosed;
            List<Module> modules = new List<Module>(from mModule in dbManager.Table<Module>() select mModule);

            switch (type)
            {
                case "Cloison":
                    module.Libelle = objet.name;
                    foreach (Module mod in modules)
                    {
                        if (mod.Nom.Contains(gammeName) && (mod.Nom.Contains(type)))
                        {
                            module.Module_Id = mod.Id;
                        }
                    }
                    if (objet.transform.GetChild(0).GetComponent<CloisonManager>().verticalActive || objet.transform.GetChild(0).GetComponent<CloisonManager>().verticalArch)
                    {
                        if (objet.transform.GetChild(0).GetComponent<CloisonManager>().verticalArch)
                        {
                            module.Vertical = 2;
                        }
                        else
                        {
                            module.Vertical = 1;
                        }
                    }
                    else
                    {
                        module.Vertical = 0;
                    }

                    if (objet.transform.GetChild(0).GetComponent<CloisonManager>().horizontalActive || objet.transform.GetChild(0).GetComponent<CloisonManager>().horizontalArch)
                    {
                        if (objet.transform.GetChild(0).GetComponent<CloisonManager>().horizontalArch)
                        {
                            module.Horizontal = 2;
                        }
                        else
                        {
                            module.Horizontal = 1;
                        }
                    }
                    else
                    {
                        module.Horizontal = 0;
                    }
                    break;
                case "Porte":
                    string wallNumber = Regex.Match(objet.transform.parent.parent.name, @"\d+").Value;
                    module.Libelle = "Door - " + wallNumber;
                    if (module.Libelle.Contains("Luxe"))
                    {
                        module.Module_Id = GameObject.Find("DBManager").GetComponent<DBInformations>().porteLuxe;
                    }
                    else
                    {
                        module.Module_Id = GameObject.Find("DBManager").GetComponent<DBInformations>().porteLowCost;
                    }
                    break;
                case "Fenetre":
                    string windowNumber = Regex.Match(objet.transform.parent.parent.name, @"\d+").Value;
                    module.Libelle = "Fenetre - " + windowNumber;
                    if (module.Libelle.Contains("Luxe"))
                    {
                        module.Module_Id = GameObject.Find("DBManager").GetComponent<DBInformations>().fenetreLuxe;
                    }
                    else
                    {
                        module.Module_Id = GameObject.Find("DBManager").GetComponent<DBInformations>().fenetreLowcost;
                    }
                    break;
                case "Mur":
                    foreach (Module mod in modules)
                    {
                        if (mod.Nom.Contains(nomGamme.ToString()) && (mod.Nom.Contains(type)))
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
                    if (slo.Libelle.Contains(Regex.Match(objet.name, @"\d+").Value))
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
        catch(Exception e)
        {
            GameObject.Find("ERROR").GetComponent<Text>().text = e.ToString();
        }
        
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

    void FillListWall()
    {
        listWall.Clear();
        GameObject house = GameObject.Find("House");
        foreach (Transform target in house.transform)
        {
            if (target.name.Contains("Wall"))
            {
                listWall.Add(target.gameObject);
            }
        }
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
                if (target.GetChild(0).GetComponent<CloisonManager>().verticalActive || target.GetChild(0).GetComponent<CloisonManager>().horizontalActive || target.GetChild(0).GetComponent<CloisonManager>().verticalArch || target.GetChild(0).GetComponent<CloisonManager>().horizontalArch)
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
        GetGammeWithId(gammeChoosed);
    }

    void InstantiateDoor(GameObject parent)
    {
        GameObject arch = Instantiate(Resources.Load("Arch", typeof(GameObject))) as GameObject;
        arch.transform.parent = parent.transform.parent;
        arch.transform.position = parent.transform.position;
        if (parent.name.Contains("Vertical"))
        {
            arch.transform.rotation = Quaternion.Euler(0, 90, 0);
            arch.transform.GetComponent<Transform>().localScale = new Vector3(parent.transform.GetComponent<Transform>().localScale.z, parent.transform.GetComponent<Transform>().localScale.y, parent.transform.GetComponent<Transform>().localScale.x);
            parent.transform.parent.GetChild(0).GetComponent<CloisonManager>().horizontalActive = false;
            parent.transform.parent.GetChild(1).GetComponent<CloisonManager>().horizontalActive = false;

            parent.transform.parent.GetChild(0).GetComponent<CloisonManager>().verticalArch = true;
            parent.transform.parent.GetChild(1).GetComponent<CloisonManager>().verticalArch = true;
        }
        else
        {
            arch.transform.rotation = Quaternion.Euler(0, 0, 0);
            arch.transform.GetComponent<Transform>().localScale = parent.transform.GetComponent<Transform>().localScale;
            parent.transform.parent.GetChild(0).GetComponent<CloisonManager>().verticalActive = false;
            parent.transform.parent.GetChild(1).GetComponent<CloisonManager>().verticalActive = false;

            parent.transform.parent.GetChild(0).GetComponent<CloisonManager>().horizontalArch = true;
            parent.transform.parent.GetChild(1).GetComponent<CloisonManager>().horizontalArch = true;
        }


        parent.transform.parent.GetChild(2).GetComponent<Renderer>().material.color = Color.white;
        parent.transform.parent.GetChild(3).GetComponent<Renderer>().material.color = Color.white;
        parent.transform.parent.GetChild(2).gameObject.SetActive(false);
        parent.transform.parent.GetChild(3).gameObject.SetActive(false);
        parent = null;
    }

    void LoadCloison()
    {
        int idProduit = Int32.Parse(GameObject.Find("UIManager").GetComponent<UIParameter>().produits);
        var cloisons = new List<ModulePlace>(from mPlace in dbManager.Table<ModulePlace>() where mPlace.Produit_Id == idProduit && mPlace.Suppression == null select mPlace);
        int  gamme = 0;

        foreach (ModulePlace test in cloisons)
        {
            if(gamme == 0)
            {
                int moduleId = test.Module_Id;
                var modul = new List<Module>(from mModule in dbManager.Table<Module>() select mModule);
                gamme = modul[0].Gamme_Id;
                gammeChoosed = gamme;
            }
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
                if (test.Vertical == 2)
                {
                    InstantiateDoor(cloison.transform.GetChild(2).gameObject);
                }
                if (test.Horizontal == 2)
                {
                    InstantiateDoor(cloison.transform.GetChild(3).gameObject);
                }
            }
            else if (test.Libelle.Contains("Fenetre"))
            {
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
