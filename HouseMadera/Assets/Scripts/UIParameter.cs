using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

public class UIParameter : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        ReadParameters();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ReadParameters()
    {
        try
        {
            string[] args = System.Environment.GetCommandLineArgs();
            string projets = "";
            string produits = "";
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (i)
                    {
                        case 1:
                            projets = args[i];
                            break;
                        case 2:
                            produits = args[i];
                            break;
                    }
                }

                if (projets != "")
                {
                    //GameObject.Find("UIManager").GetComponent<UIManager>().errorMessage.text = projets +" "+produits;
                    int value;
                    Int32.TryParse(projets, out value);
                    GetCommercialInformations(value);
                }

                if (produits != "")
                {
                    int value;
                    Int32.TryParse(projets, out value);
                    GetProductInformation(value);
                }
            }

            
        }
        catch(Exception e)
        {
            //GameObject.Find("UIManager").GetComponent<UIManager>().errorMessage.text = e.ToString();
        }
    }

    void GetCommercialInformations(int index)
    {
        string conn = "URI=file:C:\\HouseMaderaDB-sqlite\\HouseMaderaDB.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        string sqlQuery = "SELECT Commercial_Id FROM projets WHERE Id = " + index;
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string sqlQueryCommercial = "SELECT Nom, Prenom FROM commercials WHERE Id = " + reader.GetInt32(0);
            IDbCommand dbcmdCommercial = dbconn.CreateCommand();
            dbcmdCommercial.CommandText = sqlQueryCommercial;
            IDataReader readerCommercial = dbcmdCommercial.ExecuteReader();
            while (readerCommercial.Read())
            {
                GameObject.Find("UIManager").GetComponent<UIManager>().commercialName.text = readerCommercial.GetString(0) + " " + readerCommercial.GetString(1);
            }
        }
        dbconn.Close();
    }

    void GetProductInformation(int index)
    {
        string conn = "URI=file:C:\\HouseMaderaDB-sqlite\\HouseMaderaDB.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        string sqlQuery = "SELECT Nom FROM produits WHERE Id = " + index;
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            GameObject.Find("UIManager").GetComponent<UIManager>().projectName.text = reader.GetString(0);
        }
        dbconn.Close();
    }
}
