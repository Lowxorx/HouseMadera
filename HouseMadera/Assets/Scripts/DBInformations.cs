using Mono.Data.Sqlite;
using SimpleSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class DBInformations : MonoBehaviour
{
    #region TypeModule
    public int typeModuleDoor;
    public int typeModuleWindow;
    public int typeModuleWall;
    public int typeModuleCloison;
    #endregion

    #region Slot
    public int slotWall;
    public int slotWindow;
    public int slotSol;
    #endregion

    #region Gamme
    public int luxe;
    public int lowCost;
    #endregion

    #region TypeModulePlacable
    public int fpf;
    public int p;
    public int ff;
    #endregion

    public SimpleSQLManager dbManager;

    void Start()
    {
        FillTypeModule();
        FillSlot();
        FillGamme();
        FillModulePlacable();
    }

    void FillTypeModule()
    {
        try
        {
            var typeModule = new List<TypeModule>(from mPlace in dbManager.Table<TypeModule>() select mPlace);
            foreach(TypeModule type in typeModule)
            {
                switch (type.Nom)
                {
                    case "Mur":
                        typeModuleWall = type.Id;
                        break;
                    case "Fenetre":
                        typeModuleWindow = type.Id;
                        break;
                    case "Porte":
                        typeModuleDoor = type.Id;
                        break;
                    case "Cloison":
                        typeModuleCloison = type.Id;
                        break;
                }
            }
        }
        catch
        {

        }
    }

    void FillSlot()
    {
        try
        {
            var slot = new List<Slot>(from mSlot in dbManager.Table<Slot>() select mSlot);
            foreach(Slot slots in slot)
            {
                switch (slots.Nom)
                {
                    case "SlotMur":
                        slotWall = slots.Id;
                        break;
                    case "SlotFenetre":
                        slotWindow = slots.Id;
                        break;
                    case "SlotSol":
                        slotSol = slots.Id;
                        break;
                }
            }
        }
        catch
        {

        }
    }

    void FillGamme()
    {
        try
        {
            var listGamme = new List<Gamme>(from mGamme in dbManager.Table<Gamme>() select mGamme);
            foreach(Gamme gamme in listGamme)
            {
                switch (gamme.Nom)
                {
                    case "Luxe":
                        luxe = gamme.Id;
                        break;
                    case "Low-Cost":
                        lowCost = gamme.Id;
                        break;
                }
            }
        }
        catch
        {

        }
    }

    void FillModulePlacable()
    {
        try
        {
            var listeModulePlacable = new List<TypeModulePlacable>(from mTypeModule in dbManager.Table<TypeModulePlacable>() select mTypeModule);
            foreach (TypeModulePlacable type in listeModulePlacable)
            {
                Debug.Log(type.nom);
                switch (type.nom)
                {
                    case "Fenêtre - Porte - Fenêtre":
                        fpf = type.id;
                        break;
                    case "Porte":
                        p = type.id;
                        break;
                    case "Fenêtre - Fenêtre":
                        ff = type.id;
                        break;
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

}
