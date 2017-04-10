using Mono.Data.Sqlite;
using SimpleSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UIParameter : MonoBehaviour
{

    public string projets = "";
    public string produits = "";
    public string commercialId = "";
    public GameObject panelCreation;
    public InputField nameInput;
    public SimpleSQLManager dbManager;
    void Start()
    {
        ReadParameters();
    }

    void Update()
    {

    }

    void ReadParameters()
    {
        if(produits != "")
        {
            int value;
            Int32.TryParse(projets, out value);
            GetCommercialInformations(value);
            GetProductInformation(value);
            GameObject.Find("DBManager").GetComponent<DBManager>().LoadProduit();
        }
        else
        {
            string[] args = Environment.GetCommandLineArgs();
            Debug.Log(args[0]);
            if (args.Length > 1)
            {

                if (args.Length == 2)
                {
                    projets = args[1];
                    int value;
                    Int32.TryParse(projets, out value);
                    GetCommercialInformations(value);
                    GetProductInformation(value);
                    panelCreation.SetActive(true);
                }
                else if (args.Length == 3)
                {
                    projets = args[1];
                    produits = args[2];
                    int value;
                    Int32.TryParse(projets, out value);
                    GetCommercialInformations(value);
                    GetProductInformation(value);
                    GameObject.Find("DBManager").GetComponent<DBManager>().LoadProduit();
                }
            }
        }
        
        
    }

    void GetCommercialInformations(int index)
    {
        var projet = new List<Projet>(from mProjet in dbManager.Table<Projet>() where mProjet.Id == index select mProjet);
        foreach (Projet target in projet)
        {
            var commercial = new List<Commercial>(from mCommercial in dbManager.Table<Commercial>() where mCommercial.Id == target.Commercial_Id select mCommercial);
            foreach (Commercial com in commercial)
            {
                GameObject.Find("UIManager").GetComponent<UIManager>().commercialName.text = com.Nom + " " + com.Prenom;
            }
        }
    }

    public void NewProduit()
    {
        Produit product = new Produit();
        product.Nom = nameInput.text;
        product.Projet_Id = Int32.Parse(projets);
        DateTime date = DateTime.Now;
        product.Creation = date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        product.Plan_Id = 1;
        product.Devis_Id = null;
        product.StatutProduit_Id = 1;
        dbManager.Insert(product);
        panelCreation.SetActive(false);
        var produit = new List<Produit>(from mProduit in dbManager.Table<Produit>() where mProduit.Nom == product.Nom && mProduit.Projet_Id == product.Projet_Id select mProduit);
        foreach (Produit prod in produit)
        {
            produits = prod.Id.ToString();
        }
    }

    void GetProductInformation(int index)
    {
        var projet = new List<Projet>(from mProjet in dbManager.Table<Projet>() where mProjet.Id == index select mProjet);
        foreach (Projet target in projet)
        {
            GameObject.Find("UIManager").GetComponent<UIManager>().projectName.text = target.Nom;
        }
    }
}
